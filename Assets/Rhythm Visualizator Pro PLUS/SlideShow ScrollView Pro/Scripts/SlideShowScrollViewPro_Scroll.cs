﻿// Created by Carlos Arturo Rodriguez Silva (Legend)
// Contact: carlosarturors@gmail.com

using RhythmVisualizatorPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlideShowScrollViewPro {
    public class SlideShowScrollViewPro_Scroll : MonoBehaviour
    {
        public enum Permission { Denied = 0, Granted = 1, ShouldAsk = 2 };

        public static string currentPath = "";

        [HideInInspector]
        public List<GameObject> buttons;
        [HideInInspector]
        public List<GameObject> activeButtons;
        [HideInInspector]
        public List<MySongElementUI> elements;
        [HideInInspector]
        public List<SlideShowScrollViewPro_Group> groups;

        [Header("Variables")]
        public bool vertical;
        [Space(5)]
        public bool startOnGroup = false;
        public int groupDropdownStartValue = 1;

        [Space(5)]
        public bool selectRandomAtStart = true;

        public MySongElementUI selectedElementAtStart;

        [Space(5)]

        [Range(0f, 3f)]
        public float buttonsFadeTime = 0.35f;

        [Range(0f, 3f)]
        public float transitionTime = 0.5f;

        [Range(0f, 3f)]
        public float backgroundFadeTime = 0.35f;

        [Range(0.1f, 3f)]
        public float onPanelChangeFadeTime = 0.2f;

        [Space(5)]

        [Range(-100, 500f)]
        public float imagesSeparation = 100f;

        [Range(-100, 500f)]
        public float groupsSeparation = 50f;

        [Space(5)]

        [Range(10, 500)]
        public float elementsHorizontalSize = 240;
        [Range(10, 500)]
        public float elementsVerticalSize = 135;

        [Space(5)]

        public Vector3 normalScale = Vector3.one;
        public Vector3 selectedScale = new Vector3(1.75f, 1.75f, 1);

        [Space(10)]

        public int selectedElementID = -1;

        public int selectedElementPos = -1;

        int centerButton;


        [Header("Prefabs")]
        public RectTransform center;
        public ScrollRect scrollRect;
        public RectTransform viewport;

        public Image backgroundImage;

        [Space(5)]
        public GameObject imageElementUI;

        [Space(5)]
        public GameObject btnReturn;
        public GameObject btnGroup;

        [Space(5)]
        public FadeCanvas imagesFadeCanvas;
        public RectTransform panelImages;
        public RectTransform parentImagesList;


        [Space(5)]
        public FadeCanvas groupsFadeCanvas;
        public RectTransform panelGroups;
        public RectTransform parentGroupsList;

        [Space(5)]
        public TMP_InputField searchInputField;
        public TMP_Text searchNumResults;

        public TMP_Dropdown OrderByDropDown;
        public TMP_Dropdown GroupByDropDown;

        [Header("Selected Element Data")]
        public TMP_Text elementTitle;
        public TMP_Text elementArtist;

        [Header("Rhythm Visualizator GameObjects")]
        public GameObject musicPlayerCanvas;
        public GameObject rhythmVisualizatorCamera;

        [Header("Rhythm Visualizator GameObjects")]
        public GameObject scrollListCamera;
        public GameObject scrollListCanvas;
        public GameObject scrollListDownsideCanvas;


        [HideInInspector]
        public float[] distance;

        bool active = false;

        int previous = -1;
        int previousSelectedButton = -2;

        float minDistance = int.MaxValue;

        int centerGroupsButton;

        GameObject leftButton;
        GameObject rightButton;

        public static SlideShowScrollViewPro_Scroll instance;

        SlideShowScrollViewPro_Group actualDisplayingGroup;

        void Awake()
        {
            instance = this;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            currentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
#elif UNITY_ANDROID
                string path = "";
				try {
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Environment")) {
						path = androidJavaClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory")
							.Call<string>("getAbsolutePath");
					}
				}
				catch (Exception e) {
					Debug.LogWarning("Error fetching native Android external storage dir: " + e.Message);
					path = Directory.GetCurrentDirectory();
				}
			
                currentPath = path;
#elif UNITY_IOS
            currentPath = Application.streamingAssetsPath;
#else
     // Open the application to load songs from directory
                currentPath = Directory.GetCurrentDirectory();
#endif
        }

        void Start()
        {
            Restart();
        }

        public static Permission RequestPermission()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
			object threadLock = new object();
			lock( threadLock )
			{
				FBPermissionCallbackAndroid nativeCallback = new FBPermissionCallbackAndroid( threadLock );

				AJC.CallStatic( "RequestPermission", Context, nativeCallback, PlayerPrefs.GetInt( "FileBrowserPermission", (int) Permission.ShouldAsk ) );

				if( nativeCallback.Result == -1 )
					System.Threading.Monitor.Wait( threadLock );

				if( (Permission) nativeCallback.Result != Permission.ShouldAsk && PlayerPrefs.GetInt( "FileBrowserPermission", -1 ) != nativeCallback.Result )
				{
					PlayerPrefs.SetInt( "FileBrowserPermission", nativeCallback.Result );
					PlayerPrefs.Save();
				}

				return (Permission) nativeCallback.Result;
			}
#else
            return Permission.Granted;
#endif
        }

#if !UNITY_EDITOR && UNITY_ANDROID
        private static AndroidJavaClass m_ajc = null;
        private static AndroidJavaClass AJC {
            get {
                if (m_ajc == null)
                    m_ajc = new AndroidJavaClass("com.rhythmvisualizatorpro.unity.FileBrowser");

                return m_ajc;
            }
        }

        private static AndroidJavaObject m_context = null;
        private static AndroidJavaObject Context {
            get {
                if (m_context == null) {
                    using (AndroidJavaObject unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                        m_context = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                    }
                }

                return m_context;
            }
        }
#endif

        /// <summary>
        /// Restart the script.
        /// </summary>
        public void Restart()
        {
            previous = -1;
            previousSelectedButton = -2;
            StopAllCoroutines();

            // Fix Input Caret Position for TextMeshPro
            searchInputField.onSelect.AddListener(i => {
                var rect = searchInputField.GetComponentInChildren<TMP_SelectionCaret>().rectTransform;
                rect.anchoredPosition = new Vector2(0, 0);
            });

            // Clean texts
            searchNumResults.text = "";

            // Create groups
            CreateGroups();

            // Deactivate groups
            for (int i = 0; i < groups.Count; i++) {
                groups[i].gameObject.SetActive(false);
            }

            groupsFadeCanvas.FadeOut();

            // Set parent spacing
            parentGroupsList.GetComponent<HorizontalLayoutGroup>().spacing = groupsSeparation;
            parentImagesList.GetComponent<HorizontalLayoutGroup>().spacing = imagesSeparation;

            // Change viewport rotation
            if (vertical) {
                viewport.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else {
                viewport.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            groupsFadeCanvas.FadeTime = onPanelChangeFadeTime;
            imagesFadeCanvas.FadeTime = onPanelChangeFadeTime;


            // Original Songs Path
            string originalPath = currentPath;

            //            Debug.LogFormat("CurrentPath: {0}", currentPath);

            if (string.IsNullOrEmpty(originalPath)) {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                originalPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
#elif UNITY_ANDROID
                string path = "";
				try {
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Environment")) {
						path = androidJavaClass.CallStatic<AndroidJavaObject>("getExternalStorageDirectory")
							.Call<string>("getAbsolutePath");
					}
				}
				catch (Exception e) {
					Debug.LogWarning("Error fetching native Android external storage dir: " + e.Message);
					path = Directory.GetCurrentDirectory();
				}
			
                originalPath = path;
#elif UNITY_IOS
                originalPath = Application.streamingAssetsPath;
#else
     // Open the application to load songs from directory
                originalPath = Directory.GetCurrentDirectory();
#endif
            }


            // Get MP3 songs
            Debug.LogFormat("Searching songs in: {0}", originalPath);
                string[] songsPath = Directory.GetFiles(originalPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".mp3") || s.EndsWith(".wav") || s.EndsWith(".ogg")).ToArray();


            // Destroy all buttons
            if (buttons.Count > 0) {
                for (int i = 0; i < buttons.Count; i++) {
                    DestroyImmediate(buttons[i]);
                }
            }

            // If we get at least one song
            if (songsPath.Length > 0) {

                // Create a button for each song
                try {
                    foreach (string songPath in songsPath) {
                        CreateButtonOnRuntime(songPath);
                    }
                }
                catch (Exception e) {
                    MusicPlayerPlus.instance.songTitle.text = "Error creating buttons";
                    MusicPlayerPlus.instance.songArtist.text = "";
                    Debug.Log(e.Message + " " + e.StackTrace);



                    return;
                }

                // Initiate all
                Initiate();

                // Call this when after you added all your buttons in runtime
                UpdateAll();

            } else { // If there are no songs
                MusicPlayerPlus.instance.songTitle.text = "No songs found";
                MusicPlayerPlus.instance.songArtist.text = "";
            }
        }

        /// <summary>
        /// Creates a button in runtime
        /// </summary>
        /// <param name="path"></param>
        void CreateButtonOnRuntime(string path)
        {
//          Debug.Log(path);
            // Create extra button for the song
            var newButton = Instantiate(imageElementUI, parentImagesList);

            // Get song data

            try {
#if !UNITY_WEBGL
                // Get song title
                var file = TagLib.File.Create(path);
                newButton.GetComponent<MySongElementUI>().mySongCustomData.title = file.Tag.Title;

                // Get the file name 
                string songTitle = Path.GetFileNameWithoutExtension(path);

                // If the song have performers and title, we will use it
                if (file.Tag.Performers.Length > 0 && !string.IsNullOrEmpty(file.Tag.Title)) {
                    if (!string.IsNullOrEmpty(file.Tag.Performers[0])) {
                        newButton.GetComponent<MySongElementUI>().mySongCustomData.artist = file.Tag.Performers[0];
                        songTitle = string.Format("{0} - {1}", file.Tag.Performers[0], file.Tag.Title);
                    }
                }
                else { // Else, we will use the file name
                    newButton.GetComponent<MySongElementUI>().mySongCustomData.title = songTitle;
                }


                // Get genres
                string genres = "";
                if (file.Tag.Genres.Length > 0) {
                    if (!string.IsNullOrEmpty(file.Tag.Genres[0])) {
                        genres = file.Tag.Genres[0];
                    }
                }

                // Assign song data
                newButton.GetComponent<MySongElementUI>().mySongCustomData.genre = genres;

                // Get the song image
                newButton.GetComponent<MySongElementUI>().SetImage(ImportImage.LoadNewSprite(path));

#endif
            } catch (Exception e) {
                newButton.GetComponent<MySongElementUI>().mySongCustomData.title = Path.GetFileNameWithoutExtension(path);
                Debug.LogErrorFormat("Cannot get the taglib data from this song: {0} {1} {2}", path, e.Message, e.StackTrace);
            }


            // Assign song data
            newButton.GetComponent<MySongElementUI>().mySongCustomData.songPath = path;

            newButton.GetComponent<MySongElementUI>().UpdateTextsData();

            buttons.Add(newButton);

//            Debug.Log("Button created in runtime");
        }

        /// <summary>
        /// Recalculate the parent size, recalculate active buttons, and recalculate the center button.
        /// </summary>
        /// <param name="extraButtons"></param>
        public void RecalculateActiveButtonsAndSize()
        {

            int extraButtons = 0;
            if (leftButton != null) {
                extraButtons = 1;
            }

            if (rightButton != null) {
                extraButtons = 2;
            }

            // Set the correct size
            parentImagesList.sizeDelta = new Vector2((buttons.Count + extraButtons) * (elementsHorizontalSize + imagesSeparation), elementsVerticalSize);

            // Calculate active buttons
            activeButtons = CalculateActiveButtons();

            // Calculate center button
            centerButton = (int)(Math.Round(activeButtons.Count / 2f, MidpointRounding.AwayFromZero) - 1);

            if (activeButtons.Count % 2 == 0) {
                parentImagesList.anchoredPosition = new Vector2(elementsHorizontalSize / 2f, parentImagesList.anchoredPosition.y);
            }
            else {
                parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
            }
        }

        /// <summary>
        /// Updates all, call this function when you after added a new button in runtime
        /// </summary>
        public void UpdateAll()
        {
            // Update all
            RecalculateActiveButtonsAndSize();

            elements.Clear();

            // Set the ID to all buttons and assign to elements
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].GetComponent<MySongElementUI>().SetID(i);
                elements.Add(buttons[i].GetComponent<MySongElementUI>());
            }

            if (buttons[selectedElementID].gameObject.activeSelf) {
                StopCoroutine("IE_LerpToSelectedButton");
                StartCoroutine("IE_LerpToSelectedButton");
            }
            else {
                parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
            }

            for (int i = 0; i < activeButtons.Count; i++) {
                activeButtons[i].GetComponent<ShowUIWhenIsVisible>().UpdateNow();
            }
        }

        /// <summary>
        /// Creates the groups.
        /// </summary>
        void CreateGroups()
        {
            groups.Clear();

            // Create group for numbers
            var groupClone = Instantiate(btnGroup, parentGroupsList);

            groupClone.GetComponent<SlideShowScrollViewPro_Group>().indexNumber = 0;
            groupClone.GetComponent<SlideShowScrollViewPro_Group>().GroupName = "0-9";

            groups.Add(groupClone.GetComponent<SlideShowScrollViewPro_Group>());

            // Create one group for every letter
            char[] alpha = SlideShowScrollViewPro_Utilities.ABC();

            for (int i = 0; i < alpha.Length; i++) {
                groupClone = Instantiate(btnGroup, parentGroupsList);
                groupClone.GetComponent<SlideShowScrollViewPro_Group>().GroupName = alpha[i].ToString();
                groupClone.GetComponent<SlideShowScrollViewPro_Group>().indexNumber = i + 1;
                groups.Add(groupClone.GetComponent<SlideShowScrollViewPro_Group>());

            }

            // Create group for any other character
            groupClone = Instantiate(btnGroup, parentGroupsList);
            groupClone.GetComponent<SlideShowScrollViewPro_Group>().GroupName = "Others";
            groups.Add(groupClone.GetComponent<SlideShowScrollViewPro_Group>());

            parentGroupsList.sizeDelta = new Vector2(groups.Count * (elementsHorizontalSize + groupsSeparation), elementsVerticalSize);
        }

        /// <summary>
        /// Assign an Element to a group
        /// </summary>
        /// <param name="elementData"></param>
        /// <param name="buttonObject"></param>
        void AssignToGroup(MySongElementUI elementData, GameObject buttonObject)
        {
            Char firstLetter = ' ';

            if (GroupByDropDown.value == 0) { // No Order

                for (int i = 0; i < buttons.Count; i++) {
                    buttons[i].SetActive(true);
                }

                RecalculateActiveButtonsAndSize();

                return;
            }
            else if (GroupByDropDown.value == 1) { // Per Title
                if (elementData.title.text.Length > 0) {
                    firstLetter = elementData.title.text[0];
                }
            }
            else if (GroupByDropDown.value == 2) { // Per Artist
                if (elementData.artist.text.Length > 0) {
                    firstLetter = elementData.artist.text[0];
                }
            }

            Char.ToUpper(firstLetter);

            bool got = false;

            // If the letter is a number
            if (Char.IsNumber(firstLetter)) {
                for (int i = 0; i < groups.Count; i++) {
                    if (groups[i].GroupName == "0-9") {
                        buttonObject.GetComponent<MySongElementUI>().actualGroup = groups[i];
                        groups[i].IncreaseElementsCount();
                        got = true;
                        break;
                    }
                }
            }

            if (!got) {
                // Search for group with the same letter and assign to it
                for (int i = 0; i < groups.Count; i++) {
                    if (groups[i].GroupName == firstLetter.ToString()) {
                        buttonObject.GetComponent<MySongElementUI>().actualGroup = groups[i];
                        groups[i].IncreaseElementsCount();
                        got = true;
                        break;
                    }
                }
            }

            if (!got) {
                // If the letter doesn't exists then add to Others group
                for (int i = 0; i < groups.Count; i++) {
                    if (groups[i].GroupName == "Others") {
                        buttonObject.GetComponent<MySongElementUI>().actualGroup = groups[i];
                        groups[i].GetComponent<SlideShowScrollViewPro_Group>().indexNumber = SlideShowScrollViewPro_Utilities.ABC().Length + 2;
                        groups[i].IncreaseElementsCount();
                        got = true;
                        break;
                    }

                }
            }

            parentGroupsList.sizeDelta = new Vector2(SlideShowScrollViewPro_Utilities.GetActiveChildCount(parentGroupsList) * (elementsHorizontalSize + groupsSeparation), elementsVerticalSize);

            centerGroupsButton = (int)(Math.Round(SlideShowScrollViewPro_Utilities.GetActiveChildCount(parentGroupsList) / 2f, MidpointRounding.AwayFromZero) - 1);

        }

        /// <summary>
        /// Initialize all
        /// </summary>
        /// <returns></returns>
        void Initiate()
        {
            if (parentImagesList.childCount == 0) {
                Debug.Log("Please assign at least one element in the children of Parent Images List");
                return;
            }

            buttons.Clear();

            // Get children gameobjects and set initial element values
            var children = SlideShowScrollViewPro_Utilities.GetChildren(parentImagesList.gameObject).ToList();

            for (int i = 0; i < children.Count; i++) {
                children[i].GetComponent<LayoutElement>().preferredHeight = elementsVerticalSize;
                children[i].GetComponent<LayoutElement>().preferredWidth = elementsHorizontalSize;
                children[i].GetComponent<MySongElementUI>().gameObject.transform.localScale = normalScale;
                children[i].GetComponent<MySongElementUI>().image.color = Color.gray;
                children[i].GetComponent<MySongElementUI>().SetTextsColor(Color.gray);

                buttons.Add(children[i]);
            }

            distance = new float[buttons.Count];

            elements.Clear();

            // Set position to all buttons and assign to elements
            for (int i = 0; i < buttons.Count; i++) {
                buttons[i].GetComponent<MySongElementUI>().SetID(i);
                elements.Add(buttons[i].GetComponent<MySongElementUI>());
            }

            RecalculateActiveButtonsAndSize();

            active = true;

            // Start first lerp to button
            if (!selectRandomAtStart) {

                if (selectedElementAtStart == null) {
                    Debug.LogWarning("Please assign SelectedElementAtStart prefab; Selecting random button");
                    StartSelectRandom();
                }
                else {
                    StartCoroutine(IE_InitializeWithElement());
                }
            }
            else {
                StartSelectRandom();
            }

            if (startOnGroup) {
                GroupByDropDown.value = groupDropdownStartValue;
                GroupBy_Click();
            }
            else {
                imagesFadeCanvas.FadeIn();
                groupsFadeCanvas.FadeOut();
            }

        }

        /// <summary>
        /// Selects Random button
        /// </summary>
        public void SelectRandom()
        {
            selectedElementID = UnityEngine.Random.Range(0, buttons.Count - 1);
            selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

            ShowElementDataNow();

            StartCoroutine("IE_LerpToSelectedButton");

            buttons[selectedElementID].GetComponent<MySongElementUI>().mySongCustomData.LoadSong(buttons[selectedElementID].GetComponent<MySongElementUI>().image.sprite);

        }

        /// <summary>
        /// Selects a random button and resets the previous selected button.
        /// </summary>
        void StartSelectRandom()
        {
            SelectRandom();

            previous = selectedElementID;
            previousSelectedButton = selectedElementID;

            ShowElementDataNow();

            StartCoroutine("IE_LerpToSelectedButton");
        }


        /// <summary>
        /// Changes the background sprite smooth using Lerp
        /// </summary>
        /// <returns></returns>
        IEnumerator ChangeBackgroundSprite()
        {

            float time = 0;

            var actualColor = backgroundImage.color;
            var newColor = actualColor;
            newColor.a = 0;


            while (time <= backgroundFadeTime) {
                time += Time.unscaledDeltaTime;
                backgroundImage.color = Color.Lerp(actualColor, newColor, time / backgroundFadeTime);

                yield return new WaitForEndOfFrame();
            }

            if (elements[selectedElementID].image.sprite == null) {
                backgroundImage.sprite = null;

            }
            else {
                backgroundImage.sprite = elements[selectedElementID].image.sprite;
            }

            backgroundImage.color = newColor;

            actualColor = backgroundImage.color;
            newColor.a = 1;

            time = 0;

            while (time <= backgroundFadeTime) {
                time += Time.unscaledDeltaTime;

                backgroundImage.color = Color.Lerp(actualColor, newColor, time / backgroundFadeTime);

                yield return new WaitForEndOfFrame();
            }

            backgroundImage.color = newColor;
        }

        /// <summary>
        /// Initializes with an element, show his data and lerps to him.
        /// </summary>
        /// <returns></returns>
        IEnumerator IE_InitializeWithElement()
        {
            selectedElementID = selectedElementAtStart.elementID;

            selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

            StopCoroutine("ChangeBackgroundSprite");

            StartCoroutine("ChangeBackgroundSprite");

            previous = selectedElementID;
            previousSelectedButton = selectedElementID;

            // Show Element Data
            elementTitle.text = elements[selectedElementID].mySongCustomData.title;
            elementArtist.text = string.Format("Version: {0}", elements[selectedElementID].mySongCustomData.artist);

            StartCoroutine(IE_FadeIn());

            StartCoroutine("IE_LerpToSelectedButton");

            yield break;
        }

        /// <summary>
        /// Selects the specified button.
        /// </summary>
        /// <param name="elementPos"></param>
        /// <param name="elementID"></param>
        public void SelectButtonByID_Click(int elementID)
        {

            selectedElementPos = elements[elementID].elementPos;
            selectedElementID = elementID;

            StopCoroutine("IE_LerpToSelectedButton");
            StartCoroutine("IE_LerpToSelectedButton");

            OnSelectionChange();
        }

        string lastText = "";
        /// <summary>
        /// Search elements using the inputfield text.
        /// </summary>
        public void SearchElements_Click()
        {
            var textToSearch = searchInputField.text;

            // Hide all groups
            for (int i = 0; i < groups.Count; i++) {
                groups[i].ElementsCount = 0;
                groups[i].gameObject.SetActive(false);
            }

            int results = 0;

            // Search for elements
            for (int pos = 0; pos < elements.Count; pos++) {
                if (Regex.IsMatch(elements[pos].mySongCustomData.title,
                      textToSearch,
                      RegexOptions.IgnoreCase)) {

                    // Show the button if we are displaying that group
                    if (elements[pos].actualGroup == actualDisplayingGroup) {
                        elements[pos].gameObject.SetActive(true);
                    }

                    if (elements[pos].actualGroup != null) {
                        elements[pos].actualGroup.ElementsCount++;
                        elements[pos].actualGroup.gameObject.SetActive(true);
                    }

                    // Increase results
                    results++;
                    continue;
                }

                if (Regex.IsMatch(elements[pos].mySongCustomData.artist,
                      textToSearch,
                      RegexOptions.IgnoreCase)) {

                    // Show the button if we are displaying that group
                    if (elements[pos].actualGroup == actualDisplayingGroup) {
                        elements[pos].gameObject.SetActive(true);
                    }

                    if (elements[pos].actualGroup != null) {
                        elements[pos].actualGroup.ElementsCount++;
                        elements[pos].actualGroup.gameObject.SetActive(true);
                    }

                    // Increase results
                    results++;
                    continue;
                }

                buttons[pos].SetActive(false);
            }

            // Recalculate
            RecalculateActiveButtonsAndSize();
            selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;
            centerGroupsButton = (int)(Math.Round(SlideShowScrollViewPro_Utilities.GetActiveChildCount(parentGroupsList) / 2f, MidpointRounding.AwayFromZero) - 1);

            // Display results number
            if (searchInputField.text != "") {
                if (results == 1) {
                    searchNumResults.text = "1 result";
                }
                else if (results > 0) {
                    searchNumResults.text = string.Format("{0} results", results);
                }
                else {
                    searchNumResults.text = "No results";
                    parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
                }
            }
            else {
                searchNumResults.text = "";
            }


            if (lastText != searchInputField.text) {
                lastText = searchInputField.text;

                if (buttons[selectedElementID].gameObject.activeSelf) {
                    if (actualDisplayingGroup == buttons[selectedElementID].GetComponent<MySongElementUI>().actualGroup) {
                        StopCoroutine("IE_LerpToSelectedButton");
                        StartCoroutine("IE_LerpToSelectedButton");
                    }
                }
                else {
                    parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
                }
            }

            ShowVisibleActiveButtons();
        }

        /// <summary>
        /// Orders the image elements list.
        /// </summary>
        public void OrderBy_Click()
        {
            var orderedList = new List<MySongElementUI>();

            if (OrderByDropDown.value == 0) { // by Title
                orderedList = elements.OrderBy(x => x.mySongCustomData.title).ThenBy(x => x.mySongCustomData.title).ToList();
                Debug.Log("Ordering by Title");
            }
            else if (OrderByDropDown.value == 1) { // by Artist
                orderedList = elements.OrderBy(x => x.mySongCustomData.artist).ThenBy(x => x.mySongCustomData.title).ToList();
                Debug.Log("Ordering by Artist");
            }

            // Order buttons
            for (int pos = 0; pos < orderedList.Count; pos++) {
                buttons[orderedList[pos].elementID].GetComponent<MySongElementUI>().elementPos = pos;
                buttons[orderedList[pos].elementID].transform.SetSiblingIndex(pos);
            }

            if (leftButton != null) {
                leftButton.transform.SetAsFirstSibling();
            }

            if (rightButton != null) {
                rightButton.transform.SetAsLastSibling();
            }

            if (panelGroups.gameObject.activeSelf) {
                // Reset the groups to the initial state
                for (int i = 0; i < groups.Count; i++) {
                    groups[i].transform.SetParent(parentGroupsList);
                    groups[i].transform.SetSiblingIndex(groups[i].indexNumber);
                }
            }

            // Lerp to actual selected button
            RecalculateActiveButtonsAndSize();

            selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

            if (buttons[selectedElementID].gameObject.activeSelf) {
                StopCoroutine("IE_LerpToSelectedButton");
                StartCoroutine("IE_LerpToSelectedButton");
            }
            else {
                parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
            }

            ShowVisibleActiveButtons();
        }

        /// <summary>
        /// Groups the elements and Show/Hide groups panel
        /// </summary>
        public void GroupBy_Click()
        {
            DestroyReturnButtons();

            // Reset groups positions
            for (int i = 0; i < groups.Count; i++) {
                groups[i].ResetGroup();

                groups[i].transform.SetParent(parentGroupsList);
                groups[i].transform.SetSiblingIndex(groups[i].indexNumber);
            }

            if (GroupByDropDown.value == 0) { // No Order
                Debug.Log("No grouping");

                imagesFadeCanvas.FadeIn();
                groupsFadeCanvas.FadeOut();

                for (int i = 0; i < elements.Count; i++) {
                    elements[i].GetComponent<MySongElementUI>().actualGroup = null;
                }

                selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

                actualDisplayingGroup = null;
            }
            else { // Show groups
                Debug.Log("Showing groups");

                for (int i = 0; i < elements.Count; i++) {
                    AssignToGroup(elements[i], buttons[i]);
                }

                imagesFadeCanvas.FadeOut();
                groupsFadeCanvas.FadeIn();

                parentGroupsList.anchoredPosition = new Vector2(0, parentGroupsList.anchoredPosition.y);
            }

            selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

            SearchElements_Click();

            if (buttons[selectedElementID].gameObject.activeSelf) {
                StopCoroutine("IE_LerpToSelectedButton");
                StartCoroutine("IE_LerpToSelectedButton");
            }
            else {
                parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
            }

            ShowVisibleActiveButtons();
        }

        /// <summary>
        /// Show the elements from the specified group.
        /// </summary>
        /// <param name="group"></param>
        public void ShowElementsFromGroup(SlideShowScrollViewPro_Group group)
        {
            // Reset the groups to the initial state
            for (int i = 0; i < groups.Count; i++) {
                groups[i].transform.SetParent(parentGroupsList);
                groups[i].transform.SetSiblingIndex(groups[i].indexNumber);
            }

            // If we pressed a return button, destroy the return buttons
            if (group.returnButton) {
                imagesFadeCanvas.FadeOut();
                groupsFadeCanvas.FadeIn();
                DestroyReturnButtons();
                actualDisplayingGroup = null;
                parentGroupsList.anchoredPosition = new Vector2(0, parentGroupsList.anchoredPosition.y);
                return;
            }

            actualDisplayingGroup = group;

            DestroyReturnButtons();

            leftButton = null;
            rightButton = null;

            bool atLeastOne = false;

            // Check if the elements belong to the group if not, deactivate it
            for (int i = 0; i < buttons.Count; i++) {
                if (buttons[i].GetComponent<MySongElementUI>().actualGroup == group) {
                    buttons[i].SetActive(true);
                    atLeastOne = true;
                }
                else {
                    buttons[i].SetActive(false);
                }
            }

            // Show left corner button
            // If you found at least one, the elements are displayed and groups are hidden
            if (atLeastOne) {
                imagesFadeCanvas.FadeIn();
                groupsFadeCanvas.FadeOut();

                OrderBy_Click();

                // Add groups to the corners
                // Check if the group to be selected is valid
                if ((group.indexNumber) <= 0) { // If it is less than 0 then there are no more groups left
                                                // Show return button      
                    leftButton = Instantiate(btnReturn, parentImagesList);
                    leftButton.transform.SetAsFirstSibling();

                    // Fix rotation
                    if (vertical) {
                        leftButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    else {
                        leftButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }

                }
                else { // If it is valid, move the button of the previous letter to the selected one
                       // If that group has no elements check the following
                    int actualPos = 1;
                    bool hasElements = false;
                    do {
                        if (groups[group.indexNumber - actualPos].ElementsCount <= 0) {
                            actualPos++;
                        }
                        else {
                            hasElements = true;
                        }

                        // If there is no more, exit the cycle
                        if ((group.indexNumber - actualPos) < 0) {
                            break;
                        }
                    }
                    while ((SlideShowScrollViewPro_Utilities.TryGetElement(groups.ToArray(), actualPos) != null) && !hasElements);

                    if (hasElements) {
                        leftButton = groups[group.indexNumber - actualPos].gameObject;
                        leftButton.transform.SetParent(parentImagesList);
                        leftButton.transform.SetAsFirstSibling();
                    }
                    else {
                        // Show return button      
                        leftButton = Instantiate(btnReturn, parentImagesList);
                        leftButton.transform.SetAsFirstSibling();

                        // Fix rotation
                        if (vertical) {
                            leftButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        }
                        else {
                            leftButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        }
                    }
                }

                // Show right corner button
                if ((group.indexNumber + 2) > (groups.Count - 1)) { // If it is less than 0 then there are no more groups right
                    rightButton = Instantiate(btnReturn, parentImagesList);
                    rightButton.transform.SetAsLastSibling();

                    if (vertical) {
                        rightButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                    }
                    else {
                        rightButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                }
                else {

                    int actualPos = 1;

                    bool hasElements = false;
                    do {
                        if (groups[group.indexNumber + actualPos].ElementsCount <= 0) {
                            actualPos++;
                        }
                        else {
                            hasElements = true;
                        }

                        if ((group.indexNumber + actualPos) > groups.Count - 1) {
                            break;
                        }
                    }
                    while ((SlideShowScrollViewPro_Utilities.TryGetElement(groups.ToArray(), actualPos) != null) && !hasElements);

                    if (hasElements) {
                        rightButton = groups[group.indexNumber + actualPos].gameObject;
                        rightButton.transform.SetParent(parentImagesList);
                        rightButton.transform.SetAsLastSibling();
                    }
                    else {
                        rightButton = Instantiate(btnReturn, parentImagesList);
                        rightButton.transform.SetAsLastSibling();

                        if (vertical) {
                            rightButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                        }
                        else {
                            rightButton.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        }
                    }
                }
            }
            else { // If not, the elements are hidden and the group phase is returned (this should never happen anyway)
                imagesFadeCanvas.FadeOut();
                groupsFadeCanvas.FadeIn();

                actualDisplayingGroup = null;

                for (int i = 0; i < groups.Count; i++) {
                    groups[i].transform.SetParent(parentGroupsList);
                    groups[i].transform.SetSiblingIndex(groups[i].indexNumber);
                }
            }

            SearchElements_Click();
            RecalculateActiveButtonsAndSize();

            if (buttons[selectedElementID].gameObject.activeSelf) {
                selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

                StopCoroutine("IE_LerpToSelectedButton");
                StartCoroutine("IE_LerpToSelectedButton");
            }
            else {
                selectedElementPos = -1;
                parentImagesList.anchoredPosition = new Vector2(0, parentImagesList.anchoredPosition.y);
            }

            ShowVisibleActiveButtons();
        }

        /// <summary>
        /// On selection shange, show element data.
        /// </summary>
        void OnSelectionChange()
        {
            if (previousSelectedButton != selectedElementID) {

                previous = previousSelectedButton;
                previousSelectedButton = selectedElementID;

                selectedElementPos = buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

                Debug.Log("Selection changed");

                ShowElementDataNow();
            }
        }

        /// <summary>
        /// Show element data, fade previous button and change background.
        /// </summary>
        void ShowElementDataNow()
        {

            if (!active) {
                return;
            }

            StartCoroutine(IE_FadeOutPreviousButton());

            StopCoroutine("ChangeBackgroundSprite");
            StartCoroutine("ChangeBackgroundSprite");

            // Show Element Data
            elementTitle.text = elements[selectedElementID].mySongCustomData.title;
            elementArtist.text = elements[selectedElementID].mySongCustomData.artist;

//            Debug.Log("Showing element data");

            StartCoroutine(IE_FadeIn());
        }

        /// <summary>
        /// Lerp to the selected button.
        /// </summary>
        /// <returns></returns>
        IEnumerator IE_LerpToSelectedButton()
        {
            // Disable scroll
            scrollRect.horizontal = false;

            centerButton = (int)(Math.Round(activeButtons.Count / 2f, MidpointRounding.AwayFromZero) - 1);

            int pos = centerButton - buttons[selectedElementID].GetComponent<MySongElementUI>().elementPos;

            float newPosition = pos * (elementsHorizontalSize + imagesSeparation);

            //	Debug.Log ("Selected button: " + selectedButton);
            //
            //	Debug.Log ("Center button: " + centerButton);
            //
            //	Debug.Log ("Selected button pos: " + selectedButtonPos + " - " + "Pos: " + pos);
            //
            //	Debug.Log (newPosition);

            if (activeButtons.Count % 2 == 0) {
                newPosition += (elementsHorizontalSize / 2f) + (imagesSeparation / 2f);
            }

            float actualPosition = parentImagesList.anchoredPosition.x;

            float time = 0f;

            while (time < 1) {

                // Execute the function every 10 frames to no impact the performance
                if (Time.frameCount % 10 == 0) {
                    ShowVisibleActiveButtons();
                }

                time += Time.unscaledDeltaTime / transitionTime;

                // Smooth Ease In/Out Lerp
                float t = time;
                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                float newX = (Mathf.Lerp(actualPosition, newPosition, t));
                Vector2 panelNewPosition = new Vector2(newX, parentImagesList.anchoredPosition.y);

                parentImagesList.anchoredPosition = panelNewPosition;

                yield return new WaitForEndOfFrame();
            }

            Vector2 panelNewPosition2 = new Vector2(newPosition, parentImagesList.anchoredPosition.y);

            parentImagesList.anchoredPosition = panelNewPosition2;

            // Enable scroll
            scrollRect.horizontal = true;

            ShowVisibleActiveButtons();

            yield break;
        }

        /// <summary>
        /// Fade the previous button.
        /// </summary>
        /// <returns></returns>
        IEnumerator IE_FadeOutPreviousButton()
        {
            if (previous <= -1) {
                yield break;
            }

            int button = previous;

            float time = 0;
            var actualColor = buttons[button].GetComponent<MySongElementUI>().image.color;
            var actualScale = buttons[button].GetComponent<MySongElementUI>().gameObject.GetComponent<RectTransform>().localScale;
            var actualTextColor = buttons[button].GetComponent<MySongElementUI>().title.color;

            while (time < 1) {
                time += Time.unscaledDeltaTime / buttonsFadeTime;

                buttons[button].GetComponent<MySongElementUI>().image.color = Color.Lerp(actualColor,
                  Color.gray,
                  time);


                buttons[button].GetComponent<MySongElementUI>().gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(actualScale,
                  normalScale,
                  time);


                buttons[button].GetComponent<MySongElementUI>().SetTextsColor(Color.Lerp(actualTextColor, Color.gray, time));


                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Fade in the selected button.
        /// </summary>
        /// <returns></returns>
        IEnumerator IE_FadeIn()
        {
            float time = 0;
            var actualColor = buttons[selectedElementID].GetComponent<MySongElementUI>().image.color;
            var actualScale = buttons[selectedElementID].GetComponent<MySongElementUI>().gameObject.GetComponent<RectTransform>().localScale;
            var actualTextColor = buttons[selectedElementID].GetComponent<MySongElementUI>().title.color;

            while (time < 1) {
                time += Time.unscaledDeltaTime / buttonsFadeTime;

                buttons[selectedElementID].GetComponent<MySongElementUI>().image.color = Color.Lerp(actualColor, Color.white, time);


                buttons[selectedElementID].GetComponent<MySongElementUI>().gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(actualScale,
                  selectedScale,
                  time);


                buttons[selectedElementID].GetComponent<MySongElementUI>().SetTextsColor(Color.Lerp(actualTextColor, Color.white, time));


                yield return new WaitForEndOfFrame();
            }

        }

        /// <summary>
        /// Selects Down or Left Button
        /// </summary>
        public void SelectDownOrLeftButton_Click()
        {
            if (!active || !panelImages.gameObject.activeSelf) {
                return;
            }

            // Get the actual position
            int actualPos = 0;
            for (int i = 0; i < activeButtons.Count; i++) {
                if (activeButtons[i].GetComponent<MySongElementUI>().elementPos == selectedElementPos) {
                    actualPos = i;
                    break;
                }
            }

            if (actualPos - 1 >= 0) {
                for (int i = 0; i < activeButtons.Count; i++) {
                    if (activeButtons[i].GetComponent<MySongElementUI>().elementPos == selectedElementPos - 1) {
                        actualPos = activeButtons[i].GetComponent<MySongElementUI>().elementID;
                        break;
                    }
                }

                selectedElementID = actualPos;
            }
            else {
                selectedElementID = activeButtons[activeButtons.Count - 1].GetComponent<MySongElementUI>().elementID;
            }

            StopCoroutine("IE_LerpToSelectedButton");
            StartCoroutine("IE_LerpToSelectedButton");

            OnSelectionChange();
        }

        /// <summary>
        /// Selects Up or Right Button
        /// </summary>
        public void SelectUpOrRightButton_Click()
        {
            if (!active || !panelImages.gameObject.activeSelf) {
                return;
            }

            int actualPos = 0;
            for (int i = 0; i < activeButtons.Count; i++) {
                if (activeButtons[i].GetComponent<MySongElementUI>().elementPos == selectedElementPos) {
                    actualPos = i;
                    break;
                }
            }

            if (actualPos + 1 > activeButtons.Count - 1) {
                selectedElementID = activeButtons[0].GetComponent<MySongElementUI>().elementID;
            }
            else {
                for (int i = 0; i < activeButtons.Count; i++) {
                    if (activeButtons[i].GetComponent<MySongElementUI>().elementPos == selectedElementPos + 1) {
                        actualPos = activeButtons[i].GetComponent<MySongElementUI>().elementID;
                        break;
                    }
                }

                selectedElementID = actualPos;
            }

            StopCoroutine("IE_LerpToSelectedButton");
            StartCoroutine("IE_LerpToSelectedButton");

            OnSelectionChange();
        }

        /// <summary>
        /// Calculates the active buttons.
        /// </summary>
        /// <returns></returns>
        public List<GameObject> CalculateActiveButtons()
        {

            var list = SlideShowScrollViewPro_Utilities.GetChildren(parentImagesList.gameObject, false).ToList();

            int activeObjectCount = 0;

            List<GameObject> newList = new List<GameObject>();
            for (int i = 0; i < list.Count; i++) {
                if (list[i].activeSelf) {

                    if (list[i].GetComponent<MySongElementUI>() != null) {
                        list[i].GetComponent<MySongElementUI>().elementPos = activeObjectCount++;
                        newList.Add(list[i]);
                    }

                }
            }
            return newList;
        }

        /// <summary>
        /// Lerp to the nearest button
        /// </summary>
        void LerpToNearestButton()
        {
            minDistance = int.MaxValue;

            for (int i = 0; i < activeButtons.Count; i++) {

                float actualDistance = Vector2.Distance(activeButtons[i].GetComponent<RectTransform>().position, center.position);

                if (actualDistance <= minDistance) {
                    minDistance = actualDistance;
                    selectedElementID = activeButtons[i].GetComponent<MySongElementUI>().elementID;
                    selectedElementPos = activeButtons[i].GetComponent<MySongElementUI>().elementPos;
                }
            }

            StopCoroutine("IE_LerpToSelectedButton");
            StartCoroutine("IE_LerpToSelectedButton");

            OnSelectionChange();
        }

        void Update()
        {
            // Set images scroll limits
            if (panelImages.gameObject.activeSelf) {
                var value = (activeButtons.Count - 1 - centerButton) * (elementsHorizontalSize + imagesSeparation);

                if (parentImagesList.anchoredPosition.x > value) {
                    parentImagesList.anchoredPosition = new Vector2(value, parentImagesList.anchoredPosition.y);
                }
                else if (parentImagesList.anchoredPosition.x < -value) {
                    parentImagesList.anchoredPosition = new Vector2(-value, parentImagesList.anchoredPosition.y);
                }
            }

            // Set group scroll limits
            if (panelGroups.gameObject.activeSelf) {
                var value = (SlideShowScrollViewPro_Utilities.GetActiveChildCount(parentGroupsList) - 1 - centerGroupsButton) * (elementsHorizontalSize + imagesSeparation);

                if (parentGroupsList.anchoredPosition.x > value) {
                    parentGroupsList.anchoredPosition = new Vector2(value, parentGroupsList.anchoredPosition.y);
                }
                else if (parentGroupsList.anchoredPosition.x < -value) {
                    parentGroupsList.anchoredPosition = new Vector2(-value, parentGroupsList.anchoredPosition.y);
                }
            }

            // Execute the function every 10 frames to no impact the performance
            if (Time.frameCount % 10 == 0) {
                if (updateVisibleObjects) {
                    ShowVisibleActiveButtons();
                }
            }
        }

        /// <summary>
        /// Destroy return buttons
        /// </summary>
        void DestroyReturnButtons()
        {

            if (leftButton != null) {
                if (leftButton.GetComponent<SlideShowScrollViewPro_Group>().returnButton) {
                    Destroy(leftButton);
                }
            }

            if (rightButton != null) {
                if (rightButton.GetComponent<SlideShowScrollViewPro_Group>().returnButton) {
                    Destroy(rightButton);
                }
            }
        }

        /// <summary>
        /// Shows the visible by camera active buttons and deactivate not visible
        /// </summary>
        public void ShowVisibleActiveButtons()
        {
            for (int i = 0; i < activeButtons.Count; i++) {
                activeButtons[i].GetComponent<ShowUIWhenIsVisible>().UpdateNow();
            }
        }

        bool updateVisibleObjects;

        /// <summary>
        /// On Start Drag
        /// </summary>
        public void StartDrag()
        {
            center.GetComponent<Image>().enabled = true;

            StopCoroutine("IE_LerpToSelectedButton");
            scrollRect.horizontal = true;

            updateVisibleObjects = true;
        }


        /// <summary>
        /// On End Drag
        /// </summary>
        public void EndDrag()
        {
            center.GetComponent<Image>().enabled = false;

            active = true;
            LerpToNearestButton();

            updateVisibleObjects = false;
        }

        /// <summary>
        /// Update visible gameobjects on scroll
        /// </summary>
        public void OnScroll()
        {
            ShowVisibleActiveButtons();
        }

        /// <summary>
        /// Opens the scroll list
        /// </summary>
        public void OpenScrollList()
        {
            rhythmVisualizatorCamera.SetActive(false);
            musicPlayerCanvas.SetActive(false);

            scrollListCamera.SetActive(true);

            scrollListDownsideCanvas.gameObject.SetActive(true);
            scrollListCanvas.gameObject.SetActive(true);

            scrollListDownsideCanvas.GetComponent<FadeCanvas>().FadeIn();
            scrollListCanvas.GetComponent<FadeCanvas>().FadeIn();

            if (selectedElementPos >= 0) {
                SelectButtonByID_Click(activeButtons[selectedElementPos].GetComponent<MySongElementUI>().elementID);
            }
        }

        void ChangeCameraPosition()
        {
            rhythmVisualizatorCamera.SetActive(true);
            scrollListCamera.SetActive(false);

        }

        /// <summary>
        /// Closes the scroll list
        /// </summary>
        public void CloseScrollList()
        {
            Invoke("ChangeCameraPosition", scrollListCanvas.GetComponent<FadeCanvas>().FadeTime);
            musicPlayerCanvas.SetActive(true);

            scrollListDownsideCanvas.GetComponent<FadeCanvas>().FadeOut();
            scrollListCanvas.GetComponent<FadeCanvas>().FadeOut();
        }
    }
}












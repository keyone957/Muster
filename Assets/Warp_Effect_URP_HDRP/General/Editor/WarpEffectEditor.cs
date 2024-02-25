using UnityEngine;
using UnityEditor;

/// <summary>
/// This is the editor for the Warp Effect.
/// (c) 2021 Dirk Jacobasch
/// dirk.jacobasch@outlook.com
/// </summary>
namespace com.ggames4u.warp_effect_urp_hdrp {

    [HelpURL("https://www.ggames4u.com/wp-content/uploads/2021/08/WarpEffect_URP_HDRP_Manual.pdf")]
    [CustomEditor(typeof(WarpEffect))]
    public class WarpEffectEditor : Editor {
		private SerializedObject serObj;

        public string[] EmissionMapModes = new string[] { "Add", "Multiply" };

        private Renderer warpRenderer;
        private Material[] warpMaterials;
        private WarpEffect warpSphereScript;
        private Material sharedWarpMaterial;

        private Vector2 scrollPos;
        private GameObject gameObject;

        private GUIStyle defaultStyle;
        private GUIStyle titleStyle;

        // MATERIAL PROPERTIES

        [SerializeField] private float warpSpeed;

        [ColorUsage(true, true)]
        [SerializeField] private Color mainColor;

        [ColorUsage(true, true)]
        [SerializeField] private Color mixColor;
		
        // Texture 1
		[SerializeField] private Texture2D warpTextures_01;
        [SerializeField] private Vector2 warpTextures_01_Tiling;
        [SerializeField] private float rotationSpeed_01;

        // Texture 2
        [SerializeField] private Texture2D warpTextures_02;
        [SerializeField] private Vector2 warpTextures_02_Tiling;
        [SerializeField] private float rotationSpeed_02;

        // Emission maps
        [ColorUsage(true, true)]
        [SerializeField] private Color emissionColor;

        [SerializeField] private Texture2D emissionMap_01;
        [SerializeField] private Vector2 emissionMap_01_Tiling;
        [SerializeField] private float emissionMap_Rotation_Speed_01;
        [SerializeField] private float emissionMapSpeed_01;

        [SerializeField] private Texture2D emissionMap_02;
        [SerializeField] private Vector2 emissionMap_02_Tiling;
        [SerializeField] private float emissionMap_Rotation_Speed_02;
        [SerializeField] private float emissionMapSpeed_02;

        [SerializeField] private int multiplyEmissionMaps;

        /// <summary>
        /// Setup all properties. This method reads the values from
        /// the warp material shader.
        /// </summary>
        private void OnEnable() {
            // Get target material
            warpSphereScript = target as WarpEffect;
            serObj = new SerializedObject(target);

            gameObject = warpSphereScript.gameObject.transform.Find("WarpSphereContainer").Find("WarpSphere").gameObject;
            
            warpRenderer = gameObject.GetComponent<Renderer>();
            
            if (warpRenderer) {
                warpMaterials = warpRenderer.sharedMaterials;

                if (warpMaterials[2].shader.name.Contains("Warp_Shader_Graph")) {
                    sharedWarpMaterial = warpRenderer.sharedMaterials[2];

                } else {
                    Debug.LogWarning("Could not find the correct shader. Please check the material order at your WarpSphere game object.");
                }

            } else {
                Debug.LogWarning("Warp renderer not found!");
            }

            // Get values from material
            if (warpRenderer && sharedWarpMaterial) {
                // Get warp speed
                warpSpeed = sharedWarpMaterial.GetFloat("Warp_Speed");

                // Get colors
                mainColor = sharedWarpMaterial.GetColor("Main_Color");

                // For HDR intensity: 2^Intensity(color * Mathf.Pow(2, intensity) 
                mixColor = sharedWarpMaterial.GetColor("Mix_Color");

                // Get warp textures
                warpTextures_01 = sharedWarpMaterial.GetTexture("Warp_Texture_01") as Texture2D;
                warpTextures_01_Tiling = sharedWarpMaterial.GetVector("Warp_Texture_Tiling_01");
                rotationSpeed_01 = sharedWarpMaterial.GetFloat("Rotation_Speed_01");

                warpTextures_02 = sharedWarpMaterial.GetTexture("Warp_Texture_02") as Texture2D;
                warpTextures_02_Tiling = sharedWarpMaterial.GetVector("Warp_Texture_Tiling_02");
                rotationSpeed_02 = sharedWarpMaterial.GetFloat("Rotation_Speed_02");

                // EMISSION MAPS

                // Emission map 1
                emissionMap_01 = sharedWarpMaterial.GetTexture("Emission_Map_01") as Texture2D;
                emissionMap_01_Tiling = sharedWarpMaterial.GetVector("Emission_Map_Tiling_01");
                emissionMap_Rotation_Speed_01 = sharedWarpMaterial.GetFloat("Emission_Map_Rotation_Speed_01");
                emissionMapSpeed_01 = sharedWarpMaterial.GetFloat("Emission_Map_Speed_01");

                // Emission map 2
                emissionMap_02 = sharedWarpMaterial.GetTexture("Emission_Map_02") as Texture2D;
                emissionMap_02_Tiling = sharedWarpMaterial.GetVector("Emission_Map_Tiling_02");
                emissionMap_Rotation_Speed_02 = sharedWarpMaterial.GetFloat("Emission_Map_Rotation_Speed_02");
                emissionMapSpeed_02 = sharedWarpMaterial.GetFloat("Emission_Map_Speed_02");

                // Multiply emission
                multiplyEmissionMaps = (sharedWarpMaterial.GetFloat("Multiply_Emission") > 0) ? 1 : 0;

                // Emission color
                emissionColor = sharedWarpMaterial.GetColor("Emission_Color");
            }
        }

        /// <summary>
        /// Create editor UI.
        /// The editor is separeated into different blocks.
        /// </summary>
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

			serObj.Update();

            // Styles
            defaultStyle = new GUIStyle(EditorStyles.textField);
            defaultStyle.normal.textColor = new Color(0.25f, 0.25f, 0.25f);
            defaultStyle.normal.background = null;
            defaultStyle.fontSize = 11;
            defaultStyle.fontStyle = FontStyle.Bold;
            defaultStyle.stretchHeight = false;
            defaultStyle.clipping = TextClipping.Clip;

            titleStyle = new GUIStyle(EditorStyles.textField);
            titleStyle.normal.textColor = new Color(0.85f, 0.15f, 0.1f);
            titleStyle.normal.background = null;
            titleStyle.fontSize = 12;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.stretchHeight = true;
            titleStyle.clipping = TextClipping.Overflow;

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginVertical("box");

            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Speed", titleStyle);
            GUILayout.Space(5f);

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(this, "Undo Warp Settings");

            EditorGUI.indentLevel += 1;
            warpSpeed = EditorGUILayout.Slider(
                new GUIContent("Warp Speed", "The overall speed of the Warp Effect."),
                warpSpeed,
                0.1f, 20f);
            EditorGUI.indentLevel -= 1;

            GUILayout.Space(5f);
            EditorGUILayout.LabelField("Warp Colors", titleStyle);
            GUILayout.Space(5f);

			EditorGUI.indentLevel += 1;
            
            // Warp colors
            GUILayoutOption[] options = new GUILayoutOption[] { };
            mainColor = EditorGUILayout.ColorField(new GUIContent("Main Color"), mainColor, true, true, true, options);

            mixColor = EditorGUILayout.ColorField(new GUIContent("Mix Color"), mixColor, true, true, true, options);
            EditorGUI.indentLevel -= 1;
            GUILayout.Space(5f);

            EditorGUILayout.EndVertical();
            
            GUILayout.Space(10f);

            EditorGUILayout.BeginVertical("box");

            // Main textures
            EditorGUILayout.LabelField("Warp Textures", titleStyle);
            GUILayout.Space(10f);

            EditorGUI.indentLevel += 1;

            // Main texture 1
            warpTextures_01 = (Texture2D)EditorGUILayout.ObjectField("Warp Texture 1", warpTextures_01, typeof(Texture2D), true);
            warpTextures_01_Tiling = EditorGUILayout.Vector2Field("Tiling", warpTextures_01_Tiling);
			GUILayout.Space(5f);

			rotationSpeed_01 = EditorGUILayout.Slider(
				new GUIContent("Rotation Speed 1", ""),
				rotationSpeed_01,
				-1.0f, 1.0f);
			GUILayout.Space(10f);

            // Main texture 2
            warpTextures_02 = (Texture2D)EditorGUILayout.ObjectField("Warp Texture 2", warpTextures_02, typeof(Texture2D), true);
            warpTextures_02_Tiling = EditorGUILayout.Vector2Field("Tiling", warpTextures_02_Tiling);
			GUILayout.Space(5f);

            rotationSpeed_02 = EditorGUILayout.Slider(
                new GUIContent("Rotation Speed 2", ""),
                rotationSpeed_02,
                -1.0f, 1.0f);
            GUILayout.Space(10f);

            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();

			GUILayout.Space(10f);

            
			// Emission maps
			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Emission Maps", titleStyle);
			GUILayout.Space(10f);
			EditorGUI.indentLevel += 1;

			// Emission map 1
			emissionMap_01 = (Texture2D)EditorGUILayout.ObjectField("Emission Map 1", emissionMap_01, typeof(Texture2D), true);
			emissionMap_01_Tiling = EditorGUILayout.Vector2Field("Tiling", emissionMap_01_Tiling);
			GUILayout.Space(5f);

			emissionMap_Rotation_Speed_01 = EditorGUILayout.Slider(
				new GUIContent("Emission Map 1 Rotation Speed", ""),
				emissionMap_Rotation_Speed_01,
				-1.0f, 1.0f);

            emissionMapSpeed_01 = EditorGUILayout.Slider(
                new GUIContent("Emission Map 1 Speed", ""),
                emissionMapSpeed_01,
                -1.0f, 1.0f);

            GUILayout.Space(10f);

			// Emission map 2
			emissionMap_02 = (Texture2D)EditorGUILayout.ObjectField("Emission Map 2", emissionMap_02, typeof(Texture2D), true);
			emissionMap_02_Tiling = EditorGUILayout.Vector2Field("Tiling", emissionMap_02_Tiling);

            GUILayout.Space(5f);

			emissionMap_Rotation_Speed_02 = EditorGUILayout.Slider(
				new GUIContent("Emission Map 2 Rotation Speed", ""),
				this.emissionMap_Rotation_Speed_02,
				-1.0f, 1.0f);

            emissionMapSpeed_02 = EditorGUILayout.Slider(
                new GUIContent("Emission Map 2 Speed", ""),
                emissionMapSpeed_02,
                -1.0f, 1.0f);

            GUILayout.Space(5f);

            // Emission color
			emissionColor = EditorGUILayout.ColorField(new GUIContent("Emission Color"), emissionColor, true, true,  true, options);

            GUILayout.Space(5f);

            // Emission map mode. Add or multiply
            multiplyEmissionMaps = EditorGUILayout.Popup(new GUIContent("Emission Map Mode", "Add or multiply emission maps togesther."), multiplyEmissionMaps, EmissionMapModes);

            EditorGUI.indentLevel -= 1;
            EditorGUILayout.EndVertical();

            GUILayout.Space(15f);

            // Emission distortion texture values
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField("Emission Distatortion.", titleStyle);
            GUILayout.Space(10f);

			EditorGUILayout.EndVertical();

			GUILayout.Space(10f);

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10f);

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(sharedWarpMaterial, "Undo Warp Settings");

				OnApply();
            }
        }
		
        /// <summary>
        /// Apply values to the material if something was changed in the editor.
        /// </summary>
        private void OnApply() {
            if (warpRenderer != null) {
                //Debug.Log("Apply new values");

                // Warp speed
                sharedWarpMaterial.SetFloat("Warp_Speed", warpSpeed);

                // Colors
                sharedWarpMaterial.SetColor("Main_Color", mainColor);
                sharedWarpMaterial.SetColor("Mix_Color", mixColor);

                // Warp texture 1
                if (warpTextures_01) {
                    sharedWarpMaterial.SetTexture("Warp_Texture_01", warpTextures_01);
                }

                sharedWarpMaterial.SetVector("Warp_Texture_Tiling_01", warpTextures_01_Tiling);
                sharedWarpMaterial.SetFloat("Rotation_Speed_01", rotationSpeed_01);

                // Warp texture 2
                sharedWarpMaterial.SetTexture("Warp_Texture_02", warpTextures_02);
                sharedWarpMaterial.SetVector("Warp_Texture_Tiling_02", warpTextures_02_Tiling);
                sharedWarpMaterial.SetFloat("Rotation_Speed_02", rotationSpeed_02);

                // Emission map 01
                sharedWarpMaterial.SetTexture("Emission_Map_01", emissionMap_01);
                sharedWarpMaterial.SetVector("Emission_Map_Tiling_01", emissionMap_01_Tiling);
                sharedWarpMaterial.SetFloat("Emission_Map_Speed_01", emissionMapSpeed_01);
                sharedWarpMaterial.SetFloat("Emission_Map_Rotation_Speed_01", emissionMap_Rotation_Speed_01);

                // Emission map 02
                sharedWarpMaterial.SetTexture("Emission_Map_02", emissionMap_02);
                sharedWarpMaterial.SetVector("Emission_Map_Tiling_02", emissionMap_02_Tiling);
                sharedWarpMaterial.SetFloat("Emission_Map_Speed_02", emissionMapSpeed_02);
                sharedWarpMaterial.SetFloat("Emission_Map_Rotation_Speed_02", emissionMap_Rotation_Speed_02);

                // Emission color
                sharedWarpMaterial.SetColor("Emission_Color", emissionColor);

                // Multiply emission maps
                sharedWarpMaterial.SetFloat("Multiply_Emission", multiplyEmissionMaps);

                EditorUtility.SetDirty(sharedWarpMaterial);
			}
        }
    }
}

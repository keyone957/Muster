using UnityEngine;
using UnityEditor;

namespace com.ggames4u.warp_effect_urp_hdrp {

    [HelpURL("https://www.ggames4u.com/wp-content/uploads/2021/08/WarpEffect_URP_HDRP_Manual.pdf")]
    [CustomEditor(typeof(StarEffect))]
    public class StarEffectEditor : Editor {
        private SerializedObject serObj;

        private Renderer warpRenderer;
        private Material[] warpMaterials;
        private StarEffect backgroundDistortionScript;
        private Material sharedWarpMaterial;

        private Vector2 scrollPos;
        private GameObject gameObject;

        private GUIStyle defaultStyle;
        private GUIStyle titleStyle;

        public string[] yesNoOptions = new string[] { "No", "Yes" };

        // MATERIAL PROPERTIES

        [SerializeField] private float warpSpeed;

        // Star Effect Layer 1

        [SerializeField] private int renderStarLayer_01;

        [ColorUsage(true, true)]
        [SerializeField] private Color starColor_01;
        [SerializeField] private Texture2D starTextures_01;
        [SerializeField] private Vector2 starTexturesTiling_01;
        [SerializeField] private float starSpeed_01;
        [SerializeField] private float starRotationSpeed_01;

        // Star Effect Layer 2

        [SerializeField] private int renderStarLayer_02;

        [ColorUsage(true, true)]
        [SerializeField] private Color starColor_02;
        [SerializeField] private Texture2D starTextures_02;
        [SerializeField] private Vector2 starTexturesTiling_02;
        [SerializeField] private float starSpeed_02;
        [SerializeField] private float starRotationSpeed_02;

        [SerializeField] private float starAlpha;

        [SerializeField] private float Star_Texture_Start_Position_01;
        [SerializeField] private float Star_Texture_Start_Position_02;

        private void OnEnable() {
            // Get target material
            backgroundDistortionScript = target as StarEffect;
            serObj = new SerializedObject(target);

            gameObject = backgroundDistortionScript.gameObject.transform.Find("WarpSphereContainer").Find("WarpSphere").gameObject;

            warpRenderer = gameObject.GetComponent<Renderer>();

            if (warpRenderer) {
                warpMaterials = warpRenderer.sharedMaterials;

                if (warpMaterials[1].shader.name.Contains("Star_Shader_Graph")) {
                    sharedWarpMaterial = warpRenderer.sharedMaterials[1];

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

                // Star Effect Layer 1

                renderStarLayer_01 = (sharedWarpMaterial.GetInt("Render_Star_Layer_01") > 0.0f) ? 1 : 0;

                starColor_01 = sharedWarpMaterial.GetColor("Star_Color_01");

                starTextures_01 = sharedWarpMaterial.GetTexture("Star_Texture_01") as Texture2D;
                starTexturesTiling_01 = sharedWarpMaterial.GetVector("Star_Texture_Tiling_01");

                starSpeed_01 = sharedWarpMaterial.GetFloat("Star_Speed_01");
                starRotationSpeed_01 = sharedWarpMaterial.GetFloat("Star_Rotation_Speed_01");

                // Star Effect Layer 1

                renderStarLayer_02 = (sharedWarpMaterial.GetInt("Render_Star_Layer_02") > 0.0f) ? 1 : 0;

                starColor_02 = sharedWarpMaterial.GetColor("Star_Color_02");

                starTextures_02 = sharedWarpMaterial.GetTexture("Star_Texture_02") as Texture2D;
                starTexturesTiling_02 = sharedWarpMaterial.GetVector("Star_Texture_Tiling_02");

                starSpeed_02 = sharedWarpMaterial.GetFloat("Star_Speed_02");
                starRotationSpeed_02 = sharedWarpMaterial.GetFloat("Star_Rotation_Speed_02");

                starAlpha = sharedWarpMaterial.GetFloat("Star_Alpha");
            }
        }

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
            EditorGUILayout.LabelField("Star Effect Settings", titleStyle);
            GUILayout.Space(5f);

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(this, "Undo Background Star Settings");

            EditorGUI.indentLevel += 1;

            warpSpeed = EditorGUILayout.Slider(
                new GUIContent("Warp Speed", "The overall speed of the Warp Effect. This value should be set from a script with the warp speed of the warp material. You can find an example in the WarpOnOff script."),
                warpSpeed,
                0.1f, 20f);
            GUILayout.Space(5f);

            // Star Effect Layer 1

            renderStarLayer_01 = EditorGUILayout.Popup(new GUIContent("Render Star Layer 1", "Switch star layer 1 on or off."), renderStarLayer_01, yesNoOptions);

            GUILayoutOption[] options = new GUILayoutOption[] { };
            starColor_01 = EditorGUILayout.ColorField(new GUIContent("Star Color Layer 1"), starColor_01, true, true, true, options);

            starTextures_01 = EditorGUILayout.ObjectField("Star Texture 1", starTextures_01, typeof(Texture2D), true) as Texture2D;
            GUILayout.Space(5f);

            starTexturesTiling_01 = EditorGUILayout.Vector2Field("Tiling", starTexturesTiling_01);
            GUILayout.Space(5f);

            starSpeed_01 = EditorGUILayout.Slider(
                new GUIContent("Star Speed Layer 1", "The speed of star layer 1."),
                starSpeed_01,
                0.1f, 20f);
            GUILayout.Space(5f);

            starRotationSpeed_01 = EditorGUILayout.Slider(
                new GUIContent("Star Rotation Speed Layer 1", "The rotation speed of star layer 1."),
                starRotationSpeed_01,
                -20.0f, 20f);
            GUILayout.Space(5f);


            // Star Effect Layer 2

            renderStarLayer_02 = EditorGUILayout.Popup(new GUIContent("Render Star Layer 2", "Switch star layer 2 on or off."), renderStarLayer_02, yesNoOptions);

            starColor_02 = EditorGUILayout.ColorField(new GUIContent("Star Color Layer 2"), starColor_02, true, true, true, options);

            starTextures_02 = EditorGUILayout.ObjectField("Star Texture 2", starTextures_02, typeof(Texture2D), true) as Texture2D;
            GUILayout.Space(5f);

            starTexturesTiling_02 = EditorGUILayout.Vector2Field("Tiling", starTexturesTiling_02);
            GUILayout.Space(5f);

            starSpeed_02 = EditorGUILayout.Slider(
                new GUIContent("Star Speed Layer 2", "The speed of star layer 2."),
                starSpeed_02,
                0.1f, 20f);
            GUILayout.Space(5f);

            starRotationSpeed_02 = EditorGUILayout.Slider(
                new GUIContent("Star Rotation Speed Layer 2", "The rotation speed of star layer 2."),
                starRotationSpeed_02,
                -20.0f, 20f);
            GUILayout.Space(5f);

            starAlpha = EditorGUILayout.Slider(
                new GUIContent("Star Alpha", "The transparency of both star layers."),
                starAlpha,
                0.0f, 1f);
            GUILayout.Space(5f);

            EditorGUI.indentLevel -= 1;

            EditorGUILayout.EndVertical();

            GUILayout.Space(10f);

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10f);

            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(sharedWarpMaterial, "Undo Background Distortion Settings");

                OnApply();
            }
        }

        private void OnApply() {
            if (warpRenderer != null) {
                Debug.Log("Apply new values");

                // Warp speed
                sharedWarpMaterial.SetFloat("Warp_Speed", warpSpeed);

                // Star Effect Layer 1

                sharedWarpMaterial.SetInt("Render_Star_Layer_01", renderStarLayer_01);

                sharedWarpMaterial.SetColor("Star_Color_01", starColor_01);

                sharedWarpMaterial.SetTexture("Star_Texture_01", starTextures_01);
                sharedWarpMaterial.SetVector("Star_Texture_Tiling_01", starTexturesTiling_01);
                

                sharedWarpMaterial.SetFloat("Star_Speed_01", starSpeed_01);
                sharedWarpMaterial.SetFloat("Star_Rotation_Speed_01", starRotationSpeed_01);

                // Star Effect Layer 2

                sharedWarpMaterial.SetInt("Render_Star_Layer_02", renderStarLayer_02);

                sharedWarpMaterial.SetColor("Star_Color_02", starColor_02);

                sharedWarpMaterial.SetTexture("Star_Texture_02", starTextures_02);
                sharedWarpMaterial.SetVector("Star_Texture_Tiling_02", starTexturesTiling_02);
                

                sharedWarpMaterial.SetFloat("Star_Speed_02", starSpeed_02);
                sharedWarpMaterial.SetFloat("Star_Rotation_Speed_02", starRotationSpeed_02);

                sharedWarpMaterial.SetFloat("Star_Alpha", starAlpha);

                EditorUtility.SetDirty(sharedWarpMaterial);
            }
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace com.ggames4u.warp_effect_urp_hdrp {

    [HelpURL("https://www.ggames4u.com/wp-content/uploads/2021/08/WarpEffect_URP_HDRP_Manual.pdf")]
    [CustomEditor(typeof(BackgroundDistortion))]
    public class BackgroundDistortionEditor : Editor {
        private SerializedObject serObj;

        private Renderer warpRenderer;
        private Material[] warpMaterials;
        private BackgroundDistortion backgroundDistortionScript;
        private Material sharedWarpMaterial;

        private Vector2 scrollPos;
        private GameObject gameObject;

        private GUIStyle defaultStyle;
        private GUIStyle titleStyle;

        // MATERIAL PROPERTIES

        [SerializeField] private float speed;
        [SerializeField] private float distortionStrength;
        [SerializeField] private Texture2D distortionTexture_1;
        [SerializeField] private Vector2 distortionTextureTiling_1;
        [SerializeField] private Texture2D distortionTexture_2;
        [SerializeField] private Vector2 distortionTextureTiling_2;

        private void OnEnable() {
            // Get target material
            backgroundDistortionScript = target as BackgroundDistortion;
            serObj = new SerializedObject(target);

            gameObject = backgroundDistortionScript.gameObject.transform.Find("WarpSphereContainer").Find("WarpSphere").gameObject;

            warpRenderer = gameObject.GetComponent<Renderer>();

            if (warpRenderer) {
                warpMaterials = warpRenderer.sharedMaterials;

                if (warpMaterials[0].shader.name.Contains("Background_Distortion_Shader_Graph")) {
                    sharedWarpMaterial = warpRenderer.sharedMaterials[0];

                } else {
                    Debug.LogWarning("Could not find the correct shader. Please check the material order at your WarpSphere game object.");
                }

            } else {
                Debug.LogWarning("Warp renderer not found!");
            }

            // Get values from material
            if (warpRenderer && sharedWarpMaterial) {
                // Get warp speed
                speed = sharedWarpMaterial.GetFloat("Speed");
                distortionStrength = sharedWarpMaterial.GetFloat("Distortion_Strength");

                // Textures
                distortionTexture_1 = sharedWarpMaterial.GetTexture("Distortion_Texture_1") as Texture2D;
                distortionTextureTiling_1 = sharedWarpMaterial.GetVector("Distortion_Texture_Tiling_1");

                distortionTexture_2 = sharedWarpMaterial.GetTexture("Distortion_Texture_2") as Texture2D;
                distortionTextureTiling_2 = sharedWarpMaterial.GetVector("Distortion_Texture_Tiling_2");
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
            EditorGUILayout.LabelField("Background Distortion Settings", titleStyle);
            GUILayout.Space(5f);

            EditorGUI.BeginChangeCheck();
            Undo.RecordObject(this, "Undo Background Distortion Settings");

            EditorGUI.indentLevel += 1;
            EditorGUILayout.LabelField("General Settings", titleStyle);
            GUILayout.Space(10f);

            speed = EditorGUILayout.Slider(
                new GUIContent("Speed", "The overall speed of the distortion effect."),
                speed,
                0.0f, 2f);

            GUILayout.Space(5f);

            // Distortion strength
            distortionStrength = EditorGUILayout.Slider(
                new GUIContent("Distortion Strength", ""),
                distortionStrength,
                0.005f, 1.0f);

            GUILayout.Space(5f);

            // Distortion texture 1
            EditorGUILayout.LabelField("Distortion Textures 1", titleStyle);
            GUILayout.Space(10f);

            distortionTexture_1 = EditorGUILayout.ObjectField("Distortion Texture 1", distortionTexture_1, typeof(Texture2D), true) as Texture2D;
            distortionTextureTiling_1 = EditorGUILayout.Vector2Field("Tiling", distortionTextureTiling_1);
            GUILayout.Space(5f);
            
            // Distortion texture 2
            EditorGUILayout.LabelField("Distortion Textures 2", titleStyle);
            GUILayout.Space(10f);

            distortionTexture_2 = EditorGUILayout.ObjectField("Distortion Texture 1", distortionTexture_2, typeof(Texture2D), true) as Texture2D;
            distortionTextureTiling_2 = EditorGUILayout.Vector2Field("Tiling", distortionTextureTiling_2);
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
                //Debug.Log("Apply new values");

                sharedWarpMaterial.SetFloat("Speed", speed);
                sharedWarpMaterial.SetFloat("Distortion_Strength", distortionStrength);
                sharedWarpMaterial.SetTexture("Distortion_Texture_1", distortionTexture_1);
                sharedWarpMaterial.SetVector("Distortion_Texture_Tiling_1", distortionTextureTiling_1);
                sharedWarpMaterial.SetTexture("Distortion_Texture_2", distortionTexture_2);
                sharedWarpMaterial.SetVector("Distortion_Texture_Tiling_2", distortionTextureTiling_2);

                EditorUtility.SetDirty(sharedWarpMaterial);
            }
        }
    }
}
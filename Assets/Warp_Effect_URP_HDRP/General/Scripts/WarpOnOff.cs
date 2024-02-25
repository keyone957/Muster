using UnityEngine;
using System.Collections;

namespace com.ggames4u.warp_effect_urp_hdrp {
    /// <summary>
    /// Leave On Off script.
    /// If you have any questions feel free to cantact me at dirk.jacobasch@outlook.com
    /// </summary>
    [HelpURL("https://www.ggames4u.com/wp-content/uploads/2021/08/WarpEffect_URP_HDRP_Manual.pdf")]
    public class WarpOnOff : MonoBehaviour {
        #region Variables
        private Material[] warpMaterials;
        private Material backgroundDistortionMaterial;
        private Material starMaterial;
        private Material warpMaterial;

        [Tooltip("The container from the prefab which contains the warp sphere and the particle effects")]
        [SerializeField] private GameObject warpSphereContainer;

        [SerializeField] private GameObject warpSphere;

        [Tooltip("The camera inside the warp effect. The camera can be rotated by the left/right arrows")]
        [SerializeField] private Camera warpCamera;

        [SerializeField] private AudioClip warpLoopAudio;
        private AudioSource warpLoopAudiosSource;

        [SerializeField] private AudioClip leaveWarpAudio;
        private AudioSource leaveWarpAudiosSource;

        //[Tooltip("Drag the camera with the - Leave Warp Effect Script - inside here.")]
        //private LeaveWarpEffect leaveWarpEffectScript;

        [Tooltip("Delay to start render star layer 1. Works only with the WarpPrefabShaderStars.")]
        [Range(0, 20)]
        [SerializeField] private float starLayerDelay01 = 1.5f;

        [Tooltip("Delay to start render star layer 2. Works only with the WarpPrefabShaderStars.")]
        [Range(0, 20)]
        [SerializeField] private float starLayerDelay02 = 2.5f;

        private float cameraRotationSpeed = 40f;
        #endregion

        #region Properties
        public bool WarpIsEnabled { get; private set; }
        #endregion

        #region Builtin Methods
        // Get warp material
        private void Awake() {
            if (warpSphere != null) {
                warpMaterials = warpSphere.GetComponent<Renderer>().sharedMaterials;
                if (warpMaterials[0]) {
                    backgroundDistortionMaterial = warpMaterials[0];
                }

                if (warpMaterials[1]) {
                    starMaterial = warpMaterials[1];
                }

                if (warpMaterials[2]) {
                    warpMaterial = warpMaterials[2];
                }
            }
        }

        // Use this for initialization
        void Start() {
            // Hide warp effect
            warpSphereContainer.SetActive(false);

            // Setup audio sources
            warpLoopAudiosSource = gameObject.AddComponent<AudioSource>();
            warpLoopAudiosSource.clip = warpLoopAudio;
            warpLoopAudiosSource.playOnAwake = false;
            warpLoopAudiosSource.loop = true;
            warpLoopAudiosSource.volume = 0.5f;

            leaveWarpAudiosSource = gameObject.AddComponent<AudioSource>();
            leaveWarpAudiosSource.clip = leaveWarpAudio;
            leaveWarpAudiosSource.playOnAwake = false;
            leaveWarpAudiosSource.loop = false;
            leaveWarpAudiosSource.volume = 0.5f;

            // Deactivate star layer rendering
            DisableStarLayers();
        }

        /// <summary>
        /// Disable star layer rendering.
        /// </summary>
        private void DisableStarLayers() {
            if (starMaterial) {
                starMaterial.SetInt("Render_Star_Layer_01", 0);
                starMaterial.SetInt("Render_Star_Layer_02", 0);
            }
        }

        /// <summary>
        /// Enable star layer rendering.
        /// </summary>
        private void EnableStarLayers() {
            if (starMaterial) {
                starMaterial.SetInt("Render_Star_Layer_01", 1);
                starMaterial.SetInt("Render_Star_Layer_02", 1);
            }
        }

        /// <summary>
        /// Move camera.
        /// </summary>
        void Update() {
            // Camera rotation
            if (Input.GetKey(KeyCode.LeftArrow)) {
                warpCamera.transform.Rotate(Vector3.down * Time.deltaTime * cameraRotationSpeed);

            } else if (Input.GetKey(KeyCode.RightArrow)) {
                warpCamera.transform.Rotate(Vector3.up * Time.deltaTime * cameraRotationSpeed);
            }

            if (Input.GetKey(KeyCode.UpArrow)) {
                StartWarp();

            } else if (Input.GetKey(KeyCode.DownArrow)) {
                StopWarp();
            }
        }

        private void LateUpdate() {
            if (warpMaterial && starMaterial) {
                starMaterial.SetFloat("Warp_Speed", warpMaterial.GetFloat("Warp_Speed"));
            }
        }

        /// <summary>
        /// Reset to default.
        /// </summary>
        private void OnDestroy() {
            EnableStarLayers();
        }
        #endregion

        #region Custom Methods
        public void StartWarp() {
            if (warpSphereContainer.activeSelf == false) {
                warpSphereContainer.SetActive(true);
                warpLoopAudiosSource.Play();

                //if (leaveWarpEffectScript != null) {
                //    leaveWarpEffectScript.StopEffect();
                //}

                if (starMaterial) {
                    StartCoroutine(StartRenderStarLayerWithDelay(starLayerDelay01, "Render_Star_Layer_01"));
                    StartCoroutine(StartRenderStarLayerWithDelay(starLayerDelay02, "Render_Star_Layer_02"));
                }

                WarpIsEnabled = true;
            }
        }

        public void StopWarp() {
            if (warpSphereContainer.activeSelf == true) {
                warpSphereContainer.SetActive(false);
                warpLoopAudiosSource.Stop();
                leaveWarpAudiosSource.Play();

                // Start distortion image effect
                //if (leaveWarpEffectScript != null) {
                //    leaveWarpEffectScript.StartEffect();
                //}

                // Stop star layer rendering with delay
                DisableStarLayers();

                WarpIsEnabled = false;
            }
        }

        /// <summary>
        /// Start the star layer rendering with the given delay.
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="shaderProperty"></param>
        /// <returns></returns>
        private IEnumerator StartRenderStarLayerWithDelay(float delay, string shaderProperty) {
            yield return new WaitForSeconds(delay);

             starMaterial.SetInt(shaderProperty, 1);
        }
        #endregion
    }
}

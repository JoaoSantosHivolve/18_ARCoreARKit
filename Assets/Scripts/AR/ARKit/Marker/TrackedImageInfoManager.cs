using System.Collections.Generic;
using AR.ARKit;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace _3rdParty.ARKit.Scenes.ImageTracking
{
    public class TrackedImageInfoManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The camera to set on the world space UI canvas for each instantiated image info.")]
        Camera m_WorldSpaceCanvasCamera;

        /// <summary>
        /// The prefab has a world space UI canvas,
        /// which requires a camera to function properly.
        /// </summary>
        public Camera worldSpaceCanvasCamera
        {
            get { return m_WorldSpaceCanvasCamera; }
            set { m_WorldSpaceCanvasCamera = value; }
        }

        [SerializeField]
        [Tooltip("If an image is detected but no source texture can be found, this texture is used instead.")]
        Texture2D m_DefaultTexture;

        /// <summary>
        /// If an image is detected but no source texture can be found,
        /// this texture is used instead.
        /// </summary>
        public Texture2D defaultTexture
        {
            get { return m_DefaultTexture; }
            set { m_DefaultTexture = value; }
        }

        ARTrackedImageManager m_TrackedImageManager;

        public ARRaycastManager rayCastManager;
        public ArKitManipulationSystem manipulationSystem;

        [Header("Prefabs to Instantiate")]
        [SerializeField] private GameObject prefab;
        public GameObject Prefab
        {
            get => prefab;
            set
            {
                prefab = value;
                m_TrackedImageManager.trackedImagePrefab = value;
            }
        }
        public ArKitManipulatorsManager manipulatorPrefab;
        public GameObject selectionPrefab;
        [Header("Instantiated object animator")]
        public RuntimeAnimatorController runtimeAnimatorController;

        [Header("Instantiated objects")]
        public List<ArKitManipulatorsManager> placedObjects = new List<ArKitManipulatorsManager>();

        private void Awake()
        {
            m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
            Prefab = prefab;
        }
        private void OnEnable()
        {
            m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        private void OnDisable()
        {
            m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

        void UpdateInfo(ARTrackedImage trackedImage)
        {
           //// Set canvas camera
           //var canvas = trackedImage.GetComponentInChildren<Canvas>();
           //canvas.worldCamera = worldSpaceCanvasCamera;
           //
           //// Update information about the tracked image
           //var text = canvas.GetComponentInChildren<Text>();
           //text.text = string.Format(
           //    "{0}\ntrackingState: {1}\nGUID: {2}\nReference size: {3} cm\nDetected size: {4} cm",
           //    trackedImage.referenceImage.name,
           //    trackedImage.trackingState,
           //    trackedImage.referenceImage.guid,
           //    trackedImage.referenceImage.size * 100f,
           //    trackedImage.size * 100f);
           //
           //var planeParentGo = trackedImage.transform.GetChild(0).gameObject;
           //var planeGo = planeParentGo.transform.GetChild(0).gameObject;
           //
            // Disable the visual plane if it is not being tracked
            if (trackedImage.trackingState != TrackingState.None)
            {
                //trackedImage.gameObject.SetActive(true);
                //planeGo.SetActive(true);

                //// The image extents is only valid when the image is being tracked
                //trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);
                //
                //// Set the texture
                //var material = planeGo.GetComponentInChildren<MeshRenderer>().material;
                //material.mainTexture = (trackedImage.referenceImage.texture == null) ? defaultTexture : trackedImage.referenceImage.texture;
            }
            else
            {
                //trackedImage.gameObject.SetActive(false);
                //planeGo.SetActive(false);
            }
        }

        private void SetImage(ARTrackedImage trackedImage)
        {
            trackedImage.gameObject.AddComponent<ArKitObject>();

            // Instantiate object manipulators ( rotate, position, scale, ... )
            var manipulatorsManager = Instantiate(manipulatorPrefab);
            manipulatorsManager.ArKitObject = trackedImage.GetComponent<ArKitObject>();
            manipulatorsManager.rayCastManager = rayCastManager;
            manipulatorsManager.mainCamera = m_WorldSpaceCanvasCamera;

            // Instantiate object selected visual queue ( circle under object )
            var selectionVisualization = Instantiate(selectionPrefab, trackedImage.transform, true);
            selectionVisualization.transform.localPosition = Vector3.zero;
            selectionVisualization.transform.localScale = Vector3.zero;

            // Init prefab components
            trackedImage.transform.parent = manipulatorsManager.transform;
            trackedImage.GetComponent<ArKitObject>().Init(manipulatorsManager, selectionVisualization, runtimeAnimatorController);

            // Set object selected
            manipulationSystem.Select(trackedImage.GetComponent<ArKitObject>());

            // Add to list
            placedObjects.Add(manipulatorsManager);
        }

        public void SetVisibility(bool state)
        {
            foreach (var o in placedObjects)
            {
                if (o == null)
                    continue;

                o.gameObject.SetActive(state);
            }
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                //// Give the initial image a reasonable default scale
                //trackedImage.transform.localScale = new Vector3(0.01f, 1f, 0.01f);
                SetImage(trackedImage);
                //UpdateInfo(trackedImage);
            }

            foreach (var trackedImage in eventArgs.updated)
                UpdateInfo(trackedImage);
        }
    }
}

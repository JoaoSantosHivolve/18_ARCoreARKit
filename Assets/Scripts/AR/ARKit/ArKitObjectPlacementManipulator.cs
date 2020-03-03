using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR.ARKit
{
    public class ArKitObjectPlacementManipulator : MonoBehaviour
    {
        public Camera mainCamera;
        public ARRaycastManager raycastManager;
        public ArKitManipulatorController manipulatorController;

        [Header("Instantiated object animator")]
        public RuntimeAnimatorController runtimeAnimatorController;
        
        [Header("Prefabs to Instantiate")]
        public GameObject placedPrefab;
        public GameObject selectionVisualizationPrefab;
        public ArKitManipulatorsManager objectManipulatorsPrefab;

        private float m_Time;
        private bool m_UsedTwoFingers;

        private void Update()
        {
            var touch = Input.GetTouch(0);

            if (IsPointerOverUiElement(touch.position))
                return;

            if (IsPointerOverGameObject(touch.position))
                return;

            if (Input.touchCount > 1)
                m_UsedTwoFingers = true;

            if (!Tapped(touch) || Input.touchCount != 1 || m_UsedTwoFingers)
                return;

            if (raycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = s_Hits[0].pose;

                if (placedPrefab != null)
                {
                    // Instantiate prefab
                    var prefab = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                    prefab.AddComponent<ArKitObject>();

                    // Instantiate object manipulators ( rotate, position, scale, ... )
                    var manipulatorsManager = Instantiate(objectManipulatorsPrefab);
                    manipulatorsManager.ArKitObject = prefab.GetComponent<ArKitObject>();
                    manipulatorsManager.rayCastManager = raycastManager;
                    manipulatorsManager.mainCamera = mainCamera;

                    // Instantiate object selected visual queue ( circle under object )
                    var selectionVisualization = Instantiate(selectionVisualizationPrefab, prefab.transform, true);
                    selectionVisualization.transform.localPosition = Vector3.zero;
                    selectionVisualization.transform.localScale = prefab.transform.localScale;

                    // Init prefabs components
                    prefab.transform.parent = manipulatorsManager.transform;
                    prefab.GetComponent<ArKitObject>().Init(manipulatorsManager, selectionVisualization, runtimeAnimatorController);

                    // Set object selected
                    manipulatorController.SelectedObject = prefab.GetComponent<ArKitObject>();

                    // Instantiated object order
                    // - Manipulators
                    // - Object
                    // - Object Selected Visual Queue
                }
            }
        }

        private bool Tapped(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
                m_Time = Time.time;
            else if (touch.phase == TouchPhase.Ended)
            {
                m_UsedTwoFingers = false;

                if (Time.time - m_Time < 0.25f)
                    return true;
            }

            return false;
        }
      
        private static bool IsPointerOverUiElement(Vector2 position)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        private bool IsPointerOverGameObject(Vector2 position)
        {
            var ray = mainCamera.ScreenPointToRay(position);

            return Physics.Raycast(ray, out var hit) ? hit.transform.GetComponent<ArKitObject>() : false;
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    }
}
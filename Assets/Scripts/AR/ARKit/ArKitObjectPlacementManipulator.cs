using System.Collections.Generic;
using AR.ARKit.Manipulators;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR.ARKit
{
    public class ArKitObjectPlacementManipulator : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject placedPrefab;

        public ARRaycastManager raycastManager;
        public ArKitManipulatorController manipulatorController;

        public GameObject selectionVisualizationPrefab;
        public ArKitManipulatorsManager objectManipulatorsPrefab;

        public float time;

        private void Update()
        {
            var touch = Input.GetTouch(0);

            if (IsPointerOverUiElement(touch.position))
                return;

            if (IsPointerOverGameObject(touch.position))
                return;

            if (!Tapped(touch))
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
                    prefab.transform.parent = manipulatorsManager.transform;
                    prefab.GetComponent<ArKitObject>().manager = manipulatorsManager;

                    // Instantiate object selected visual queue ( circle under object )
                    var selectionVisualization = Instantiate(selectionVisualizationPrefab, prefab.transform, true);
                    selectionVisualization.transform.localPosition = Vector3.zero;
                    selectionVisualization.transform.localScale = prefab.transform.localScale;
                    prefab.GetComponent<ArKitObject>().selectionVisualization = selectionVisualization;

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
                time = Time.time;
            else if (touch.phase == TouchPhase.Ended)
                if (Time.time - time < 1.0f)
                    return true;

            return false;
        }
        private static bool TryGetTouchPosition(out Vector2 touchPosition)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                var mousePosition = Input.mousePosition;
                touchPosition = new Vector2(mousePosition.x, mousePosition.y);
                return true;
            }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif
            touchPosition = default;
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
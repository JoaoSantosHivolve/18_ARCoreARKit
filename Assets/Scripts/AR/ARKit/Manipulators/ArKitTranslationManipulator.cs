using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR.ARKit.Manipulators
{
    public class ArKitTranslationManipulator : ArKitManipulator
    {
        private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        private bool canMove;

        public override void UpdateManipulator()
        {
            if (Input.touchCount != 1)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverUiElement(touch.position))
                {
                    canMove = false;
                    return;
                }

                var ray = manager.mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out var hit))
                    canMove = arKitObject == hit.transform.GetComponent<ArKitObject>();
                else
                    canMove = false;
            }
            else if (touch.phase == TouchPhase.Moved && canMove)
            {
                if (manager.rayCastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
                {
                    var hitPose = s_Hits[0].pose;

                    arKitObject.transform.position = hitPose.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                canMove = false;
            }
        }
        private static bool IsPointerOverUiElement(Vector2 position)
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = position;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}

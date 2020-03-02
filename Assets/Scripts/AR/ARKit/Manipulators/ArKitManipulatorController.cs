using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitManipulatorController : MonoBehaviour
    {
        public Camera mainCamera;

        private ArKitObject m_SelectedObject;
        public ArKitObject SelectedObject
        {
            get => m_SelectedObject;
            set
            {
                if (m_SelectedObject == null)
                {
                    if (value == null) return;

                    m_SelectedObject = value;
                    m_SelectedObject.IsSelected = true;
                }
                else if (m_SelectedObject != null)
                {
                    m_SelectedObject.IsSelected = false;
                    m_SelectedObject = value;

                    if(value != null)
                        m_SelectedObject.IsSelected = true;
                }
            }
        }
        public bool canManipulate;

        private void Update()
        {
            // Object Tap Detection
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out var hit))
                {
                    canManipulate = hit.transform.GetComponent<ArKitObject>() != null;

                    if (hit.transform.GetComponent<ArKitObject>())
                    {
                        Select(hit.transform.GetComponent<ArKitObject>());
                    }
                }
                else
                {
                    Deselect();
                    canManipulate = false;
                }
            }

            if (canManipulate)
            {
                // Object one finger moving
                if (touch.phase == TouchPhase.Moved && Input.touchCount == 1)
                {
                    // Check if finger position is on plane
                }
            }
            
        }

        private void Select(ArKitObject newObject)
        {
            if (SelectedObject == newObject)
                return;

            Deselect();

            SelectedObject = newObject;
        }

        private void Deselect()
        {
            SelectedObject = null;
        }
    }
}

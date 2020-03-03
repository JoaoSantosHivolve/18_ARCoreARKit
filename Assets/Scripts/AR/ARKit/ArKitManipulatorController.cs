using UnityEngine;

namespace AR.ARKit
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
        private float m_Time;

        private void Update()
        {
            var touch = Input.GetTouch(0);

            if (!Tapped(touch))
                return;

            var ray = mainCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.GetComponent<ArKitObject>())
                {
                    Select(hit.transform.GetComponent<ArKitObject>());
                }
                else
                    Deselect();
            }
            else
            {
                Deselect();
            }
        }
        private bool Tapped(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
                m_Time = Time.time;
            else if (touch.phase == TouchPhase.Ended)
                if (Time.time - m_Time < 0.25f)
                    return true;

            return false;
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

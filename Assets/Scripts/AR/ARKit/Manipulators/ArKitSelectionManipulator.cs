using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitSelectionManipulator : ArKitManipulator
    {
        private bool m_IsSelected;
        public bool IsSelected
        {
            get => m_IsSelected;
            set
            {
                m_IsSelected = value;

                if (m_IsSelected)
                {
                    Select();
                }
                else
                {
                    Deselect();
                }
            }
        }
        private GameObject SelectionVisualization => arKitObject.selectionVisualization;

        private void Select()
        {
            SelectionVisualization.SetActive(true);
        }
        private void Deselect()
        {
            SelectionVisualization.SetActive(false);
        }

        public override void UpdateManipulator()
        {
            // No updates on selection manipulator.
            // Is selected is controller in the ArKitManipulatorController.
        }
    }
}
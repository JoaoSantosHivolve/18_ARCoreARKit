using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitObject : MonoBehaviour
    {
        public ArKitManipulatorsManager manager;
        public GameObject selectionVisualization;

        public bool IsSelected 
        {
            get => manager.selectionManipulator.IsSelected;
            set => manager.selectionManipulator.IsSelected = value;
        }
    }
}
using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitManipulatorsManager : MonoBehaviour
    {
        public ArKitObject ArKitObject
        {
            set
            {
                selectionManipulator.arKitObject = value;
                //translateManipulator.arKitObject = value;
                //rotationManipulator.arKitObject = value;
                //scaleManipulator.arKitObject = value;
            }
        }

        public ArKitSelectionManipulator selectionManipulator;
        //public ArKitManipulator translateManipulator;
        //public ArKitManipulator rotationManipulator;
        //public ArKitManipulator scaleManipulator;
    }
}
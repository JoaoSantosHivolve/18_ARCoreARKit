using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public abstract class ArKitManipulator : MonoBehaviour
    {
        [HideInInspector] public ArKitObject arKitObject;

        private void Update()
        {
            if(arKitObject.IsSelected)
                UpdateManipulator();
        }
        public abstract void UpdateManipulator();
    }
}
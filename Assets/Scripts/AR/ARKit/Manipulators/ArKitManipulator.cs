using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public abstract class ArKitManipulator : MonoBehaviour
    {
        [HideInInspector] public ArKitObject arKitObject;
    }
}
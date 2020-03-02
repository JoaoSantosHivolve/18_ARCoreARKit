using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AR.ARKit.Manipulators
{
    public class ArKitManipulatorsManager : MonoBehaviour
    {
        public ARRaycastManager rayCastManager;
        public Camera mainCamera;
        public ArKitObject ArKitObject
        {
            set
            {
                selectionManipulator.arKitObject = value;
                translationManipulator.arKitObject = value;
                rotationManipulator.arKitObject = value;
                //scaleManipulator.arKitObject = value;
            }
        }

        private void Awake()
        {
            selectionManipulator.manager = this;
            rotationManipulator.manager = this;
            translationManipulator.manager = this;
        }

        public ArKitSelectionManipulator selectionManipulator;
        public ArKitTranslationManipulator translationManipulator;
        public ArKitRotationManipulator rotationManipulator;
        //public ArKitManipulator scaleManipulator;
    }
}
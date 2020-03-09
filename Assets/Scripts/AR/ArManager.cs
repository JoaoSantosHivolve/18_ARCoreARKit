using AR.ARCore;
using AR.ARKit;
using Assets.Scripts.Common;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine;

namespace AR
{
    public class ArManager : Singleton<ArManager>
    {
        [Header("AR Components")]
        public GameObject arKitSection;
        public GameObject arCoreSection;

        [Header("Object placement components")]
        public ArCoreObjectPlacementManipulator arCorePlacementManipulator;
        public ArKitObjectPlacementManipulator arKitPlacementManipulator;

        [Header("Object manipulation systems")] 
        public ArCoreManipulationSystem arCoreManipulationSystem;
        public ArKitManipulationSystem arKitManipulationSystem;

        protected override void Awake()
        {
#if UNITY_ANDROID
            arKitSection.SetActive(false);
            arCoreSection.SetActive(true);
#elif UNITY_IOS
            arKitSection.SetActive(true);
            arCoreSection.SetActive(false);
#endif
        }

        public void SetFloorScanningObject(GameObject prefab)
        {
#if UNITY_ANDROID
            arCorePlacementManipulator.placedPrefab = prefab;
#elif UNITY_IOS
            arKitPlacementManipulator.placedPrefab = prefab;
#endif
        }

        public void SetMarkerObject()
        {

        }
        public void SetPlacedObjectVisibility(bool state)
        {
#if UNITY_ANDROID
            arCorePlacementManipulator.SetVisibility(state);
#elif UNITY_IOS
            arKitPlacementManipulator.SetVisibility(state);
#endif
        }
        public void DeletePlacedObjects()
        {
#if UNITY_ANDROID
            arCorePlacementManipulator.DeletePlacedObjects();;
#elif UNITY_IOS
            arKitPlacementManipulator.DeletePlacedObjects();
#endif
        }
        public void DeleteSelectedObject()
        {
#if UNITY_ANDROID
            arCoreManipulationSystem.Delete();
#elif UNITY_IOS
            arKitManipulationSystem.Delete();
#endif
        }
    }
}

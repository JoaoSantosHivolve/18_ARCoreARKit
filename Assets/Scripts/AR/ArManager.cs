using AR.ARCore;
using Assets.Scripts.Common;
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
            arCorePlacementManipulator.prefab = prefab;
#elif UNITY_IOS
        arKitSection.SetActive(true);
        arCoreSection.SetActive(false);
#endif
        }

    }
}

using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitScalingManipulator : ArKitManipulator
    {
        private const float PinchRatio = 1;
        private const float MinPinchDistance = 0;

        private static float s_PinchDistanceDelta;
        private static float s_PinchDistance;

        public override void UpdateManipulator()
        {
            if (!arKitObject.IsSelected)
                return;

            Calculate();

            arKitObject.transform.localScale = Vector3.one + (Vector3.one * s_PinchDistanceDelta);
        }

        private static void Calculate()
        {
            // if two fingers are touching the screen at the same time ...
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.touches[0];
                Touch touch2 = Input.touches[1];

                // ... if at least one of them moved ...
                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    // ... check the delta distance between them ...
                    s_PinchDistance = Vector2.Distance(touch1.position, touch2.position);

                    float prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition,
                        touch2.position - touch2.deltaPosition);

                    s_PinchDistanceDelta = s_PinchDistance - prevDistance;

                    // ... if it's greater than a minimum threshold, it's a pinch!
                    if (Mathf.Abs(s_PinchDistanceDelta) > MinPinchDistance)
                    {
                        s_PinchDistanceDelta *= PinchRatio;
                    }
                }
            }
        }
    }
}
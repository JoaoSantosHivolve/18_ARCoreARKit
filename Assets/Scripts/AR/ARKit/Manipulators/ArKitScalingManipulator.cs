using System;
using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitScalingManipulator : ArKitManipulator
    {
        private const float PinchRatio = 1;
        private const float MinPinchDistance = 0;

        [Range(1.0f,5.0f)]
        public float maxSize;
        [Range(0.5f,1.0f)]
        public float minSize;

        private float m_PinchDistanceDelta;
        private float PinchDistanceDelta
        {
            get => m_PinchDistanceDelta;
            set
            {
                if (value >= maxSize)
                    m_PinchDistanceDelta = maxSize;
                else if (value <= minSize)
                    m_PinchDistanceDelta = minSize;
                else
                    m_PinchDistanceDelta = value;
            }
        }
        private static float s_PinchDistance;

        public override void UpdateManipulator()
        {
            // Uses LateUpdate instead.
        }

        private void LateUpdate()
        {
            if (!arKitObject.IsSelected)
                return;

            Calculate();

            arKitObject.transform.localScale = Vector3.one + (Vector3.one * PinchDistanceDelta);
        }

        private void Calculate()
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

                    PinchDistanceDelta = s_PinchDistance - prevDistance;

                    // ... if it's greater than a minimum threshold, it's a pinch!
                    if (Mathf.Abs(PinchDistanceDelta) > MinPinchDistance)
                    {
                        PinchDistanceDelta *= PinchRatio;
                    }
                }
            }
        }
    }
}
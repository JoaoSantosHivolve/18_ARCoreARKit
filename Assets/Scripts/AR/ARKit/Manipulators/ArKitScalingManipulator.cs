using Common;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitScalingManipulator : ArKitManipulator
    {
        private const float PinchRatio = 0.5f;
        private const float MinPinchDistance = 1f;

        [Range( 0.00f, 2.00f)]
        public float maxSize;
        [Range(-0.50f, -0.01f)]
        public float minSize;

        public static float pinchDistanceDelta;
        public static float pinchDistance;

        public Vector3 m_Scale;
        public Vector3 Scale
        {
            get => m_Scale;
            set
            {
                m_Scale = value;

                if (m_Scale.x >= maxSize)
                {
                    m_Scale = Vector3.one * maxSize;
                }
                else if(m_Scale.x < minSize)
                {
                    m_Scale = Vector3.one * minSize;
                }
            }
        }

        public bool isScaling;
        public float lastDistance;

        public override void UpdateManipulator()
        {
            // Uses LateUpdate instead.
        }

        private void LateUpdate()
        {
            if (!arKitObject.IsSelected)
                return;

            Calculate();

            float pinchAmount = 0;

            if (Mathf.Abs(pinchDistanceDelta) > 0)
            { // zoom
                pinchAmount = pinchDistanceDelta;
            }

            Scale += Vector3.one * pinchAmount;

            arKitObject.transform.localScale += Scale;
        }
        private void Calculate()
        {
            pinchDistance = pinchDistanceDelta = 0;

            if (Input.touchCount == 2)
            {
                var touch1 = Input.GetTouch(0);
                var touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    pinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    
                    var prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                    pinchDistanceDelta = pinchDistance - prevDistance;

                    // ... if it's greater than a minimum threshold, it's a pinch!
                    if (Mathf.Abs(pinchDistanceDelta) > MinPinchDistance)
                    {
                        pinchDistanceDelta *= PinchRatio;
                    }
                    else
                    {
                        pinchDistance = pinchDistanceDelta = 0;
                    }

                    //// ... check the delta distance between them ...
                    //m_PinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    //
                    //var prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                    //
                    //var distance = m_PinchDistance - prevDistance;
                    //
                    //// ... if it's greater than a minimum threshold, it's a pinch!
                    //if (Mathf.Abs(distance) > MinPinchDistance)
                    //{
                    //    isScaling = true;
                    //
                    //    PinchDistanceDelta = Helper.RemapNumber(distance - MinPinchDistance, 0, 50, minSize, maxSize);
                    //}
                }
            }
            else isScaling = false;
        }
    }
}
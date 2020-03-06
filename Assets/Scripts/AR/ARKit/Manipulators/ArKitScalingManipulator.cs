using Common;
using TMPro;
using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitScalingManipulator : ArKitManipulator
    {
        private const float PinchRatio = 1f;
        private const float MinPinchDistance = 2.5f;

        [Range( 0.00f, 2.00f)]
        public float maxSize;
        [Range(-0.50f, -0.01f)]
        public float minSize;

        private float m_MinPinchDistance = 1;
        private float m_MaxPinchDistance = 100;

        public static float pinchDistanceDelta;
        public static float pinchDistance;

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
            arKitObject.transform.localScale += Vector3.one * pinchAmount;
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

                    DebugText.Instance.Text = m_MinPinchDistance.ToString();

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
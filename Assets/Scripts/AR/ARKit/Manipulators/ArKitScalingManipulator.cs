using Common;
using TMPro;
using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitScalingManipulator : ArKitManipulator
    {
        private const float PinchRatio = 0.05f;
        private const float MinPinchDistance = 2.5f;

        [Range( 0.00f, 2.00f)]
        public float maxSize;
        [Range(-0.50f, -0.01f)]
        public float minSize;

        private float m_MinPinchDistance = 1;
        private float m_MaxPinchDistance = 100;

        private float m_PinchDistanceDelta;

        private float PinchDistanceDelta;
        //{
        //    get => m_PinchDistanceDelta;
        //    set
        //    {
        //        if (value >= maxSize)
        //            m_PinchDistanceDelta = maxSize;
        //        else if (value <= minSize)
        //            m_PinchDistanceDelta = minSize;
        //        else
        //            m_PinchDistanceDelta = value;
        //    }
        //}
        private float m_PinchDistance;

        public bool isScaling;

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

        private Vector2 firstTouchPos1;
        private Vector2 firstTouchPos2;

        private void Calculate()
        {
            if (Input.touchCount == 1)
            {
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                    firstTouchPos1 = Input.GetTouch(0).position;
            }

            if (Input.touchCount == 2)
            {
                if (Input.GetTouch(1).phase == TouchPhase.Began)
                    firstTouchPos2 = Input.GetTouch(1).position;

                var touch1 = Input.GetTouch(0);
                var touch2 = Input.GetTouch(1);

                var scaleValue = 0.0f;
                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    scaleValue += Vector2.Distance(firstTouchPos1, touch1.position);
                    scaleValue += Vector2.Distance(firstTouchPos2, touch2.position);

                    DebugText.Instance.Text = scaleValue.ToString();

                    if (scaleValue > m_MinPinchDistance)
                    {
                        isScaling = true;
                        PinchDistanceDelta = Helper.RemapNumber(scaleValue, 0, m_MaxPinchDistance, minSize, maxSize);
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
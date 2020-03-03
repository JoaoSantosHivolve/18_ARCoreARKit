using UnityEngine;

namespace AR.ARKit.Manipulators
{
    public class ArKitScalingManipulator : ArKitManipulator
    {
        private const float PinchRatio = 0.1f;
        private const float MinPinchDistance = 0;

        [Range(1.00f,5.00f)]
        public float maxSize;
        [Range(-1.00f,0.99f)]
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
        private float m_PinchDistance;

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
            if (Input.touchCount != 2) 
                return;

            var touch1 = Input.touches[0];
            var touch2 = Input.touches[1];

            // ... if at least one of them moved ...
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                // ... check the delta distance between them ...
                m_PinchDistance = Vector2.Distance(touch1.position, touch2.position);

                var prevDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);

                PinchDistanceDelta = m_PinchDistance - prevDistance;

                // ... if it's greater than a minimum threshold, it's a pinch!
                if (Mathf.Abs(PinchDistanceDelta) > MinPinchDistance)
                {
                    PinchDistanceDelta *= PinchRatio;
                }
            }
        }
    }
}
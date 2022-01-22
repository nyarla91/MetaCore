using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.Touch;

namespace NyarlaEssentials.Pointers
{
    public class TouchWatcher : MonoBehaviour
    {
        private static TouchWatcher _instance;
        private static readonly float ScreenSize = Mathf.Max(Screen.width, Screen.height);

        private Touch[] _currentTouches = new Touch[0];

        public Vector2[] CurrentTouchesPositions
        {
            get
            {
                Vector2[] result = new Vector2[_currentTouches.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = _currentTouches[i].position;
                }
                return result;
            }
        }
        
        public Vector2[] CurrentTouchesDeltaPositions
        {
            get
            {
                Vector2[] result = new Vector2[_currentTouches.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = _currentTouches[i].deltaPosition;
                }
                return result;
            }
        }
        
        public Vector2[] PreviousTouchesPositions
        {
            get
            {
                Vector2[] result = new Vector2[_currentTouches.Length];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = _currentTouches[i].position - _currentTouches[i].deltaPosition;
                }
                return result;
            }
        }

        private int TouchesCount => _currentTouches.Length;

        public delegate void SwipeHandler(SwipeContext context);
        public static SwipeHandler OnSwipe;

        private bool _leftMousePressed;

        private void Awake()
        {
            _instance = this;
            PointerControls.Actions.Touch.Enable();
            PointerControls.Actions.Touch.Tap.started += SwipeWithTouch;
            PointerControls.Actions.Mouse.LeftClick.started += PressLeftMouseButton;
            PointerControls.Actions.Mouse.LeftMouse.canceled += ReleaseLeftMouseButton;
            PointerControls.Actions.Mouse.LeftMouse.started += SwipeWithMouse;
        }

        private void Update()
        {
            UpdateTouches();
        }

        private void UpdateTouches()
        {
            _currentTouches = Input.touches;
        }

        public static Vector2 DeltaDrag(int touches)
        {
            if (touches != _instance.TouchesCount)
                return Vector2.zero;

            Vector2 delta = NEVectors.LerpMulti(_instance.CurrentTouchesDeltaPositions);
            delta.y = -delta.y;
            return delta / ScreenSize;
        }
        
        public static float DeltaZoom(int touches)
        {
            if (touches < 2 || touches != _instance.TouchesCount)
            {
                return 0;
            }

            float currentTouchDelta = AverageDistanceFromCenter(_instance.CurrentTouchesPositions);
            float previousTouchDelta = AverageDistanceFromCenter(_instance.PreviousTouchesPositions);

            return (previousTouchDelta - currentTouchDelta) / ScreenSize;
            
            float AverageDistanceFromCenter(Vector2[] inspectedTouches)
            {
                Vector2 center = NEVectors.LerpMulti(inspectedTouches);
                float[] distances = new float[_instance.TouchesCount];
                for (int i = 0; i < distances.Length; i++)
                {
                    distances[i] = Vector2.Distance(center, inspectedTouches[i]);
                }
                return NEMath.Average(distances);
            }
        }

        public static float DeltaTwist(int touches)
        {
            if (touches < 2 || touches != _instance.TouchesCount)
            {
                return 0;
            }

            float[] deltaAngles = new float[_instance.TouchesCount];
            Vector2 touchesCenter = NEVectors.LerpMulti(_instance.PreviousTouchesPositions);
            for (int i = 0; i < _instance.TouchesCount; i++)
            {
                deltaAngles[i] = DeltaAngleForTouch(touchesCenter, _instance._currentTouches[i]);
            }

            return -NEMath.Average(deltaAngles);
            
            float DeltaAngleForTouch(Vector2 center, Touch inspectedTouch)
            {
                const float MinDistancePercent = 0.1f;

                if (Vector2.Distance(inspectedTouch.position, center) < ScreenSize * MinDistancePercent)
                {
                    return 0;
                }
                
                float previousAngle = NEVectors.ToDegrees(inspectedTouch.position - inspectedTouch.deltaPosition - center);
                float currentAngle = NEVectors.ToDegrees(inspectedTouch.position - center);
                float deltaAngle = currentAngle - previousAngle;

                return deltaAngle > 180 ? 360 - deltaAngle : deltaAngle;
            }
        }

        private void SwipeWithTouch(InputAction.CallbackContext context) => Swipe(true);
        
        private void SwipeWithMouse(InputAction.CallbackContext context) => Swipe(false);

        private async void Swipe(bool isTouch)
        {
            SwipeContext swipeContext = await PointerLifecycle(isTouch);
            if (OnSwipe != null)
                OnSwipe(swipeContext);
        }

        private async Task<SwipeContext> PointerLifecycle(bool isTouch)
        {
            Vector2 originPosition = PointerPosition();
            float time = 0;
            Vector2 releasePosition = PointerPosition();
            while (IsHolded())
            {
                time += Time.deltaTime;
                releasePosition = PointerPosition();
                await Task.Delay(1);
            }
            return new SwipeContext(originPosition, releasePosition, time);

            bool IsHolded() => isTouch ? Input.touchCount > 0 : _leftMousePressed;
            Vector2 PointerPosition() => isTouch ? Input.GetTouch(0).position : (Vector2) Input.mousePosition;
        }

        private void PressLeftMouseButton(InputAction.CallbackContext context) => _leftMousePressed = true;
        private void ReleaseLeftMouseButton(InputAction.CallbackContext context) => _leftMousePressed = false;

        private void OnDestroy()
        {
            PointerControls.Actions.Touch.Tap.started -= SwipeWithTouch;
            PointerControls.Actions.Mouse.LeftClick.started -= SwipeWithMouse;
            PointerControls.Actions.Mouse.LeftClick.started -= PressLeftMouseButton;
            PointerControls.Actions.Mouse.LeftClick.canceled -= ReleaseLeftMouseButton;
        }

        public class SwipeContext
        {
            private readonly Vector2 _originPosition;
            public Vector2 OriginPosition => _originPosition;
            
            private readonly Vector2 _releasePosition;
            public Vector2 ReleasePosition => _releasePosition;
            
            private readonly float _time;
            public float Time => _time;
            
            private readonly Vector2 _deltaPosition;
            public Vector2 DeltaPosition => _deltaPosition;
            
            private readonly float _angle;
            public float Angle => _angle;

            private readonly Vector2 _direction;
            public Vector2 Direction => _direction;

            private readonly float _distance;
            public float Distance => _distance;

            public SwipeContext(Vector2 originPosition, Vector2 releasePosition, float time)
            {
                _originPosition = originPosition / ScreenSize;
                _releasePosition = releasePosition / ScreenSize;
                _time = time;
                _deltaPosition = ReleasePosition - OriginPosition;
                _direction = DeltaPosition.normalized;
                _distance = DeltaPosition.magnitude;
                _angle = NEVectors.ToDegrees(DeltaPosition);
            }

            public int RoundedAngle(int step) => Mathf.RoundToInt(NEMath.Snap(Angle, step));
        }
    }
}
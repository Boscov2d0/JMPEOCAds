using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Gameplay.PlayerLogic.Input
{
    public class LookInputService : IPlayerLookInputService
    {
        private const float RIGHT_SIDE_START = 0.5f;
        private const int UNASSIGNED_FINGER_ID = -1;

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        private int _lookFingerId = UNASSIGNED_FINGER_ID;
        private bool _isMouseLooking;
        private Vector2 _lookDelta;
        private Vector2 _previousMousePosition;
        private bool _hasPreviousMousePosition;

        public Vector2 LookDelta => _lookDelta;

        public void Tick()
        {
            _lookDelta = UnityEngine.Input.touchCount > 0
                ? ReadTouchDelta()
                : ReadMouseDelta();
        }

        private Vector2 ReadTouchDelta()
        {
            _isMouseLooking = false;
            _hasPreviousMousePosition = false;

            Vector2 delta = Vector2.zero;
            bool lookTouchFound = false;

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch touch = UnityEngine.Input.GetTouch(i);
                if (_lookFingerId == UNASSIGNED_FINGER_ID || touch.fingerId != _lookFingerId)
                    continue;

                lookTouchFound = true;

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    delta = touch.deltaPosition;
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _lookFingerId = UNASSIGNED_FINGER_ID;
                    lookTouchFound = false;
                }

                break;
            }

            if (lookTouchFound == false)
                _lookFingerId = UNASSIGNED_FINGER_ID;

            if (_lookFingerId == UNASSIGNED_FINGER_ID)
                TryCaptureLookTouch();

            return delta;
        }

        private void TryCaptureLookTouch()
        {
            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch touch = UnityEngine.Input.GetTouch(i);
                if (touch.phase != TouchPhase.Began)
                    continue;

                if (touch.position.x < Screen.width * RIGHT_SIDE_START)
                    continue;

                if (IsPointerOverUi(touch.position, touch.fingerId))
                    continue;

                _lookFingerId = touch.fingerId;
                return;
            }
        }

        private Vector2 ReadMouseDelta()
        {
            _lookFingerId = UNASSIGNED_FINGER_ID;

            Vector2 mousePosition = UnityEngine.Input.mousePosition;
            Vector2 delta = _hasPreviousMousePosition ? mousePosition - _previousMousePosition : Vector2.zero;
            _previousMousePosition = mousePosition;
            _hasPreviousMousePosition = true;

            bool mousePressed = UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButton(1);
            bool mouseDown = UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetMouseButtonDown(1);

            if (mouseDown)
            {
                _isMouseLooking = mousePosition.x >= Screen.width * RIGHT_SIDE_START
                    && IsPointerOverUi(mousePosition, -1) == false;
            }

            if (mousePressed == false)
            {
                _isMouseLooking = false;
                return Vector2.zero;
            }

            if (_isMouseLooking == false)
                return Vector2.zero;

            return delta;
        }

        private bool IsPointerOverUi([Bridge.Ref] Vector2 screenPosition, int pointerId)
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return false;

            if (eventSystem.IsPointerOverGameObject(pointerId))
                return true;

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = screenPosition
            };

            _raycastResults.Clear();
            eventSystem.RaycastAll(pointerData, _raycastResults);
            return _raycastResults.Count > 0;
        }
    }
}

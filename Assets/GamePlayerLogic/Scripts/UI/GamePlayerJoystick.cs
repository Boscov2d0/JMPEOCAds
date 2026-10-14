using System.Collections.Generic;
using Code.Gameplay.PlayerLogic.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Gameplay.PlayerLogic.UI
{
    public class GamePlayerJoystick : MonoBehaviour, IPlayerJoystickInput
    {
        private const float DEFAULT_HANDLE_RANGE = 1f;
        private const float LEFT_SIDE_END = 0.5f;
        private const int UNASSIGNED_POINTER_ID = int.MinValue;

        [SerializeField] private CanvasSelector _canvasSelector;
        [SerializeField] private PlayerHUDView _playerHUDView;
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;
        [SerializeField] private float _handleRange = DEFAULT_HANDLE_RANGE;
        [SerializeField] private GameObject _finger;
        [SerializeField] private float _timeActivate = 2f;

        private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>();

        private Vector2 _axis;
        private Vector2 _defaultAnchoredPosition;
        private float _idleTimer;
        private bool _isMoving;
        private bool _canActivateFinger;
        private int _activePointerId = UNASSIGNED_POINTER_ID;
        private Image _joystickImage;
        private Camera _eventCamera;
        private Canvas _canvas;
        private bool _mouseHeld;
        private Vector2 _originScreen;
        private float _radiusScreen;

        public float Horizontal => _axis.x;
        public float Vertical => _axis.y;
        
        private void Awake() => _canvasSelector.ChangeCanvas += SetUIElement;
        private void SetUIElement(IActiveCanvas canvas)
        {
            _playerHUDView = canvas as PlayerHUDView;
            _canvas = _playerHUDView.GetComponent<Canvas>();
            _background = _playerHUDView.Background;
            _handle = _playerHUDView.JoystickHandle;
            _finger = _playerHUDView.Finger;

            EnsureDefaultLayout();

            _defaultAnchoredPosition = _background.anchoredPosition;
            _joystickImage = _background.GetComponent<Image>();

            _eventCamera = _canvas != null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay
                ? _canvas.worldCamera
                : null;

            if (_joystickImage != null)
                _joystickImage.raycastTarget = false;

            SetJoystickVisible(false);
            SetFingerActive(false);
        }
        private void OnEnable()
        {
            GamePlayerContext context = GamePlayerContext.Instance;
            context?.RegisterJoystick(this);
        }

        private void OnDisable()
        {
            _canvasSelector.ChangeCanvas -= SetUIElement;
            GamePlayerContext context = GamePlayerContext.Resolve();
            context?.UnregisterJoystick(this);
        }

        private void OnValidate() => EnsureDefaultLayout();

        private void Update()
        {
            UpdatePointer();
            UpdateIdleFinger();
            PublishInput();
        }

        private void UpdatePointer()
        {
            // Playworks turns a touch into mouse input, but also reports touchCount.
            // A touch that is no longer in Began must not block that mouse path.
            if (UnityEngine.Input.touchCount > 0 && UpdateTouchPointer())
                return;

            UpdateMousePointer();
        }

        private bool UpdateTouchPointer()
        {
            if (_isMoving && _activePointerId == UNASSIGNED_POINTER_ID)
                return false;

            if (_isMoving)
            {
                for (int i = 0; i < UnityEngine.Input.touchCount; i++)
                {
                    Touch touch = UnityEngine.Input.GetTouch(i);
                    if (touch.fingerId != _activePointerId)
                        continue;

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        EndControl();
                    else
                        UpdateAxisFromScreen(touch.position);

                    return true;
                }

                EndControl();
                return true;
            }

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch touch = UnityEngine.Input.GetTouch(i);
                // Playworks advances Began to Stationary inside Input.Update.
                // A drag still arrives as Moved before gameplay Update, so start from that too.
                if (touch.phase != TouchPhase.Began && touch.phase != TouchPhase.Moved)
                    continue;

                if (CanBeginControl(touch.position) == false)
                    continue;

                _activePointerId = touch.fingerId;
                BeginControl(touch.position);
                return true;
            }

            return false;
        }

        private void UpdateMousePointer()
        {
            Vector2 mousePosition = UnityEngine.Input.mousePosition;
            bool mouseDown = UnityEngine.Input.GetMouseButton(0);
            // GetMouseButtonDown is cleared on Playworks prerender and is easy to miss.
            bool mousePressed = (mouseDown && _mouseHeld == false) || UnityEngine.Input.GetMouseButtonDown(0);
            _mouseHeld = mouseDown;

            if (_isMoving)
            {
                if (mouseDown == false)
                    EndControl();
                else
                    UpdateAxisFromScreen(mousePosition);

                return;
            }

            if (mousePressed == false)
                return;

            if (CanBeginControl(mousePosition) == false)
                return;

            _activePointerId = UNASSIGNED_POINTER_ID;
            BeginControl(mousePosition);
        }

        private bool CanBeginControl(Vector2 screenPosition)
        {
            if (screenPosition.x >= Screen.width * LEFT_SIDE_END)
                return false;

            return IsBlockedByOtherUi(screenPosition) == false;
        }

        private bool IsBlockedByOtherUi(Vector2 screenPosition)
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
                return false;

            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = screenPosition
            };

            _raycastResults.Clear();
            eventSystem.RaycastAll(pointerData, _raycastResults);

            for (int i = 0; i < _raycastResults.Count; i++)
            {
                GameObject hitObject = _raycastResults[i].gameObject;
                if (hitObject == null || hitObject == gameObject || hitObject.transform.IsChildOf(transform))
                    continue;

                return true;
            }

            return false;
        }

        private void BeginControl(Vector2 screenPosition)
        {
            _isMoving = true;
            _idleTimer = 0f;
            _originScreen = screenPosition;
            _radiusScreen = GetScreenRadius();
            SetFingerActive(false);
            MoveJoystickTo(screenPosition);
            SetJoystickVisible(true);
            UpdateAxisFromScreen(screenPosition);
        }

        private void EndControl()
        {
            _isMoving = false;
            _activePointerId = UNASSIGNED_POINTER_ID;
            _idleTimer = 0f;
            _axis = Vector2.zero;

            if (_handle != null)
                _handle.anchoredPosition = Vector2.zero;

            SetJoystickVisible(false);
            _background.anchoredPosition = _defaultAnchoredPosition;
        }

        private void MoveJoystickTo(Vector2 screenPosition)
        {
            if (_background == null)
                return;

            RectTransform parent = _background.parent as RectTransform;
            if (parent == null
                || parent.rect.width < 1f
                || parent.rect.height < 1f
                || TryScreenToLocal(parent, screenPosition, out Vector2 localPoint) == false)
            {
                _background.position = screenPosition;
                return;
            }

            _background.anchoredPosition = localPoint - GetAnchorReference(parent, _background);
        }

        private void UpdateAxisFromScreen(Vector2 screenPosition)
        {
            float radius = _radiusScreen > 1f ? _radiusScreen : GetScreenRadius();
            Vector2 delta = screenPosition - _originScreen;
            Vector2 clamped = Vector2.ClampMagnitude(delta, radius);
            float axisX = clamped.x / radius;
            float axisY = clamped.y / radius;
            _axis = new Vector2(axisX, axisY);

            if (_handle == null)
                return;

            float localRadius = GetLocalRadius();
            _handle.anchoredPosition = new Vector2(axisX * localRadius, axisY * localRadius);
        }

        private void PublishInput()
        {
            GamePlayerContext context = GamePlayerContext.Resolve();
            if (context == null)
                return;

            context.RegisterJoystick(this);
            context.ReportJoystick(_axis.x, _axis.y);
        }

        private float GetScreenRadius()
        {
            float localRadius = GetLocalRadius();
            float scale = _background != null ? Mathf.Abs(_background.lossyScale.x) : 1f;
            if (scale < 0.0001f)
                scale = 1f;

            if (_canvas != null && Mathf.Abs(scale - 1f) < 0.05f)
            {
                float canvasScale = Mathf.Abs(_canvas.scaleFactor);
                if (canvasScale > 0.05f)
                    scale = canvasScale;
            }

            return Mathf.Max(localRadius * scale, 1f);
        }

        private float GetLocalRadius()
        {
            if (_background == null)
                return 80f;

            float width = _background.rect.width;
            if (width < 1f)
                width = Mathf.Abs(_background.sizeDelta.x);

            if (width < 1f)
                width = 160f;

            return width * 0.5f * _handleRange;
        }

        // Playworks raycasts overlay UI against one plane and ScreenPointToLocalPointInRectangle
        // returns false, so the handle never leaves the center. Fall back to the pivot on screen.
        private bool TryScreenToLocal(RectTransform rect, Vector2 screenPosition, out Vector2 localPoint)
        {
            localPoint = Vector2.zero;
            if (rect == null)
                return false;

            bool converted = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rect,
                screenPosition,
                _eventCamera,
                out localPoint);

            float extent = Mathf.Max(rect.rect.width, rect.rect.height);
            if (extent < 1f)
                extent = Mathf.Max(Mathf.Abs(rect.sizeDelta.x), Mathf.Abs(rect.sizeDelta.y));

            float limit = Mathf.Max(extent * 4f, 100f);
            if (converted && localPoint.sqrMagnitude <= limit * limit)
                return true;

            return TryScreenToLocalManual(rect, screenPosition, out localPoint);
        }

        private bool TryScreenToLocalManual(RectTransform rect, Vector2 screenPosition, out Vector2 localPoint)
        {
            Vector2 pivotScreen = _eventCamera == null
                ? (Vector2)rect.position
                : (Vector2)RectTransformUtility.WorldToScreenPoint(_eventCamera, rect.position);

            Vector3 lossy = rect.lossyScale;
            float scaleX = Mathf.Abs(lossy.x) > 0.0001f ? lossy.x : 1f;
            float scaleY = Mathf.Abs(lossy.y) > 0.0001f ? lossy.y : 1f;

            if (_canvas != null && _canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                float canvasScale = _canvas.scaleFactor;
                bool transformIsUnscaled = Mathf.Abs(Mathf.Abs(scaleX) - 1f) < 0.05f;
                if (canvasScale > 0.0001f && transformIsUnscaled && Mathf.Abs(canvasScale - 1f) > 0.05f)
                {
                    scaleX = canvasScale;
                    scaleY = canvasScale;
                }
            }

            localPoint = new Vector2(
                (screenPosition.x - pivotScreen.x) / scaleX,
                (screenPosition.y - pivotScreen.y) / scaleY);
            return true;
        }

        private static Vector2 GetAnchorReference(RectTransform parent, RectTransform child)
        {
            Rect parentRect = parent.rect;
            Vector2 anchor = (child.anchorMin + child.anchorMax) * 0.5f;
            return new Vector2(
                Mathf.Lerp(parentRect.xMin, parentRect.xMax, anchor.x),
                Mathf.Lerp(parentRect.yMin, parentRect.yMax, anchor.y));
        }

        private void UpdateIdleFinger()
        {
            if (!_canActivateFinger)
                return;

            if (_isMoving)
                return;

            _idleTimer += Time.deltaTime;

            if (_idleTimer >= _timeActivate)
                SetFingerActive(true);
        }

        private void SetJoystickVisible(bool isVisible)
        {
            if (_joystickImage != null)
                _joystickImage.enabled = isVisible;

            if (_handle != null && _handle.gameObject.activeSelf != isVisible)
                _handle.gameObject.SetActive(isVisible);
        }

        private void SetFingerActive(bool isActive)
        {
            if (_finger != null && _finger.activeSelf != isActive)
                _finger.SetActive(isActive);
        }

        private void EnsureDefaultLayout()
        {
            RectTransform rect = transform as RectTransform;
            if (rect != null && (rect.sizeDelta.x < 10f || rect.sizeDelta.y < 10f))
            {
                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.zero;
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.sizeDelta = new Vector2(220f, 220f);
                rect.anchoredPosition = new Vector2(180f, 180f);
            }

            if (_handle != null && (_handle.sizeDelta.x < 10f || _handle.sizeDelta.y < 10f))
            {
                _handle.anchorMin = new Vector2(0.5f, 0.5f);
                _handle.anchorMax = new Vector2(0.5f, 0.5f);
                _handle.pivot = new Vector2(0.5f, 0.5f);
                _handle.sizeDelta = new Vector2(90f, 90f);
                _handle.anchoredPosition = Vector2.zero;
            }
        }
        public void ActivateFingerAnimation() => _canActivateFinger = true;
    }
}

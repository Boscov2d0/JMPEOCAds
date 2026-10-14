using Code.Gameplay.PlayerLogic.Input;
using Code.Gameplay.PlayerLogic.Services;
using UnityEngine;

namespace Code.Gameplay.PlayerLogic
{
    [DefaultExecutionOrder(-10000)]
    public class GamePlayerContext : MonoBehaviour
    {
        public static GamePlayerContext Instance { get; private set; }

        public static GamePlayerContext Resolve()
        {
            if (Instance != null)
                return Instance;

            Instance = FindObjectOfType<GamePlayerContext>();
            return Instance;
        }

        private LookInputService _lookInputService;
        private CompositeInputService _inputService;
        private PlayerSpawnService _spawnService;
        private Transform _cameraTransform;
        private bool _created;
        private bool _isControlEnabled = true;

        public bool IsControlEnabled => _isControlEnabled;

        public void SetControlEnabled(bool isEnabled) => _isControlEnabled = isEnabled;

        public IPlayerInputService InputService
        {
            get
            {
                EnsureCreated();
                return _inputService;
            }
        }

        public IPlayerLookInputService LookInputService
        {
            get
            {
                EnsureCreated();
                return _lookInputService;
            }
        }

        public IPlayerSpawnService SpawnService
        {
            get
            {
                EnsureCreated();
                return _spawnService;
            }
        }

        public Transform CameraTransform
        {
            get
            {
                if (_cameraTransform != null)
                    return _cameraTransform;

                Camera main = Camera.main;
                return main != null ? main.transform : null;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            EnsureCreated();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void Update() => _lookInputService?.Tick();

        public void RegisterJoystick(IPlayerJoystickInput joystick)
        {
            EnsureCreated();
            _inputService.SetJoystick(joystick);
        }

        public void ReportJoystick(float horizontal, float vertical)
        {
            EnsureCreated();
            _inputService.ReportJoystick(horizontal, vertical);
        }

        public void UnregisterJoystick(IPlayerJoystickInput joystick)
        {
            EnsureCreated();
            if (_inputService.Joystick == joystick)
            {
                _inputService.ReportJoystick(0f, 0f);
                _inputService.SetJoystick(null);
            }
        }

        public void RegisterJumpButton(IPlayerJumpButtonInput jumpButton)
        {
            EnsureCreated();
            _inputService.SetJumpButton(jumpButton);
        }

        public void UnregisterJumpButton(IPlayerJumpButtonInput jumpButton)
        {
            EnsureCreated();
            _inputService.SetJumpButton(null);
        }

        public void RegisterCamera(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }

        public void UnregisterCamera(Transform cameraTransform)
        {
            if (_cameraTransform == cameraTransform)
                _cameraTransform = null;
        }

        private void EnsureCreated()
        {
            if (_created)
                return;

            Instance = this;
            _lookInputService = new LookInputService();
            _inputService = new CompositeInputService(new KeyboardInputService());
            _spawnService = new PlayerSpawnService();
            _created = true;
        }
    }
}

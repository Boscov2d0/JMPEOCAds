using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Input
{
    public class CompositeInputService : IPlayerInputService
    {
        private readonly IPlayerInputService _keyboard;
        private IPlayerJoystickInput _joystick;
        private IPlayerJumpButtonInput _jumpButton;
        private float _joystickHorizontal;
        private float _joystickVertical;
        private bool _hasJoystickSample;

        public CompositeInputService(IPlayerInputService keyboard)
        {
            _keyboard = keyboard;
        }

        public IPlayerJoystickInput Joystick => _joystick;

        public void SetJoystick(IPlayerJoystickInput joystick) => _joystick = joystick;

        public void SetJumpButton(IPlayerJumpButtonInput jumpButton) => _jumpButton = jumpButton;

        public void ReportJoystick(float horizontal, float vertical)
        {
            _joystickHorizontal = horizontal;
            _joystickVertical = vertical;
            _hasJoystickSample = true;
        }

        public Vector2 MoveInput
        {
            get
            {
                Vector2 keyboard = _keyboard != null ? _keyboard.MoveInput : Vector2.zero;
                Vector2 mobile = ReadJoystick();
                return mobile.sqrMagnitude >= keyboard.sqrMagnitude ? mobile : keyboard;
            }
        }

        private Vector2 ReadJoystick()
        {
            if (_hasJoystickSample)
                return Vector2.ClampMagnitude(new Vector2(_joystickHorizontal, _joystickVertical), 1f);

            if (_joystick == null)
                return Vector2.zero;

            return Vector2.ClampMagnitude(new Vector2(_joystick.Horizontal, _joystick.Vertical), 1f);
        }

        public bool IsJumpPressed
        {
            get
            {
                bool keyboardJump = _keyboard != null && _keyboard.IsJumpPressed;
                bool mobileJump = _jumpButton != null && _jumpButton.IsJumpPressed;
                return keyboardJump || mobileJump;
            }
        }
    }
}

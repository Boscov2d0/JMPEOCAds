using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Input
{
    public class MobileInputService : IPlayerInputService
    {
        private readonly IPlayerJoystickInput _joystick;
        private readonly IPlayerJumpButtonInput _jumpButton;

        public MobileInputService(IPlayerJoystickInput joystick, IPlayerJumpButtonInput jumpButton)
        {
            _joystick = joystick;
            _jumpButton = jumpButton;
        }

        public Vector2 MoveInput
        {
            get
            {
                if (_joystick == null)
                    return Vector2.zero;

                return Vector2.ClampMagnitude(new Vector2(_joystick.Horizontal, _joystick.Vertical), 1f);
            }
        }

        public bool IsJumpPressed => _jumpButton != null && _jumpButton.IsJumpPressed;
    }
}

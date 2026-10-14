using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Input
{
    public class KeyboardInputService : IPlayerInputService
    {
        public Vector2 MoveInput
        {
            get
            {
                float horizontal = 0f;
                float vertical = 0f;

                if (UnityEngine.Input.GetKey(KeyCode.D))
                    horizontal += 1f;

                if (UnityEngine.Input.GetKey(KeyCode.A))
                    horizontal -= 1f;

                if (UnityEngine.Input.GetKey(KeyCode.W))
                    vertical += 1f;

                if (UnityEngine.Input.GetKey(KeyCode.S))
                    vertical -= 1f;

                return Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1f);
            }
        }

        public bool IsJumpPressed => UnityEngine.Input.GetKeyDown(KeyCode.Space);
    }
}

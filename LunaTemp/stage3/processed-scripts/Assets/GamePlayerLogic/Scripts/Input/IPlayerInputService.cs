using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Input
{
    public interface IPlayerInputService
    {
        Vector2 MoveInput { get; }
        bool IsJumpPressed { get; }
    }
}

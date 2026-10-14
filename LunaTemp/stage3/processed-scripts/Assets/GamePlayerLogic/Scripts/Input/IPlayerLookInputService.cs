using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Input
{
    public interface IPlayerLookInputService
    {
        Vector2 LookDelta { get; }
    }
}

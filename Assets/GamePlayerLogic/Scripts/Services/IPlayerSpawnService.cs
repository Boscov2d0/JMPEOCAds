using Code.Gameplay.PlayerLogic.Behaviours;
using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Services
{
    public interface IPlayerSpawnService
    {
        Transform Player { get; }
        GamePlayerCharacter Character { get; }
        void RegisterPoint(GamePlayerSpawnPoint spawnPoint);
        void UnregisterPoint(GamePlayerSpawnPoint spawnPoint);
        void Spawn(GamePlayerSpawnPoint spawnPoint);
        void Respawn();
        void Remove();
    }
}

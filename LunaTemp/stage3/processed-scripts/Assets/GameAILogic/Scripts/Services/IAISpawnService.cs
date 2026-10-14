using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Services;

public interface IAISpawnService
{
    void Spawn(AISpawnPoint spawnPoint);
    void TryRespawn(AICharacter aiCharacter);
    void RestartAtSpawn(AICharacter aiCharacter);
    void DespawnAll();
}

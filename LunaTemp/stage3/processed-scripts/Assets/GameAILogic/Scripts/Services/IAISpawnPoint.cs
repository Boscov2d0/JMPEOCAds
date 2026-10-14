using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Brains;
using UnityEngine;

public interface IAISpawnPoint
{
    bool ShouldReplaceAIBrain { get; }
    Transform Transform { get; }
    GameObject GameObject { get; }
    AICharacter Prefab { get; }
    AIBrain CreateAIBrain();
}

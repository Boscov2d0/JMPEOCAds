using UnityEngine;

namespace Code.Gameplay.AI.Brains
{
    public abstract class AIBrainProvider : MonoBehaviour, IAIBrainProvider
    {
        public abstract AIBrain CreateAIBrain();
    }
}

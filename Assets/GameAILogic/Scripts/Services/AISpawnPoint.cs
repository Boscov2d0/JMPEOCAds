using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Brains;
using UnityEngine;

namespace Code.Gameplay.AI.Services
{
    public class AISpawnPoint : MonoBehaviour, IAISpawnPoint
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private AICharacter _prefab;
        [SerializeField] private bool _shouldReplaceAIBrain;
        [SerializeField] private AIBrainProvider _aiBrainProvider;
        
        public bool ShouldReplaceAIBrain => _shouldReplaceAIBrain;
        public Transform Transform => _spawnPoint.transform;
        public GameObject GameObject => _spawnPoint.gameObject;
        public AICharacter Prefab => _prefab;

        public void SetBrainProvider(AIBrainProvider aiBrainProvider)
        {
            _aiBrainProvider = aiBrainProvider;
        }
        
        public AIBrain CreateAIBrain()
        {
            return _aiBrainProvider != null ? _aiBrainProvider.CreateAIBrain() : null;
        }
    }
}
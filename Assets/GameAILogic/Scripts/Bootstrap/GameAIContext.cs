using Code.Gameplay.AI.Factories;
using Code.Gameplay.AI.Services;
using UnityEngine;

namespace Code.Gameplay.AI
{
    [DefaultExecutionOrder(-10000)]
    public class GameAIContext : MonoBehaviour
    {
        public static GameAIContext Instance { get; private set; }

        private BotTickService _tickService;
        private AIBrainFactory _brainFactory;
        private AISpawnService _spawnService;
        private bool _created;

        public IBotTickService TickService
        {
            get
            {
                EnsureCreated();
                return _tickService;
            }
        }

        public IAIBrainFactory BrainFactory
        {
            get
            {
                EnsureCreated();
                return _brainFactory;
            }
        }

        public IAISpawnService SpawnService
        {
            get
            {
                EnsureCreated();
                return _spawnService;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            EnsureCreated();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void Update() => _tickService?.Tick();

        private void EnsureCreated()
        {
            if (_created)
                return;

            Instance = this;
            _tickService = new BotTickService();
            _brainFactory = new AIBrainFactory();
            _spawnService = new AISpawnService(_tickService);
            _created = true;
        }
    }
}

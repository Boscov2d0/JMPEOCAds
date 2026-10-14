using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Services
{
    public class GamePlayerSpawnController : MonoBehaviour
    {
        [SerializeField] private GamePlayerSpawnPoint _spawnPoint;
        [SerializeField] private bool _spawnOnStart = true;

        private IPlayerSpawnService _spawnService;

        public void Construct(IPlayerSpawnService spawnService)
        {
            _spawnService = spawnService;
        }

        private void Awake()
        {
            ResolveSpawnPoint();
        }

        private void Start()
        {
            EnsureServices();

            if (_spawnOnStart)
                Spawn();
        }

        public void Spawn()
        {
            EnsureServices();

            if (_spawnService == null)
                return;

            GamePlayerSpawnPoint spawnPoint = ResolveSpawnPoint();
            if (spawnPoint == null)
                return;

            _spawnService.Spawn(spawnPoint);
        }

        public void Respawn()
        {
            EnsureServices();
            _spawnService?.Respawn();
        }

        private GamePlayerSpawnPoint ResolveSpawnPoint()
        {
            if (_spawnPoint != null)
                return _spawnPoint;

            _spawnPoint = GetComponentInChildren<GamePlayerSpawnPoint>(true);
            if (_spawnPoint != null)
                return _spawnPoint;

            _spawnPoint = FindObjectOfType<GamePlayerSpawnPoint>();
            return _spawnPoint;
        }

        private void EnsureServices()
        {
            if (_spawnService != null)
                return;

            GamePlayerContext context = GamePlayerContext.Instance;
            if (context == null)
                context = FindObjectOfType<GamePlayerContext>();

            if (context == null)
                context = gameObject.AddComponent<GamePlayerContext>();

            Construct(context.SpawnService);
        }
    }
}

using Code.Gameplay.PlayerLogic.Behaviours;
using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Services
{
    public class GamePlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private GamePlayerCharacter _prefab;

        public Transform Transform
        {
            get
            {
                if (_spawnPoint != null)
                    return _spawnPoint;

                return transform;
            }
        }

        public GameObject GameObject => Transform.gameObject;
        public Vector3 Position => Transform.position;
        public GamePlayerCharacter Prefab => _prefab;

        private void OnEnable()
        {
            TrySpawn();
        }

        private void Start()
        {
            TrySpawn();
        }

        private void OnDestroy()
        {
            GamePlayerContext context = GamePlayerContext.Instance;
            context?.SpawnService.UnregisterPoint(this);
        }

        private void TrySpawn()
        {
            GamePlayerContext context = GamePlayerContext.Instance;
            if (context == null)
                return;

            IPlayerSpawnService spawnService = context.SpawnService;
            spawnService.RegisterPoint(this);
            spawnService.Spawn(this);
        }
    }
}

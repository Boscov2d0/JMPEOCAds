using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.AI.Services
{
    public class AISpawnController : MonoBehaviour
    {
        [SerializeField] private List<AISpawnPoint> _spawnPoints;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private bool _spawnOnStart;

        private IAISpawnService _spawnService;
        private Coroutine _spawnCoroutine;
        private int _spawnCount;

        public void Construct(IAISpawnService spawnService)
        {
            _spawnService = spawnService;
            _spawnCount = 0;
        }

        public void SuppressAutoSpawn() => _spawnOnStart = false;

        private void Start()
        {
            EnsureServices();

            if (_spawnOnStart)
                Spawn();
        }

        public void RegisterSpawnPoint(AISpawnPoint spawnPoint)
        {
            _spawnPoints.Add(spawnPoint);
        }

        public void UnregisterSpawnPoint(AISpawnPoint spawnPoint)
        {
            _spawnPoints.Remove(spawnPoint);
        }

        public void Spawn()
        {
            EnsureServices();

            if (_spawnService == null)
                return;

            if (_spawnCoroutine != null)
                StopCoroutine(_spawnCoroutine);

            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                yield return new WaitForSeconds(_spawnCount * _spawnDelay);
                _spawnCount++;
                _spawnService.Spawn(spawnPoint);
            }
        }

        public void Despawn()
        {
            EnsureServices();
            _spawnCount = 0;
            _spawnCoroutine = null;
            _spawnService?.DespawnAll();
        }

        private void EnsureServices()
        {
            if (_spawnService != null)
                return;

            GameAIContext context = GameAIContext.Instance;
            if (context == null)
                return;

            Construct(context.SpawnService);
        }
    }
}

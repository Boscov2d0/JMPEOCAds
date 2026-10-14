using System.Collections.Generic;
using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Brains;
using UnityEngine;

namespace Code.Gameplay.AI.Services
{
    public class AISpawnService : IAISpawnService
    {
        private readonly IBotTickService _tickService;
        private readonly List<AICharacter> _characters = new List<AICharacter>();
        private readonly Dictionary<AICharacter, AISpawnPoint> _spawnOrigins = new Dictionary<AICharacter, AISpawnPoint>();

        public AISpawnService(IBotTickService tickService)
        {
            _tickService = tickService;
        }

        public IReadOnlyList<AICharacter> Characters => _characters;

        public void Spawn(AISpawnPoint spawnPoint)
        {
            if (spawnPoint == null || spawnPoint.Prefab == null)
                return;

            GetSpawnPose(spawnPoint, out Vector3 position, out Quaternion rotation);

            AICharacter character = Object.Instantiate(
                spawnPoint.Prefab, position, rotation);

            AIBrain brain = spawnPoint.CreateAIBrain();

            character.Initialize(brain, _characters.Count);
            character.RouteFinished += RestartAtSpawn;
            _spawnOrigins[character] = spawnPoint;
            _characters.Add(character);
            _tickService.Register(character);
        }

        public void TryRespawn(AICharacter aiCharacter)
        {
            if (aiCharacter != null)
                aiCharacter.TryRespawn();
        }

        public void RestartAtSpawn(AICharacter aiCharacter)
        {
            if (aiCharacter == null || !_spawnOrigins.TryGetValue(aiCharacter, out AISpawnPoint spawnPoint))
                return;

            if (spawnPoint == null)
                return;

            AIBrain brain = spawnPoint.CreateAIBrain();

            if (brain == null)
                return;

            GetSpawnPose(spawnPoint, out Vector3 position, out Quaternion rotation);
            Vector3 facing = rotation * Vector3.forward;
            facing.y = 0f;
            aiCharacter.Mover.Teleport(position, facing);
            aiCharacter.Restart(brain);
        }

        public void DespawnAll()
        {
            for (int i = 0; i < _characters.Count; i++)
            {
                AICharacter character = _characters[i];

                if (character == null)
                    continue;

                character.RouteFinished -= RestartAtSpawn;
                _tickService.Unregister(character);
                Object.Destroy(character.gameObject);
            }

            _characters.Clear();
            _spawnOrigins.Clear();
        }

        private static void GetSpawnPose(AISpawnPoint spawnPoint, out Vector3 position, out Quaternion rotation)
        {
            Transform origin = spawnPoint.Transform;
            position = origin.position;
            rotation = origin.rotation;
        }
    }
}

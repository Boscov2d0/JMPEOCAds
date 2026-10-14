using Code.Gameplay.PlayerLogic.Behaviours;
using Source.Scripts.UI.Customization.Data;
using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Services
{
    public class PlayerSpawnService : IPlayerSpawnService
    {
        private GamePlayerSpawnPoint _spawnPoint;
        private GameObject _player;
        private GamePlayerCharacter _character;

        public Transform Player => _player != null ? _player.transform : null;
        public GamePlayerCharacter Character => _character;

        public void RegisterPoint(GamePlayerSpawnPoint spawnPoint)
        {
            if (spawnPoint != null)
                _spawnPoint = spawnPoint;
        }

        public void UnregisterPoint(GamePlayerSpawnPoint spawnPoint)
        {
            if (_spawnPoint == spawnPoint)
                _spawnPoint = null;
        }

        public void Spawn(GamePlayerSpawnPoint spawnPoint)
        {
            RegisterPoint(spawnPoint);

            if (_spawnPoint == null || _spawnPoint.Prefab == null)
                return;

            Transform origin = _spawnPoint.Transform;
            if (origin == null)
                return;

            if (_player != null)
                return;

            _player = Object.Instantiate(
                _spawnPoint.Prefab.gameObject,
                origin.position,
                origin.rotation);
            _character = _player.GetComponent<GamePlayerCharacter>();
            if (_character == null)
                _character = _player.GetComponentInChildren<GamePlayerCharacter>();

            CharacterAppearanceStore.ApplyTo(_player.transform);
        }

        public void Respawn()
        {
            if (_player == null || _spawnPoint == null || _spawnPoint.Transform == null)
                return;

            Transform origin = _spawnPoint.Transform;
            Vector3 facing = origin.forward;
            facing.y = 0f;

            if (_character != null)
            {
                _character.Teleport(origin.position, facing);
                return;
            }

            _player.transform.SetPositionAndRotation(
                origin.position,
                facing.sqrMagnitude > 0.001f
                    ? Quaternion.LookRotation(facing, Vector3.up)
                    : origin.rotation);
        }

        public void Remove() => _spawnPoint = null;
    }
}

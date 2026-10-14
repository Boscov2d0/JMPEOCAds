using Code.Gameplay.PlayerLogic.Input;
using Code.Gameplay.PlayerLogic.Movement;
using UnityEngine;

namespace Code.Gameplay.PlayerLogic.Behaviours
{
    [RequireComponent(typeof(GamePlayerMovement))]
    [RequireComponent(typeof(Rigidbody))]
    public class GamePlayerCharacter : MonoBehaviour
    {
        [SerializeField] private GamePlayerMovement _movement;

        private Rigidbody _rigidbody;

        public GamePlayerMovement Movement => _movement;

        private void Awake()
        {
            if (_movement == null)
                _movement = GetComponent<GamePlayerMovement>();

            _rigidbody = GetComponent<Rigidbody>();
            BindInput();
        }

        private void OnEnable() => BindInput();

        public void Teleport(Vector3 position, Vector3 planarForward)
        {
            if (_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();

            planarForward.y = 0f;
            Quaternion rotation = planarForward.sqrMagnitude > 0.001f
                ? Quaternion.LookRotation(planarForward.normalized, Vector3.up)
                : transform.rotation;

            if (_rigidbody != null)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.position = position;
                _rigidbody.rotation = rotation;
            }

            transform.SetPositionAndRotation(position, rotation);

            if (_movement != null)
                _movement.NotifyTeleported();
        }

        private void BindInput()
        {
            if (_movement == null)
                return;

            GamePlayerContext context = GamePlayerContext.Resolve();
            if (context == null)
                return;

            _movement.Construct(context.InputService);
        }
    }
}

using UnityEngine;

namespace Code.Gameplay.AI.Movement
{
    public class BotKinematicMover
    {
        private const float MIN_FACING_SQR = 0.000001f;
        private const float SPEED_EPSILON = 0.01f;

        // Playworks resolves Animator.StringToHash only after the controller is loaded.
        // A hash cached at startup never matches the Speed parameter, so Run never starts.
        private const string SPEED_PARAMETER = "Speed";
        private const string JUMP_PARAMETER = "Jump";

        private readonly Transform _transform;
        private readonly Animator _animator;
        private readonly float _rotationSpeed;

        private Vector3 _position;
        private float _locomotionSpeed = -1f;

        public BotKinematicMover(Transform transform, Animator animator, float rotationSpeed)
        {
            _transform = transform;
            _animator = animator;
            _rotationSpeed = rotationSpeed;
            _position = transform.position;
        }

        public Vector3 Position => _position;

        public void SetPose([Bridge.Ref] Vector3 position, [Bridge.Ref] Vector3 facing, float deltaTime)
        {
            _position = position;
            _transform.position = position;

            if (facing.sqrMagnitude <= MIN_FACING_SQR)
                return;

            Quaternion target = Quaternion.LookRotation(facing, Vector3.up);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, target, _rotationSpeed * deltaTime);
        }

        public void Teleport([Bridge.Ref] Vector3 position, [Bridge.Ref] Vector3 facing)
        {
            _position = position;
            _transform.position = position;

            if (facing.sqrMagnitude > MIN_FACING_SQR)
                _transform.rotation = Quaternion.LookRotation(facing, Vector3.up);
        }

        public void SetLocomotionSpeed(float speed)
        {
            if (_animator == null || Mathf.Abs(_locomotionSpeed - speed) < SPEED_EPSILON)
                return;

            _locomotionSpeed = speed;
            _animator.SetFloat(SPEED_PARAMETER, speed);
        }

        public void PlayJump()
        {
            if (_animator != null)
                _animator.SetTrigger(JUMP_PARAMETER);
        }
    }
}

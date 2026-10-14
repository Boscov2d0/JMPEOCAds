using UnityEngine;

namespace KinematicCharacterController
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMover : MonoBehaviour
    {
        public Vector3 Velocity { get; private set; }
        public Vector3 AngularVelocity { get; private set; }

        private Rigidbody _rigidbody;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _lastPosition = transform.position;
            _lastRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.deltaTime;
            if (deltaTime <= 0f)
            {
                Velocity = Vector3.zero;
                AngularVelocity = Vector3.zero;
                return;
            }

            var currentPosition = transform.position;
            var currentRotation = transform.rotation;

            Velocity = (currentPosition - _lastPosition) / deltaTime;

            var deltaRotation = currentRotation * Quaternion.Inverse(_lastRotation);
            deltaRotation.ToAngleAxis(out var angle, out var axis);
            if (angle > 180f)
                angle -= 360f;

            AngularVelocity = axis.normalized * (angle * Mathf.Deg2Rad / deltaTime);

            _lastPosition = currentPosition;
            _lastRotation = currentRotation;
        }
    }
}

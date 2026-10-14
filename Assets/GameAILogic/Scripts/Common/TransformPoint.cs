using UnityEngine;

namespace Code.Gameplay.Common.Behaviours
{
    public class TransformPoint : MonoBehaviour, ITransformPoint
    {
        private Transform _transform;

        Vector3 ITransformPoint.Position => _transform.position;

        Transform ITransformPoint.Transform => _transform;

        public GameObject GameObject => _transform.gameObject;

        private void Awake() => _transform = transform;
        
        public Vector3 Position() => _transform.position;

        public Transform Transform() => _transform;
    }
}
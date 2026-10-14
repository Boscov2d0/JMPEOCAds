using System;
using System.Collections.Generic;
using Code.Gameplay.AI.Commands;
using UnityEngine;

namespace Code.Gameplay.AI.Waypoints
{
    public class RouteWaypoint : AIWaypoint
    {
        [SerializeField] private List<RouteCommandPoint> _points = new List<RouteCommandPoint>();
        [SerializeField, Min(1f)] private float _maxDrawDistance = 100f;

        public float MaxDrawDistance => _maxDrawDistance;
        public List<RouteCommandPoint> Points => _points;
        public int PointCount => _points.Count;

        private void Awake()
        {
            foreach (RouteCommandPoint point in _points)
                point.SetupWorldPosition(transform.TransformPoint(point.LocalPosition));
        }

        public void AddPoints(List<RouteCommandPoint> points)
        {
            _points.AddRange(points);
        }

        public Vector3 GetWorldPosition(int pointIndex, int variant = 0)
        {
            if (pointIndex < 0 || pointIndex >= _points.Count)
                return transform.position;

            return transform.TransformPoint(_points[pointIndex].GetLocalPosition(variant));
        }

        public int FindNearestPointIndex([Bridge.Ref] Vector3 worldPosition)
        {
            int bestIndex = 0;
            float bestSqrDistance = float.MaxValue;

            for (int i = 0; i < _points.Count; i++)
            {
                float sqrDistance = (GetWorldPosition(i) - worldPosition).sqrMagnitude;

                if (sqrDistance >= bestSqrDistance)
                    continue;

                bestSqrDistance = sqrDistance;
                bestIndex = i;
            }

            return bestIndex;
        }

        public override IAICommand CreateCommand() => new RouteCommand(this);
    }

    [Serializable]
    public class RouteCommandPoint
    {
        [NonSerialized] private Vector3 _worldPosition;
        [SerializeField] private Vector3 _localPosition;
        [SerializeField] private List<Vector3> _variantLocalPositions = new List<Vector3>();
        [SerializeField] private bool _allowNoise = true;

        public Vector3 WorldPosition => _worldPosition;
        public Vector3 LocalPosition => _localPosition;
        public List<Vector3> VariantLocalPositions => _variantLocalPositions;
        public bool AllowNoise => _allowNoise;

        public int VariantCount => _variantLocalPositions.Count + 1;

        public RouteCommandPoint(Vector3 localPosition, bool allowNoise = true)
        {
            _localPosition = localPosition;
            _allowNoise = allowNoise;
        }

        public Vector3 GetLocalPosition(int variant) =>
            variant > 0 && variant <= _variantLocalPositions.Count
                ? _variantLocalPositions[variant - 1]
                : _localPosition;

        public void SetupWorldPosition(Vector3 worldPosition) => _worldPosition = worldPosition;
        public void AddVariant(Vector3 localPosition) => _variantLocalPositions.Add(localPosition);

        public void RemoveVariant(int index)
        {
            if (index >= 0 && index < _variantLocalPositions.Count)
                _variantLocalPositions.RemoveAt(index);
        }

        public void ToggleNoise() => _allowNoise = !_allowNoise;
    }
}

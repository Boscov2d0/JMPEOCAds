using UnityEngine;

namespace Code.Gameplay.AI.Data
{
    public class BotRoute
    {
        private const float MIN_SEGMENT_LENGTH = 0.0001f;

        private readonly Vector3[] _positions;
        private readonly RoutePointFlags[] _flags;
        private readonly float[] _distances;
        private readonly float[] _segmentLengths;
        private readonly Vector3[] _flatDirections;

        public BotRoute(Vector3[] positions, RoutePointFlags[] flags)
        {
            _positions = positions;
            _flags = flags;

            int segmentCount = Mathf.Max(positions.Length - 1, 0);

            _distances = new float[positions.Length];
            _segmentLengths = new float[segmentCount];
            _flatDirections = new Vector3[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                Vector3 delta = positions[i + 1] - positions[i];
                float length = delta.magnitude;

                _segmentLengths[i] = length;
                _distances[i + 1] = _distances[i] + length;

                Vector3 flat = new Vector3(delta.x, 0f, delta.z);
                _flatDirections[i] = flat.sqrMagnitude > MIN_SEGMENT_LENGTH ? flat.normalized : Vector3.forward;
            }
        }

        public int PointCount => _positions.Length;
        public int SegmentCount => _segmentLengths.Length;
        public bool IsValid => _positions.Length >= 2;
        public float Length => _positions.Length > 0 ? _distances[_positions.Length - 1] : 0f;

        public Vector3 GetPosition(int index) => _positions[index];
        public RoutePointFlags GetFlags(int index) => _flags[index];
        public float GetDistanceAt(int index) => _distances[index];

        public Vector3 GetFlatDirection(int segmentIndex) =>
            _flatDirections[Mathf.Clamp(segmentIndex, 0, SegmentCount - 1)];

        /// <summary>
        /// Walks the polyline from the previous segment instead of searching it,
        /// so a tick costs a couple of comparisons regardless of route length.
        /// </summary>
        public void Evaluate(float distance, ref int segmentIndex, out Vector3 position, out float segmentT)
        {
            int lastSegment = SegmentCount - 1;

            if (lastSegment < 0)
            {
                segmentIndex = 0;
                segmentT = 0f;
                position = _positions.Length > 0 ? _positions[0] : Vector3.zero;
                return;
            }

            segmentIndex = Mathf.Clamp(segmentIndex, 0, lastSegment);

            while (segmentIndex < lastSegment && distance >= _distances[segmentIndex + 1])
                segmentIndex++;

            while (segmentIndex > 0 && distance < _distances[segmentIndex])
                segmentIndex--;

            float segmentLength = _segmentLengths[segmentIndex];

            segmentT = segmentLength > MIN_SEGMENT_LENGTH
                ? Mathf.Clamp01((distance - _distances[segmentIndex]) / segmentLength)
                : 1f;

            position = Vector3.LerpUnclamped(_positions[segmentIndex], _positions[segmentIndex + 1], segmentT);
        }

        /// <summary>
        /// Projects a world position onto the polyline. Used when a bot switches routes mid-run,
        /// so it can join the new route where it already stands instead of snapping to its start.
        /// </summary>
        public float FindNearestDistance([Bridge.Ref] Vector3 position)
        {
            float bestDistance = 0f;
            float bestSqrDistance = float.MaxValue;

            for (int i = 0; i < SegmentCount; i++)
            {
                Vector3 from = _positions[i];
                Vector3 delta = _positions[i + 1] - from;
                float lengthSqr = delta.sqrMagnitude;

                float t = lengthSqr > MIN_SEGMENT_LENGTH
                    ? Mathf.Clamp01(Vector3.Dot(position - from, delta) / lengthSqr)
                    : 0f;

                float sqrDistance = (position - (from + delta * t)).sqrMagnitude;

                if (sqrDistance >= bestSqrDistance)
                    continue;

                bestSqrDistance = sqrDistance;
                bestDistance = _distances[i] + _segmentLengths[i] * t;
            }

            return bestDistance;
        }
    }
}

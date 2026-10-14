using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.AI.Services
{
    public class BotFieldMap : MonoBehaviour
    {
        private const float DEFAULT_CELL_SIZE = 0.5f;
        private const float DEFAULT_AGENT_RADIUS = 0.4f;
        private const float TILE_EXPAND = 0.35f;
        private const int DEFAULT_SAMPLE_ATTEMPTS = 48;

        [SerializeField] private Transform _fieldRoot;
        [SerializeField] private string[] _obstaclePrefixes = { "SM_fence1", "SM_box" };
        [SerializeField, Min(0.1f)] private float _cellSize = DEFAULT_CELL_SIZE;
        [SerializeField, Min(0.1f)] private float _agentRadius = DEFAULT_AGENT_RADIUS;

        private bool[] _onField;
        private bool[] _blocked;
        private Bounds[] _obstacles;
        private int _width;
        private int _height;
        private float _originX;
        private float _originZ;
        private float _groundY;
        private bool _isBuilt;

        public bool IsReady
        {
            get
            {
                EnsureBuilt();
                return _isBuilt && _onField != null && _onField.Length > 0;
            }
        }

        public float GroundY
        {
            get
            {
                EnsureBuilt();
                return _groundY;
            }
        }

        private void Awake() => EnsureBuilt();

        public bool TryGetRandomWalkablePoint(Vector3 from, float minDistance, float maxDistance, out Vector3 point)
        {
            EnsureBuilt();
            point = from;

            if (!IsReady)
                return false;

            float minSqr = minDistance * minDistance;
            float maxSqr = maxDistance * maxDistance;

            for (int i = 0; i < DEFAULT_SAMPLE_ATTEMPTS; i++)
            {
                if (!TryGetRandomCellPoint(out Vector3 candidate))
                    continue;

                float sqr = HorizontalSqr(from, candidate);
                if (sqr < minSqr || sqr > maxSqr)
                    continue;

                if (!HasLineOfSight(from, candidate))
                    continue;

                point = candidate;
                return true;
            }

            for (int i = 0; i < DEFAULT_SAMPLE_ATTEMPTS; i++)
            {
                if (!TryGetRandomCellPoint(out Vector3 candidate))
                    continue;

                if (HorizontalSqr(from, candidate) < 0.25f)
                    continue;

                if (!HasLineOfSight(from, candidate))
                    continue;

                point = candidate;
                return true;
            }

            return false;
        }

        public bool TryGetRandomWalkablePoint(out Vector3 point)
        {
            EnsureBuilt();
            return TryGetRandomCellPoint(out point);
        }

        public bool TryGetNearestWalkablePoint(Vector3 from, out Vector3 point)
        {
            EnsureBuilt();
            point = from;

            if (!IsReady)
                return false;

            ToCell(from, out int startX, out int startZ);
            if (IsWalkable(startX, startZ))
            {
                point = CellCenter(startX, startZ);
                return true;
            }

            int maxRadius = Mathf.Max(_width, _height);
            for (int radius = 1; radius <= maxRadius; radius++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int x0 = startX + dx;
                    if (IsWalkable(x0, startZ - radius))
                    {
                        point = CellCenter(x0, startZ - radius);
                        return true;
                    }

                    if (IsWalkable(x0, startZ + radius))
                    {
                        point = CellCenter(x0, startZ + radius);
                        return true;
                    }
                }

                for (int dz = -radius + 1; dz <= radius - 1; dz++)
                {
                    int z0 = startZ + dz;
                    if (IsWalkable(startX - radius, z0))
                    {
                        point = CellCenter(startX - radius, z0);
                        return true;
                    }

                    if (IsWalkable(startX + radius, z0))
                    {
                        point = CellCenter(startX + radius, z0);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool HasLineOfSight(Vector3 from, Vector3 to)
        {
            EnsureBuilt();
            if (!IsReady)
                return false;

            if (_obstacles == null)
                return true;

            for (int i = 0; i < _obstacles.Length; i++)
            {
                if (SegmentHitsBoundsXz(from, to, _obstacles[i]))
                    return false;
            }

            return true;
        }

        public bool Contains(Vector3 position)
        {
            EnsureBuilt();
            if (!IsReady)
                return false;

            ToCell(position, out int x, out int z);
            return IsWalkable(x, z);
        }

        private void EnsureBuilt()
        {
            if (_isBuilt)
                return;

            Transform root = _fieldRoot != null ? _fieldRoot : transform;
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>(false);
            if (renderers == null || renderers.Length == 0)
                return;

            Bounds fieldBounds = renderers[0].bounds;
            var tileBounds = new List<Bounds>(renderers.Length);

            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null || !renderer.gameObject.activeInHierarchy)
                    continue;

                Bounds bounds = renderer.bounds;
                tileBounds.Add(bounds);
                fieldBounds.Encapsulate(bounds);
            }

            if (tileBounds.Count == 0)
                return;

            _groundY = fieldBounds.max.y;
            _originX = fieldBounds.min.x;
            _originZ = fieldBounds.min.z;
            _cellSize = Mathf.Max(0.1f, _cellSize);
            _width = Mathf.Max(1, Mathf.CeilToInt(fieldBounds.size.x / _cellSize) + 1);
            _height = Mathf.Max(1, Mathf.CeilToInt(fieldBounds.size.z / _cellSize) + 1);
            _onField = new bool[_width * _height];
            _blocked = new bool[_width * _height];

            for (int i = 0; i < tileBounds.Count; i++)
                MarkOnField(tileBounds[i], -TILE_EXPAND);

            CollectObstacles(fieldBounds);
            _isBuilt = true;
        }

        private void CollectObstacles(Bounds fieldBounds)
        {
            BoxCollider[] colliders = FindObjectsOfType<BoxCollider>();
            if (colliders == null)
                return;

            var obstacles = new List<Bounds>();

            for (int i = 0; i < colliders.Length; i++)
            {
                BoxCollider box = colliders[i];
                if (box == null || !MatchesObstacle(box.gameObject.name))
                    continue;

                Bounds obstacle = box.bounds;
                if (!OverlapsXz(fieldBounds, obstacle))
                    continue;

                obstacle.Expand(new Vector3(_agentRadius, 0f, _agentRadius));
                obstacles.Add(obstacle);
                MarkBlocked(obstacle);
            }

            _obstacles = obstacles.ToArray();
        }

        private bool MatchesObstacle(string objectName)
        {
            if (string.IsNullOrEmpty(objectName) || _obstaclePrefixes == null)
                return false;

            for (int i = 0; i < _obstaclePrefixes.Length; i++)
            {
                string prefix = _obstaclePrefixes[i];
                if (string.IsNullOrEmpty(prefix))
                    continue;

                if (objectName == prefix || objectName.StartsWith(prefix + " ") || objectName.StartsWith(prefix + "("))
                    return true;
            }

            return false;
        }

        private void MarkOnField(Bounds bounds, float inset)
        {
            GetCellRange(bounds, inset, out int x0, out int x1, out int z0, out int z1);

            for (int z = z0; z <= z1; z++)
            {
                int row = z * _width;
                for (int x = x0; x <= x1; x++)
                    _onField[row + x] = true;
            }
        }

        private void MarkBlocked(Bounds bounds)
        {
            GetCellRange(bounds, 0f, out int x0, out int x1, out int z0, out int z1);

            for (int z = z0; z <= z1; z++)
            {
                int row = z * _width;
                for (int x = x0; x <= x1; x++)
                    _blocked[row + x] = true;
            }
        }

        private void GetCellRange(Bounds bounds, float inset, out int x0, out int x1, out int z0, out int z1)
        {
            float minX = bounds.min.x + inset;
            float maxX = bounds.max.x - inset;
            float minZ = bounds.min.z + inset;
            float maxZ = bounds.max.z - inset;

            if (minX > maxX)
            {
                minX = bounds.center.x;
                maxX = bounds.center.x;
            }

            if (minZ > maxZ)
            {
                minZ = bounds.center.z;
                maxZ = bounds.center.z;
            }

            x0 = Mathf.Clamp(Mathf.FloorToInt((minX - _originX) / _cellSize), 0, _width - 1);
            x1 = Mathf.Clamp(Mathf.FloorToInt((maxX - _originX) / _cellSize), 0, _width - 1);
            z0 = Mathf.Clamp(Mathf.FloorToInt((minZ - _originZ) / _cellSize), 0, _height - 1);
            z1 = Mathf.Clamp(Mathf.FloorToInt((maxZ - _originZ) / _cellSize), 0, _height - 1);
        }

        private bool TryGetRandomCellPoint(out Vector3 point)
        {
            point = Vector3.zero;
            if (_onField == null || _onField.Length == 0)
                return false;

            int cellCount = _onField.Length;
            int start = Random.Range(0, cellCount);

            for (int i = 0; i < cellCount; i++)
            {
                int index = start + i;
                if (index >= cellCount)
                    index -= cellCount;

                int z = index / _width;
                int x = index - z * _width;
                if (!IsWalkable(x, z))
                    continue;

                point = CellCenter(x, z);
                return true;
            }

            return false;
        }

        private bool IsWalkable(int x, int z)
        {
            if (x < 0 || z < 0 || x >= _width || z >= _height)
                return false;

            int index = z * _width + x;
            return _onField[index] && !_blocked[index];
        }

        private void ToCell(Vector3 position, out int x, out int z)
        {
            x = Mathf.Clamp(Mathf.FloorToInt((position.x - _originX) / _cellSize), 0, _width - 1);
            z = Mathf.Clamp(Mathf.FloorToInt((position.z - _originZ) / _cellSize), 0, _height - 1);
        }

        private Vector3 CellCenter(int x, int z)
        {
            return new Vector3(
                _originX + (x + 0.5f) * _cellSize,
                _groundY,
                _originZ + (z + 0.5f) * _cellSize);
        }

        private static bool SegmentHitsBoundsXz(Vector3 from, Vector3 to, Bounds bounds)
        {
            float x1 = from.x;
            float z1 = from.z;
            float x2 = to.x;
            float z2 = to.z;
            float minX = bounds.min.x;
            float maxX = bounds.max.x;
            float minZ = bounds.min.z;
            float maxZ = bounds.max.z;

            float dx = x2 - x1;
            float dz = z2 - z1;
            float tMin = 0f;
            float tMax = 1f;

            if (!ClipAxis(x1, dx, minX, maxX, ref tMin, ref tMax))
                return false;

            if (!ClipAxis(z1, dz, minZ, maxZ, ref tMin, ref tMax))
                return false;

            return tMax >= tMin;
        }

        private static bool ClipAxis(float start, float delta, float min, float max, ref float tMin, ref float tMax)
        {
            if (Mathf.Abs(delta) < 0.0001f)
                return start >= min && start <= max;

            float t0 = (min - start) / delta;
            float t1 = (max - start) / delta;
            if (t0 > t1)
            {
                float swap = t0;
                t0 = t1;
                t1 = swap;
            }

            tMin = Mathf.Max(tMin, t0);
            tMax = Mathf.Min(tMax, t1);
            return tMax >= tMin;
        }

        private static float HorizontalSqr(Vector3 a, Vector3 b)
        {
            float dx = a.x - b.x;
            float dz = a.z - b.z;
            return dx * dx + dz * dz;
        }

        private static bool OverlapsXz(Bounds a, Bounds b)
        {
            return a.min.x <= b.max.x && a.max.x >= b.min.x &&
                   a.min.z <= b.max.z && a.max.z >= b.min.z;
        }
    }
}

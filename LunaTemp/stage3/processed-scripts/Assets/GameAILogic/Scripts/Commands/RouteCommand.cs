using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Data;
using Code.Gameplay.AI.Waypoints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Gameplay.AI.Commands
{
    public class RouteCommand : IAICommand, IAIRespawnCommand
    {
        private const float MIN_FACING_SQR = 0.000001f;
        private const float ARRIVAL_RADIUS = 0.35f;

        private readonly RouteWaypoint _waypoint;

        private BotRoute _leg;
        private int _currentIndex = -1;
        private int _targetIndex;
        private int _segmentIndex;
        private float _distance;
        private Vector3 _respawnPosition;
        private Vector3 _respawnFacing = Vector3.forward;
        private bool _isIdle;

        public RouteCommand(RouteWaypoint waypoint)
        {
            _waypoint = waypoint;
        }

        public void Enter(AICharacter character)
        {
            _leg = null;
            _isIdle = false;
            _distance = 0f;
            _segmentIndex = 0;

            if (_waypoint == null || _waypoint.PointCount == 0)
            {
                Idle(character);
                return;
            }

            _currentIndex = _waypoint.FindNearestPointIndex(character.Mover.Position);
            StoreRespawn(_waypoint.GetWorldPosition(_currentIndex), _waypoint.transform.forward);
            StartNextLeg(character);
        }

        public AICommandStatus Tick(AICharacter character, float deltaTime)
        {
            if (_isIdle)
            {
                character.Mover.SetLocomotionSpeed(0f);
                return AICommandStatus.Running;
            }

            if (_leg == null || !_leg.IsValid)
            {
                StartNextLeg(character);
                return AICommandStatus.Running;
            }

            _distance += character.MoveSpeed * deltaTime;
            bool arrived = _distance >= _leg.Length;

            if (arrived)
                _distance = _leg.Length;

            _leg.Evaluate(_distance, ref _segmentIndex, out Vector3 position, out float segmentT);
            ApplyPose(character, position, segmentT, deltaTime);

            if (!arrived)
                return AICommandStatus.Running;

            _currentIndex = _targetIndex;
            StoreRespawn(position, _leg.GetFlatDirection(0));
            StartNextLeg(character);
            return AICommandStatus.Running;
        }

        public bool TryRespawn(AICharacter character)
        {
            if (_waypoint == null || _waypoint.PointCount == 0)
                return false;

            character.Mover.Teleport(_respawnPosition, _respawnFacing);
            StartNextLeg(character);
            return true;
        }

        private void StartNextLeg(AICharacter character)
        {
            int count = _waypoint.PointCount;

            if (count <= 1)
            {
                MoveOrIdle(character, 0);
                return;
            }

            Vector3 from = character.Mover.Position;

            for (int attempt = 0; attempt < count; attempt++)
            {
                _targetIndex = PickNextIndex();
                RouteCommandPoint target = _waypoint.Points[_targetIndex];
                int variant = Random.Range(0, target.VariantCount);
                Vector3 to = _waypoint.GetWorldPosition(_targetIndex, variant);

                if ((from - to).sqrMagnitude <= ARRIVAL_RADIUS * ARRIVAL_RADIUS)
                {
                    _currentIndex = _targetIndex;
                    continue;
                }

                _leg = BuildLeg(from, to, target);
                _distance = 0f;
                _segmentIndex = 0;
                _isIdle = false;
                return;
            }

            Idle(character);
        }

        private void MoveOrIdle(AICharacter character, int pointIndex)
        {
            Vector3 to = _waypoint.GetWorldPosition(pointIndex);
            Vector3 from = character.Mover.Position;

            if ((from - to).sqrMagnitude <= ARRIVAL_RADIUS * ARRIVAL_RADIUS)
            {
                Idle(character);
                return;
            }

            _targetIndex = pointIndex;
            _leg = BuildLeg(from, to, _waypoint.Points[pointIndex]);
            _distance = 0f;
            _segmentIndex = 0;
            _isIdle = false;
        }

        private int PickNextIndex()
        {
            int count = _waypoint.PointCount;
            int next = Random.Range(0, count - 1);

            if (next >= _currentIndex)
                next++;

            return next;
        }

        private void Idle(AICharacter character)
        {
            _isIdle = true;
            _leg = null;
            character.Mover.SetLocomotionSpeed(0f);
        }

        private void StoreRespawn(Vector3 position, Vector3 facing)
        {
            _respawnPosition = position;
            facing.y = 0f;
            _respawnFacing = facing.sqrMagnitude > MIN_FACING_SQR ? facing : Vector3.forward;
        }

        private void ApplyPose(AICharacter character, Vector3 position, float segmentT, float deltaTime)
        {
            RoutePointFlags segmentFlags = _leg.GetFlags(_segmentIndex);
            Vector3 direction = _leg.GetFlatDirection(_segmentIndex);

            if ((segmentFlags & RoutePointFlags.Jump) != 0)
                position.y += character.JumpArcHeight * segmentT * (1f - segmentT);

            if ((segmentFlags & RoutePointFlags.Noise) != 0)
                position += SideOffset(character, direction, segmentT);

            Vector3 delta = position - character.Mover.Position;
            delta.y = 0f;

            Vector3 facing = delta.sqrMagnitude > MIN_FACING_SQR ? delta : direction;

            character.Mover.SetPose(position, facing, deltaTime);
            character.Mover.SetLocomotionSpeed(1f);
        }

        private static Vector3 SideOffset(AICharacter character, [Bridge.Ref] Vector3 direction, float segmentT)
        {
            float wave = Mathf.Sin(Time.time * character.NoiseFrequency + character.NoisePhase) *
                         character.NoiseAmplitude;
            float fade = Mathf.Sin(Mathf.PI * segmentT);
            Vector3 side = new Vector3(-direction.z, 0f, direction.x);

            return side * ((character.LaneOffset + wave) * fade);
        }

        private static BotRoute BuildLeg([Bridge.Ref] Vector3 from, [Bridge.Ref] Vector3 to, RouteCommandPoint target)
        {
            var positions = new[] { from, to };
            var flags = new[] { ToFlags(target), RoutePointFlags.None };
            return new BotRoute(positions, flags);
        }

        private static RoutePointFlags ToFlags(RouteCommandPoint point)
        {
            RoutePointFlags flags = RoutePointFlags.None;

            if (point.AllowNoise)
                flags |= RoutePointFlags.Noise;

            return flags;
        }
    }
}

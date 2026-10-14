using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Services;
using UnityEngine;

namespace Code.Gameplay.AI.Commands
{
    public class FieldPatrolCommand : IAICommand, IAIRespawnCommand
    {
        private const float MIN_FACING_SQR = 0.000001f;
        private const float ARRIVAL_RADIUS = 0.55f;
        private const float MIN_WANDER_DISTANCE = 2f;
        private const float MAX_WANDER_DISTANCE = 18f;
        private const float REPATH_COOLDOWN = 0.35f;

        private readonly BotFieldMap _fieldMap;

        private Vector3 _target;
        private Vector3 _respawnPosition;
        private Vector3 _respawnFacing = Vector3.forward;
        private float _repathTimer;
        private bool _hasTarget;
        private bool _isJoining;

        public FieldPatrolCommand(BotFieldMap fieldMap)
        {
            _fieldMap = fieldMap;
        }

        public void Enter(AICharacter character)
        {
            _repathTimer = 0f;
            _isJoining = true;

            Vector3 from = character.Mover.Position;
            StoreRespawn(from, character.transform.forward);

            if (_fieldMap != null && _fieldMap.Contains(from))
            {
                _isJoining = false;
                PickNewTarget(from);
                return;
            }

            PickJoinTarget(from);
        }

        public AICommandStatus Tick(AICharacter character, float deltaTime)
        {
            if (_fieldMap == null || !_fieldMap.IsReady)
            {
                character.Mover.SetLocomotionSpeed(0f);
                return AICommandStatus.Running;
            }

            if (_repathTimer > 0f)
                _repathTimer -= deltaTime;

            Vector3 from = character.Mover.Position;

            if (_isJoining)
            {
                if (!_hasTarget || HasArrived(from))
                {
                    if (_hasTarget && HasArrived(from))
                    {
                        _isJoining = false;
                        PickNewTarget(from);
                    }
                    else
                    {
                        PickJoinTarget(from);
                    }
                }
            }
            else if (!_hasTarget || HasArrived(from) || !IsTargetValid(from))
            {
                PickNewTarget(from);
            }

            if (!_hasTarget)
            {
                character.Mover.SetLocomotionSpeed(0f);
                return AICommandStatus.Running;
            }

            from = character.Mover.Position;
            Vector3 next = MoveTowards(from, character.MoveSpeed * deltaTime);
            Vector3 facing = next - from;
            facing.y = 0f;
            if (facing.sqrMagnitude <= MIN_FACING_SQR)
                facing = _target - from;

            character.Mover.SetPose(next, facing, deltaTime);
            character.Mover.SetLocomotionSpeed(1f);
            return AICommandStatus.Running;
        }

        public bool TryRespawn(AICharacter character)
        {
            if (_fieldMap == null || !_fieldMap.IsReady)
                return false;

            character.Mover.Teleport(_respawnPosition, _respawnFacing);
            _isJoining = !_fieldMap.Contains(_respawnPosition);

            if (_isJoining)
                PickJoinTarget(_respawnPosition);
            else
                PickNewTarget(_respawnPosition);

            return true;
        }

        private void PickJoinTarget([Bridge.Ref] Vector3 from)
        {
            _hasTarget = _fieldMap.TryGetNearestWalkablePoint(from, out _target);
            _repathTimer = REPATH_COOLDOWN;
        }

        private void PickNewTarget([Bridge.Ref] Vector3 from)
        {
            _hasTarget = _fieldMap.TryGetRandomWalkablePoint(
                from, MIN_WANDER_DISTANCE, MAX_WANDER_DISTANCE, out _target);

            if (!_hasTarget)
                _hasTarget = _fieldMap.TryGetRandomWalkablePoint(out _target);

            _repathTimer = REPATH_COOLDOWN;
        }

        private bool HasArrived([Bridge.Ref] Vector3 from)
        {
            return HorizontalSqr(from, _target) <= ARRIVAL_RADIUS * ARRIVAL_RADIUS;
        }

        private bool IsTargetValid([Bridge.Ref] Vector3 from)
        {
            if (_repathTimer > 0f)
                return true;

            return _fieldMap.Contains(_target) && _fieldMap.HasLineOfSight(from, _target);
        }

        private Vector3 MoveTowards(Vector3 from, float step)
        {
            Vector3 delta = _target - from;
            delta.y = 0f;
            float distance = delta.magnitude;
            Vector3 next = distance <= step || distance <= 0.0001f
                ? _target
                : from + delta * (step / distance);

            next.y = Mathf.Lerp(from.y, _fieldMap.GroundY, 0.35f);

            if (_isJoining || _fieldMap.HasLineOfSight(from, next))
                return next;

            _hasTarget = false;
            return from;
        }

        private void StoreRespawn(Vector3 position, Vector3 facing)
        {
            _respawnPosition = position;
            facing.y = 0f;
            _respawnFacing = facing.sqrMagnitude > MIN_FACING_SQR ? facing.normalized : Vector3.forward;
        }

        private static float HorizontalSqr([Bridge.Ref] Vector3 a, [Bridge.Ref] Vector3 b)
        {
            float dx = a.x - b.x;
            float dz = a.z - b.z;
            return dx * dx + dz * dz;
        }
    }
}

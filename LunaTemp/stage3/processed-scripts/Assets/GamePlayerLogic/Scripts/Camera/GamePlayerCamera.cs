using Code.Gameplay.PlayerLogic.Behaviours;
using Code.Gameplay.PlayerLogic.Input;
using Code.Gameplay.PlayerLogic.Movement;
using Code.Gameplay.PlayerLogic.Services;
using UnityEngine;

namespace Code.Gameplay.PlayerLogic.CameraControl
{
    [DefaultExecutionOrder(32000)]
    public class GamePlayerCamera : MonoBehaviour
    {
        private const float DEFAULT_YAW_SENSITIVITY = 180f;
        private const float DEFAULT_PITCH_SENSITIVITY = 180f;
        private const float DEFAULT_MIN_PITCH = 8f;
        private const float DEFAULT_MAX_PITCH = 70f;
        private const float POLE_LIMIT = 85f;
        private const float OCCLUSION_EPSILON = 0.05f;

        [SerializeField] private Vector3 _offset = new Vector3(0f, 5f, -8f);
        [SerializeField] private Vector3 _lookAtOffset = new Vector3(0f, 1.5f, 0f);
        [Tooltip("Shifts the rest camera arm before orbit. X tilts height, Y turns yaw.")]
        [SerializeField] private Vector3 _angles = Vector3.zero;
        [SerializeField] private float _yawSensitivity = DEFAULT_YAW_SENSITIVITY;
        [SerializeField] private float _pitchSensitivity = DEFAULT_PITCH_SENSITIVITY;
        [SerializeField] private float _minPitch = DEFAULT_MIN_PITCH;
        [SerializeField] private float _maxPitch = DEFAULT_MAX_PITCH;
        [SerializeField] private bool _invertY;
        [Tooltip("How far the camera body stays from walls. The view also pulls in until the character's head and torso are actually visible.")]
        [SerializeField] private float _collisionRadius = 0.28f;
        [Tooltip("How fast the camera returns to the rest arm after the obstacle is gone.")]
        [SerializeField] private float _returnSmoothTime = 0.18f;
        [Tooltip("Added to the camera field of view while the arm is fully pulled in. Zero keeps the rest lens.")]
        [SerializeField] private float _occludedFieldOfViewBoost = 14f;
        [Tooltip("Near clip used while the arm is fully pulled in, so the character is not sliced when the camera is close.")]
        [SerializeField] private float _occludedNearClip = 0.05f;
        [SerializeField] private LayerMask _collisionMask = ~((1 << 2));

        private IPlayerSpawnService _spawnService;
        private IPlayerLookInputService _lookInputService;
        private Transform _target;
        private float _lookYaw;
        private float _lookPitch;
        private Camera _camera;
        private bool _cameraDefaultsReady;
        private float _restFieldOfView = 60f;
        private float _restNearClip = 0.3f;
        private bool _armReady;
        private float _armRadius;
        private float _radiusVelocity;
        private readonly RaycastHit[] _rayHits = new RaycastHit[32];
        private readonly Collider[] _overlapHits = new Collider[16];

        private int CollisionMask
        {
            get
            {
                int mask = _collisionMask.value;
                return mask == 0 ? Physics.DefaultRaycastLayers : mask;
            }
        }

        public void Construct(IPlayerSpawnService spawnService, IPlayerLookInputService lookInputService)
        {
            _spawnService = spawnService;
            _lookInputService = lookInputService;
        }

        private void OnEnable()
        {
            EnsureServices();
            GamePlayerContext context = GamePlayerContext.Instance;
            context?.RegisterCamera(transform);
        }

        private void OnDisable()
        {
            RestoreCameraLens();
            _armReady = false;
            _radiusVelocity = 0f;

            GamePlayerContext context = GamePlayerContext.Instance;
            context?.UnregisterCamera(transform);
        }

        private void LateUpdate()
        {
            EnsureServices();

            Transform previousTarget = _target;
            Transform target = ResolveTarget();
            if (target != previousTarget)
                _armReady = false;

            if (target == null)
            {
                _lookYaw = 0f;
                _lookPitch = 0f;
                _armReady = false;
                RestoreCameraLens();
                return;
            }

            GetRestArm(out float radius, out float restElevation, out float restYaw);
            ApplyLook(restElevation);

            float elevation = ClampElevation(restElevation + _lookPitch);
            float yaw = restYaw + _lookYaw;
            Vector3 lookPoint = target.position + _lookAtOffset;
            float armRadius = ResolveArmRadius(lookPoint, radius, elevation, yaw);
            Vector3 desiredPosition = lookPoint + ArmFromSpherical(armRadius, elevation, yaw);
            ApplyLens(armRadius, radius);
            Vector3 toLookPoint = lookPoint - desiredPosition;

            if (toLookPoint.sqrMagnitude < 0.0001f)
                toLookPoint = Vector3.forward;

            transform.SetPositionAndRotation(
                desiredPosition,
                Quaternion.LookRotation(toLookPoint, Vector3.up));
        }

        private float ResolveArmRadius([Bridge.Ref] Vector3 lookPoint, float radius, float elevation, float yaw)
        {
            float clearRadius = ClearArmDistance(lookPoint, ArmFromSpherical(radius, elevation, yaw));

            if (_armReady == false || clearRadius < _armRadius - 0.01f)
            {
                _armRadius = clearRadius;
                _radiusVelocity = 0f;
                _armReady = true;
                return _armRadius;
            }

            float smoothTime = Mathf.Max(_returnSmoothTime, 0.01f);
            float relaxed = Mathf.SmoothDamp(_armRadius, clearRadius, ref _radiusVelocity, smoothTime);
            _armRadius = Mathf.Min(relaxed, clearRadius);
            if (_armRadius < relaxed - 0.0001f)
                _radiusVelocity = 0f;

            return _armRadius;
        }

        private float ClearArmDistance([Bridge.Ref] Vector3 lookPoint, [Bridge.Ref] Vector3 arm)
        {
            float distance = arm.magnitude;
            if (distance <= OCCLUSION_EPSILON)
                return OCCLUSION_EPSILON;

            Vector3 direction = arm / distance;
            if (ArmIsBlocked(lookPoint, direction, distance) == false)
                return distance;

            float clear = OCCLUSION_EPSILON;
            float blocked = distance;
            for (int i = 0; i < 8; i++)
            {
                float mid = (clear + blocked) * 0.5f;
                if (ArmIsBlocked(lookPoint, direction, mid))
                    blocked = mid;
                else
                    clear = mid;
            }

            float resolved = Mathf.Max(clear * 0.78f, OCCLUSION_EPSILON);
            if (ArmIsBlocked(lookPoint, direction, resolved) == false)
                return resolved;

            float step = resolved;
            for (int i = 0; i < 4; i++)
            {
                step = Mathf.Max(step * 0.5f, OCCLUSION_EPSILON);
                if (ArmIsBlocked(lookPoint, direction, step) == false)
                    return step;
            }

            return OCCLUSION_EPSILON;
        }

        private bool ArmIsBlocked([Bridge.Ref] Vector3 lookPoint, [Bridge.Ref] Vector3 direction, float distance)
        {
            Vector3 cameraPosition = lookPoint + direction * distance;
            float radius = Mathf.Max(_collisionRadius, 0.05f);
            if (GeometryOverlaps(cameraPosition, radius))
                return true;

            return CharacterOccluded(cameraPosition, lookPoint);
        }

        private bool CharacterOccluded(Vector3 cameraPosition, Vector3 lookPoint)
        {
            Vector3 origin = _target != null ? _target.position : lookPoint - _lookAtOffset;
            Vector3 side = Vector3.Cross(Vector3.up, cameraPosition - origin);
            if (side.sqrMagnitude < 0.0001f)
                side = Vector3.right;
            else
                side.Normalize();

            const float halfWidth = 0.34f;
            Vector3 head = origin + new Vector3(0f, 1.55f, 0f);
            Vector3 chest = origin + new Vector3(0f, 1.15f, 0f);

            if (PointOccluded(cameraPosition, head))
                return true;
            if (PointOccluded(cameraPosition, chest))
                return true;
            if (PointOccluded(cameraPosition, chest + side * halfWidth))
                return true;
            if (PointOccluded(cameraPosition, chest - side * halfWidth))
                return true;

            return false;
        }

        private bool PointOccluded([Bridge.Ref] Vector3 from, [Bridge.Ref] Vector3 point)
        {
            if (GeometryOverlaps(point, 0.04f))
                return false;

            Vector3 delta = point - from;
            float distance = delta.magnitude;
            float castDistance = distance - 0.1f;
            if (castDistance <= 0.02f)
                return false;

            int count = Physics.RaycastNonAlloc(
                from,
                delta / distance,
                _rayHits,
                castDistance,
                CollisionMask,
                QueryTriggerInteraction.Ignore);

            return NearestHitDistance(count) < float.MaxValue;
        }

        private float NearestHitDistance(int count)
        {
            float nearest = float.MaxValue;
            int safeCount = Mathf.Min(count, _rayHits.Length);
            for (int i = 0; i < safeCount; i++)
            {
                RaycastHit hit = _rayHits[i];
                if (hit.collider == null || IsIgnoredCollider(hit.collider))
                    continue;

                if (hit.distance < nearest)
                    nearest = hit.distance;
            }

            return nearest;
        }

        private bool GeometryOverlaps([Bridge.Ref] Vector3 point, float radius)
        {
            int count = Physics.OverlapSphereNonAlloc(
                point,
                radius,
                _overlapHits,
                CollisionMask,
                QueryTriggerInteraction.Ignore);

            int safeCount = Mathf.Min(count, _overlapHits.Length);
            for (int i = 0; i < safeCount; i++)
            {
                if (IsIgnoredCollider(_overlapHits[i]) == false)
                    return true;
            }

            return false;
        }

        private bool IsIgnoredCollider(Collider collider)
        {
            if (collider == null)
                return true;

            Transform hitTransform = collider.transform;
            if (_target != null && (hitTransform == _target || hitTransform.IsChildOf(_target)))
                return true;

            return hitTransform == transform || hitTransform.IsChildOf(transform);
        }

        private void ApplyLens(float armRadius, float restRadius)
        {
            EnsureCameraDefaults();
            if (_camera == null || _camera.orthographic)
                return;

            float closeness = 1f - Mathf.Clamp01(armRadius / Mathf.Max(restRadius, 0.001f));
            float boostedFieldOfView = Mathf.Min(_restFieldOfView + Mathf.Max(_occludedFieldOfViewBoost, 0f), 100f);
            _camera.fieldOfView = Mathf.Lerp(_restFieldOfView, boostedFieldOfView, closeness);

            float closeNearClip = Mathf.Clamp(Mathf.Min(_occludedNearClip, _restNearClip), 0.01f, _restNearClip);
            _camera.nearClipPlane = Mathf.Lerp(_restNearClip, closeNearClip, closeness);
        }

        private void RestoreCameraLens()
        {
            if (_cameraDefaultsReady == false || _camera == null)
                return;

            _camera.fieldOfView = _restFieldOfView;
            _camera.nearClipPlane = _restNearClip;
        }

        private void EnsureCameraDefaults()
        {
            if (_cameraDefaultsReady)
                return;

            _camera = GetComponent<Camera>();
            if (_camera == null)
                return;

            _restFieldOfView = _camera.fieldOfView;
            _restNearClip = Mathf.Max(_camera.nearClipPlane, 0.01f);
            _cameraDefaultsReady = true;
        }

        private void ApplyLook(float restElevation)
        {
            GamePlayerContext context = GamePlayerContext.Instance;
            if (context != null && context.IsControlEnabled == false)
                return;

            if (_lookInputService == null)
                return;

            Vector2 lookDelta = _lookInputService.LookDelta;
            if (lookDelta.sqrMagnitude < 0.0001f)
                return;

            float screenHeight = Mathf.Max(Screen.height, 1);
            float yawDelta = lookDelta.x / screenHeight * _yawSensitivity;
            float pitchDelta = lookDelta.y / screenHeight * _pitchSensitivity;

            if (_invertY)
                pitchDelta = -pitchDelta;

            _lookYaw += yawDelta;
            _lookPitch -= pitchDelta;
            _lookPitch = ClampElevation(restElevation + _lookPitch) - restElevation;
        }

        private void GetRestArm(out float radius, out float elevation, out float yaw)
        {
            Vector3 restOffset = Quaternion.Euler(_angles.x, _angles.y, 0f) * _offset;
            radius = Mathf.Max(restOffset.magnitude, 0.001f);

            float horizontal = Mathf.Sqrt(restOffset.x * restOffset.x + restOffset.z * restOffset.z);
            elevation = Mathf.Atan2(restOffset.y, Mathf.Max(horizontal, 0.0001f)) * Mathf.Rad2Deg;
            yaw = horizontal > 0.0001f
                ? Mathf.Atan2(restOffset.x, restOffset.z) * Mathf.Rad2Deg
                : _angles.y;
        }

        private float ClampElevation(float elevation)
        {
            float minPitch = Mathf.Min(_minPitch, _maxPitch);
            float maxPitch = Mathf.Max(_minPitch, _maxPitch);
            return Mathf.Clamp(elevation, Mathf.Max(minPitch, -POLE_LIMIT), Mathf.Min(maxPitch, POLE_LIMIT));
        }

        private static Vector3 ArmFromSpherical(float radius, float elevation, float yaw)
        {
            float elevationRad = elevation * Mathf.Deg2Rad;
            float yawRad = yaw * Mathf.Deg2Rad;
            float horizontal = radius * Mathf.Cos(elevationRad);

            return new Vector3(
                Mathf.Sin(yawRad) * horizontal,
                radius * Mathf.Sin(elevationRad),
                Mathf.Cos(yawRad) * horizontal);
        }

        private Transform ResolveTarget()
        {
            Transform player = _spawnService != null ? _spawnService.Player : null;
            if (player != null)
            {
                _target = player;
                return _target;
            }

            if (_target != null)
                return _target;

            GamePlayerCharacter character = FindObjectOfType<GamePlayerCharacter>();
            if (character != null)
            {
                _target = character.transform;
                return _target;
            }

            GamePlayerMovement movement = FindObjectOfType<GamePlayerMovement>();
            if (movement == null)
                return null;

            _target = movement.transform;
            return _target;
        }

        private void EnsureServices()
        {
            if (_spawnService != null && _lookInputService != null)
                return;

            GamePlayerContext context = GamePlayerContext.Instance;
            if (context == null)
                return;

            Construct(context.SpawnService, context.LookInputService);
        }
    }
}

using Code.Gameplay.PlayerLogic.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Gameplay.PlayerLogic.Movement
{
    public class GamePlayerMovement : MonoBehaviour
    {
        private const float GROUND_RAY_OFFSET = 0.1f;
        private const float DEFAULT_MOVE_SPEED = 5f;
        private const float DEFAULT_ROTATION_SPEED = 120f;
        private const float DEFAULT_JUMP_FORCE = 7f;
        private const float DEFAULT_JUMP_COOLDOWN = 0.3f;
        private const float DEFAULT_GROUND_CHECK_DISTANCE = 0.15f;
        private const float DEFAULT_WALL_CHECK_DISTANCE = 0.6f;
        private const float DEFAULT_JUMP_BLOCK_TIME = 0.4f;
        private const float DEFAULT_CLIMB_SPEED = 3f;
        private const float DEFAULT_CLIMB_HEAD_HEIGHT = 1.05f;
        private const float DEFAULT_CLIMB_STICK_SKIN = 0.05f;
        private const float CLIMB_STICK_DEADZONE = 0.02f;
        private const float MAX_CLIMBABLE_NORMAL_Y = 0.45f;
        private const float MOVE_INPUT_DEADZONE = 0.1f;
        private const float DETACH_FROM_WALL_DOT = 0.5f;
        private const float CLIMB_UP_DOT = 0.15f;
        private const float GROUND_PROBE_MAX = 0.25f;
        private const float GROUND_SPHERE_SCALE = 0.9f;
        private const float WALL_SKIN = 0.08f;
        private const float WALL_CONTACT_MIN_INTO = 0.01f;
        private const float COYOTE_TIME = 0.12f;
        private const float JUMP_WALL_SEPARATION = 1.5f;
        private const float MAX_STEP_HEIGHT = 0.28f;
        private const float MIN_STEP_HEIGHT = 0.04f;
        private const float STEP_LIFT_SKIN = 0.02f;
        private const float WALKABLE_NORMAL_Y = 0.5f;
        private const int OVERLAP_BUFFER_SIZE = 16;
        private const string ANIMATOR_SPEED = "Speed";
        private const string ANIMATOR_JUMP = "Jump";
        private const string ANIMATOR_CLIMB = "Climb";
        private const string ANIMATOR_IDLE = "Idle";
        private const string ANIMATOR_RUN = "Run";
        private const string CLIMBABLE_LAYER = "Climbable";
        private const float FOOT_SINK_CORRECT = 0.25f;

        [FormerlySerializedAs("moveSpeed")]
        [SerializeField] private float _moveSpeed = DEFAULT_MOVE_SPEED;
        [FormerlySerializedAs("rotationSpeed")]
        [SerializeField] private float _rotationSpeed = DEFAULT_ROTATION_SPEED;
        [FormerlySerializedAs("jumpForce")]
        [SerializeField] private float _jumpForce = DEFAULT_JUMP_FORCE;
        [FormerlySerializedAs("jumpCooldown")]
        [SerializeField] private float _jumpCooldown = DEFAULT_JUMP_COOLDOWN;
        [FormerlySerializedAs("groundCheckDistance")]
        [SerializeField] private float _groundCheckDistance = DEFAULT_GROUND_CHECK_DISTANCE;
        [FormerlySerializedAs("groundMask")]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _climbMask;
        [SerializeField] private float _wallCheckDistance = DEFAULT_WALL_CHECK_DISTANCE;
        [SerializeField] private float _jumpBlockTime = DEFAULT_JUMP_BLOCK_TIME;
        [SerializeField] private float _climbSpeed = DEFAULT_CLIMB_SPEED;
        [SerializeField] private float _climbHeadHeight = DEFAULT_CLIMB_HEAD_HEIGHT;
        [SerializeField] private AudioSource _jumpSound;

        private IPlayerInputService _inputService;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private CapsuleCollider _capsule;
        private RigidbodyConstraints _defaultConstraints;
        private readonly Collider[] _overlapBuffer = new Collider[OVERLAP_BUFFER_SIZE];
        private float _jumpTimer;
        private bool _jumpRequested;
        private float _jumpBlockTimer;
        private float _coyoteTimer;
        private Vector3 _moveForward;
        private Vector3 _moveRight;
        private Vector3 _moveDirection;
        private Vector3 _wallNormal;
        private float _wallDistance;
        private Vector3 _collisionWallNormal;
        private int _wallContactFrame = int.MinValue;
        private bool _isClimbing;
        private bool _hasMoveInput;
        private float _moveMagnitude;
        private bool _playworksMotion;
        private Vector3 _motionPosition;
        private float _yaw;
        private float _verticalVelocity;
        private float _stableGroundY;
        private bool _hasStableGround;
        private bool _airborne;
        private float _groundLockY;
        private float _groundLockTimer;
        private int _groundMisses;
        private Transform _poseRoot;
        private Transform _leftFoot;
        private Transform _rightFoot;
        private Vector3 _poseRootLocalPosition;
        private bool _exitJumpPose;
        private readonly RaycastHit[] _rayHits = new RaycastHit[16];

        public void Construct(IPlayerInputService inputService) => _inputService = inputService;

        private const int ENVIRONMENT_LAYER = 6;

        private LayerMask ClimbMask
        {
            get
            {
                if (_climbMask.value != 0)
                    return _climbMask;

                int climbable = LayerMask.NameToLayer(CLIMBABLE_LAYER);
                if (climbable < 0)
                    return 0;

                return 1 << climbable;
            }
        }

        private LayerMask GroundMask =>
            _groundMask.value != 0 ? _groundMask : (LayerMask)Physics.DefaultRaycastLayers;

        private LayerMask ObstacleMask
        {
            get
            {
                int mask = GroundMask.value;
                if (mask == 0)
                    mask = ~0;

                mask |= ClimbMask.value;
                mask |= 1;
                mask |= 1 << ENVIRONMENT_LAYER;
                mask &= ~(1 << 2);
                return mask;
            }
        }

        private bool HasWallContact => Time.frameCount - _wallContactFrame <= 2;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            _capsule = GetComponent<CapsuleCollider>();

            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            if (_animator != null)
                _animator.applyRootMotion = false;

            EnsureInput();

            if (_rigidbody == null)
                return;

            _defaultConstraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.constraints = _defaultConstraints;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rigidbody.angularDamping = 10f;
            ApplyNoFrictionMaterial();
            ConfigurePlayworksBody();
        }

        private void ConfigurePlayworksBody()
        {
#if UNITY_LUNA
            // Goblin sinks a capsule until its center sits on the floor, then pops it back.
            // The body stays kinematic and out of the contact solver; the script places the feet.
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _playworksMotion = true;
            _motionPosition = _rigidbody.position;
            _yaw = _rigidbody.rotation.eulerAngles.y;
            KeepPlayworksBodyOutOfSolver();
            CachePlayworksPose();
#endif
        }

        private void OnEnable()
        {
            Application.onBeforeRender += CorrectPoseBeforeRender;
        }

        private void OnDisable()
        {
            Application.onBeforeRender -= CorrectPoseBeforeRender;
        }

        private void OnAnimatorMove()
        {
            if (_playworksMotion == false || _rigidbody == null)
                return;

            Quaternion rotation = Quaternion.Euler(0f, _yaw, 0f);
            transform.SetPositionAndRotation(_motionPosition, rotation);
            _rigidbody.position = _motionPosition;
            _rigidbody.rotation = rotation;
            LiftSunkPose();
        }

        private void CachePlayworksPose()
        {
            if (_animator == null || _animator.avatar == null || _animator.avatar.isHuman == false)
                return;

            _leftFoot = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            _rightFoot = _animator.GetBoneTransform(HumanBodyBones.RightFoot);
            Transform hips = _animator.GetBoneTransform(HumanBodyBones.Hips);
            if (hips == null || hips.parent == null || hips.parent == transform)
                return;

            _poseRoot = hips.parent;
            _poseRootLocalPosition = _poseRoot.localPosition;
        }

        private void CorrectPoseBeforeRender()
        {
            if (_playworksMotion == false)
                return;

            KeepPlayworksBodyOutOfSolver();
            Quaternion rotation = Quaternion.Euler(0f, _yaw, 0f);
            transform.SetPositionAndRotation(_motionPosition, rotation);
            _rigidbody.position = _motionPosition;
            _rigidbody.rotation = rotation;
            LiftSunkPose();
        }

        private void LiftSunkPose()
        {
            if (_poseRoot == null)
                CachePlayworksPose();

            if (_poseRoot == null || _leftFoot == null || _rightFoot == null)
                return;

            Vector3 local = _poseRoot.localPosition;
            local.y = _poseRootLocalPosition.y;
            _poseRoot.localPosition = local;

            float lowestFoot = Mathf.Min(_leftFoot.position.y, _rightFoot.position.y);
            float drop = transform.position.y - lowestFoot;
            if (drop <= FOOT_SINK_CORRECT)
                return;

            local = _poseRoot.localPosition;
            local.y += drop;
            _poseRoot.localPosition = local;
        }

        private void ExitJumpPose()
        {
            if (_animator == null || _exitJumpPose == false || _airborne)
                return;

            _animator.applyRootMotion = false;
            _animator.ResetTrigger(ANIMATOR_JUMP);

            if (_hasMoveInput == false)
                return;

            _animator.Play(ANIMATOR_RUN, 0, 0f);
            _animator.Update(0f);
            _exitJumpPose = false;
        }

        private void KeepPlayworksBodyOutOfSolver()
        {
            if (_rigidbody == null)
                return;

            // Goblin embeds a dynamic capsule until its center lies on the floor.
            // That is half the body height, then the next sample stands the feet back up.
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
            _rigidbody.detectCollisions = false;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void NotifyTeleported()
        {
            if (_rigidbody == null)
                return;

            _motionPosition = _rigidbody.position;
            _yaw = _rigidbody.rotation.eulerAngles.y;
            _verticalVelocity = 0f;
        }

        private void ApplyNoFrictionMaterial()
        {
            if (_capsule == null)
                return;

            var material = new PhysicsMaterial()
            {
                dynamicFriction = 0f,
                staticFriction = 0f,
                bounciness = 0f,
                frictionCombine = PhysicsMaterialCombine.Minimum,
                bounceCombine = PhysicsMaterialCombine.Minimum
            };

            _capsule.material = material;
        }

        private void Update()
        {
            EnsureInput();

            if (IsControlEnabled() == false)
            {
                _jumpRequested = false;
                if (_animator != null)
                    _animator.SetFloat(ANIMATOR_SPEED, 0f);
                return;
            }

            if (_inputService == null)
                return;

            if (_jumpTimer > 0f)
                _jumpTimer -= Time.deltaTime;

            bool canJump = _coyoteTimer > 0f || IsGrounded() || _isClimbing;
            if (_inputService.IsJumpPressed && _jumpTimer <= 0f && canJump)
                _jumpRequested = true;

            if (_animator == null)
                return;

            _animator.SetFloat(ANIMATOR_SPEED, _inputService.MoveInput.magnitude);
            _animator.SetBool(ANIMATOR_CLIMB, _isClimbing);
        }

        private void LateUpdate()
        {
            if (_playworksMotion == false || _rigidbody == null)
                return;

            if (_animator != null)
                _animator.applyRootMotion = false;

            KeepPlayworksBodyOutOfSolver();
            Quaternion rotation = Quaternion.Euler(0f, _yaw, 0f);
            _rigidbody.position = _motionPosition;
            _rigidbody.rotation = rotation;
            transform.SetPositionAndRotation(_motionPosition, rotation);
            LiftSunkPose();
        }

        private void FixedUpdate()
        {
            EnsureInput();

            if (IsControlEnabled() == false)
            {
                _jumpRequested = false;
                _hasMoveInput = false;
                return;
            }

            if (_inputService == null || _rigidbody == null)
                return;

#if UNITY_LUNA
            SimulatePlayworksMotion();
            return;
#endif

            if (_jumpBlockTimer > 0f)
                _jumpBlockTimer -= Time.fixedDeltaTime;

            _hasMoveInput = TryGetMoveDirection(out _moveDirection, out _moveMagnitude);

            if (_jumpRequested)
            {
                StopClimbing();
                Jump();
                _jumpRequested = false;
                _jumpBlockTimer = _jumpBlockTime;
                ApplyPlanarMovement(keepAirMomentum: true);
                return;
            }

            if (TryKeepClimbing())
            {
                ApplyClimb();
                return;
            }

            StopClimbing();
            SeparateFromObstacles();
            UpdateCoyote(IsGrounded());

            if (_hasMoveInput)
            {
                ApplyRotation(_moveDirection);
                ApplyPlanarMovement(keepAirMomentum: true);
                return;
            }

            ApplyPlanarMovement(keepAirMomentum: _coyoteTimer <= 0f);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (IsSteppableCollider(collision.collider))
                return;

            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector3 normal = collision.GetContact(i).normal;
                if (normal.y >= MAX_CLIMBABLE_NORMAL_Y)
                    continue;

                Vector3 planar = Flatten(normal);
                if (planar.sqrMagnitude < 0.001f)
                    continue;

                _collisionWallNormal = planar.normalized;
                _wallContactFrame = Time.frameCount;
                return;
            }
        }

        private bool TryGetMoveDirection(out Vector3 direction, out float magnitude)
        {
            Vector2 input = _inputService.MoveInput;
            magnitude = Mathf.Clamp01(input.magnitude);

            if (magnitude < MOVE_INPUT_DEADZONE)
            {
                direction = Vector3.zero;
                return false;
            }

            CacheMoveBasis();

            direction = _moveForward * input.y + _moveRight * input.x;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = Vector3.zero;
                return false;
            }

            direction.Normalize();
            return true;
        }

        private void CacheMoveBasis()
        {
            Transform basisSource = ResolveCameraTransform();
            if (basisSource == null)
                basisSource = transform;

            Vector3 forward = basisSource.forward;
            forward.y = 0f;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.ProjectOnPlane(basisSource.up, Vector3.up);

            if (forward.sqrMagnitude < 0.001f)
                forward = transform.forward;

            _moveForward = forward.normalized;
            _moveRight = Vector3.Cross(Vector3.up, _moveForward).normalized;
        }

        private Transform ResolveCameraTransform()
        {
            GamePlayerContext context = GamePlayerContext.Resolve();
            if (context != null && context.CameraTransform != null)
                return context.CameraTransform;

            Camera main = Camera.main;
            return main != null ? main.transform : null;
        }

        private bool TryKeepClimbing()
        {
            Vector3 probeDirection = GetClimbProbeDirection();
            if (TryGetClimbableWall(probeDirection, out RaycastHit wallHit) == false)
                return false;

            if (_hasMoveInput && Vector3.Dot(_moveDirection, Flatten(wallHit.normal)) > DETACH_FROM_WALL_DOT)
                return false;

            _wallNormal = Flatten(wallHit.normal);
            if (_wallNormal.sqrMagnitude < 0.001f)
                _wallNormal = Flatten(-probeDirection);

            _wallNormal.Normalize();
            if (_isClimbing)
                _wallDistance = Mathf.Lerp(_wallDistance, wallHit.distance, 0.35f);
            else
                _wallDistance = wallHit.distance;

            return true;
        }

        private Vector3 GetClimbProbeDirection()
        {
            if (_isClimbing)
            {
                Vector3 intoWall = Flatten(-_wallNormal);
                if (intoWall.sqrMagnitude > 0.001f)
                    return intoWall.normalized;
            }

            if (_hasMoveInput)
                return _moveDirection;

            return transform.forward;
        }

        private void ApplyClimb()
        {
            StartClimbing();

            Vector3 faceDirection = Flatten(-_wallNormal);
            if (faceDirection.sqrMagnitude > 0.0001f)
                ApplyUprightYaw(faceDirection.normalized);

            bool climbUp = _hasMoveInput && Vector3.Dot(_moveDirection, Flatten(-_wallNormal)) > CLIMB_UP_DOT;
            Vector3 nextPosition = GetStuckPosition(_rigidbody.position);

            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            if (climbUp)
            {
                nextPosition += Vector3.up * (_climbSpeed * _moveMagnitude * Time.fixedDeltaTime);

                if (IsHeadBlocked() == false)
                    nextPosition += Flatten(-_wallNormal).normalized * (_moveSpeed * _moveMagnitude * Time.fixedDeltaTime);

                MoveBody(nextPosition);
                return;
            }

            if ((nextPosition - _rigidbody.position).sqrMagnitude > 0.0001f)
                MoveBody(nextPosition);
        }

        private Vector3 GetStuckPosition(Vector3 currentPosition)
        {
            Vector3 stickNormal = Flatten(_wallNormal);
            if (stickNormal.sqrMagnitude < 0.001f)
                return currentPosition;

            stickNormal.Normalize();
            float radius = _capsule != null ? _capsule.radius : 0.25f;
            float desiredDistance = radius + DEFAULT_CLIMB_STICK_SKIN;
            float error = _wallDistance - desiredDistance;

            if (Mathf.Abs(error) <= CLIMB_STICK_DEADZONE)
            {
                currentPosition.y = _rigidbody.position.y;
                return currentPosition;
            }

            Vector3 stuckPosition = currentPosition + stickNormal * -error;
            stuckPosition.y = currentPosition.y;
            return stuckPosition;
        }

        private void StartClimbing()
        {
            if (_isClimbing)
                return;

            _isClimbing = true;
            _rigidbody.useGravity = false;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            ApplyUprightYaw(Flatten(-_wallNormal));
        }

        private void StopClimbing()
        {
            if (_isClimbing == false)
                return;

            _isClimbing = false;
            _rigidbody.useGravity = _playworksMotion == false;

            _rigidbody.constraints = _defaultConstraints;
        }

        private void ApplyPlanarMovement(bool keepAirMomentum)
        {
            Vector3 planarVelocity;

            if (_hasMoveInput)
            {
                planarVelocity = SlideAlongWalls(_moveDirection * (_moveSpeed * _moveMagnitude));
            }
            else if (keepAirMomentum)
            {
                Vector3 current = _rigidbody.linearVelocity;
                planarVelocity = SlideAlongWalls(new Vector3(current.x, 0f, current.z));
            }
            else
            {
                planarVelocity = Vector3.zero;
            }

            Vector3 velocity = _rigidbody.linearVelocity;
            velocity.x = planarVelocity.x;
            velocity.z = planarVelocity.z;
            _rigidbody.linearVelocity = velocity;
        }

        private Vector3 SlideAlongWalls(Vector3 planarVelocity)
        {
            if (planarVelocity.sqrMagnitude < 0.0001f)
                return Vector3.zero;

            if (TryStepUp(planarVelocity))
                return planarVelocity;

            Vector3 velocity = planarVelocity;
            velocity = SlideOffHit(velocity);
            velocity = SlideOffHit(velocity);

            if (HasWallContact && Vector3.Dot(velocity, -_collisionWallNormal) > WALL_CONTACT_MIN_INTO)
                velocity = Vector3.ProjectOnPlane(velocity, _collisionWallNormal);

            velocity.y = 0f;
            return velocity;
        }

        private bool TryStepUp(Vector3 planarVelocity)
        {
            if (_coyoteTimer <= 0f)
                return false;

            Vector3 direction = Flatten(planarVelocity);
            if (direction.sqrMagnitude < 0.0001f)
                return false;

            direction.Normalize();
            GetCapsuleWorldPoints(out Vector3 bottom, out Vector3 top, out float radius);
            float feetY = bottom.y - radius;
            float probe = radius + WALL_SKIN + planarVelocity.magnitude * Time.fixedDeltaTime;

            if (TryCapsuleCast(direction, probe, ObstacleMask, out RaycastHit forwardHit) == false)
                return false;

            if (forwardHit.normal.y >= WALKABLE_NORMAL_Y)
                return false;

            if (IsSteppableCollider(forwardHit.collider) == false)
            {
                float boundHeight = forwardHit.collider != null
                    ? ColliderTopY(forwardHit.collider) - feetY
                    : MAX_STEP_HEIGHT + 1f;
                if (boundHeight > MAX_STEP_HEIGHT)
                    return false;
            }

            Vector3 surfaceProbe = _rigidbody.position + direction * (radius + 0.12f);
            surfaceProbe.y = feetY + MAX_STEP_HEIGHT + 0.12f;
            float sphereRadius = Mathf.Max(0.04f, radius * 0.3f);

            float stepHeight;
            if (Physics.SphereCast(
                    surfaceProbe,
                    sphereRadius,
                    Vector3.down,
                    out RaycastHit surfaceHit,
                    MAX_STEP_HEIGHT + 0.2f,
                    ObstacleMask,
                    QueryTriggerInteraction.Ignore)
                && surfaceHit.normal.y >= WALKABLE_NORMAL_Y)
            {
                stepHeight = surfaceHit.point.y - feetY;
            }
            else if (IsSteppableCollider(forwardHit.collider))
            {
                stepHeight = ColliderTopY(forwardHit.collider) - feetY;
            }
            else
            {
                return false;
            }

            if (stepHeight < MIN_STEP_HEIGHT || stepHeight > MAX_STEP_HEIGHT)
                return false;

            float lift = stepHeight + STEP_LIFT_SKIN;
            float castRadius = Mathf.Max(0.01f, radius - 0.02f);

            if (Physics.CapsuleCast(
                    bottom,
                    top,
                    castRadius,
                    Vector3.up,
                    lift,
                    ObstacleMask,
                    QueryTriggerInteraction.Ignore))
                return false;

            Vector3 liftedBottom = bottom + Vector3.up * lift;
            Vector3 liftedTop = top + Vector3.up * lift;
            if (Physics.CapsuleCast(
                    liftedBottom,
                    liftedTop,
                    castRadius,
                    direction,
                    out RaycastHit raisedHit,
                    radius * 0.45f,
                    ObstacleMask,
                    QueryTriggerInteraction.Ignore)
                && raisedHit.normal.y < WALKABLE_NORMAL_Y)
                return false;

            Vector3 position = _rigidbody.position;
            position.y += lift;
            _rigidbody.position = position;
            return true;
        }

        private bool IsSteppableCollider(Collider other)
        {
            if (other == null)
                return false;

            GetCapsuleWorldPoints(out Vector3 bottom, out _, out float radius);
            float feetY = bottom.y - radius;
            float height = ColliderTopY(other) - feetY;
            return height > MIN_STEP_HEIGHT && height <= MAX_STEP_HEIGHT;
        }

        private static float ColliderTopY(Collider other)
        {
            Bounds bounds = other.bounds;
            return bounds.center.y + bounds.extents.y;
        }

        private Vector3 SlideOffHit(Vector3 planarVelocity)
        {
            if (planarVelocity.sqrMagnitude < 0.0001f)
                return Vector3.zero;

            Vector3 direction = planarVelocity.normalized;
            float probe = GetCapsuleRadius() + WALL_SKIN + planarVelocity.magnitude * Time.fixedDeltaTime;

            if (TryCapsuleCast(direction, probe, ObstacleMask, out RaycastHit hit) == false)
                return planarVelocity;

            if (hit.normal.y >= WALKABLE_NORMAL_Y)
                return planarVelocity;

            Vector3 normal = Flatten(hit.normal);
            if (normal.sqrMagnitude < 0.001f)
                return planarVelocity;

            normal.Normalize();
            if (Vector3.Dot(direction, -normal) <= WALL_CONTACT_MIN_INTO)
                return planarVelocity;

            return Vector3.ProjectOnPlane(planarVelocity, normal);
        }

        private void ApplyUprightYaw(Vector3 planarForward)
        {
            planarForward = Flatten(planarForward);
            if (planarForward.sqrMagnitude < 0.0001f)
                return;

            SetYaw(Quaternion.LookRotation(planarForward.normalized, Vector3.up));
        }

        private void ApplyRotation(Vector3 direction)
        {
            direction = Flatten(direction);
            if (direction.sqrMagnitude < 0.0001f)
                return;

            float currentYaw = _playworksMotion ? _yaw : _rigidbody.rotation.eulerAngles.y;
            Quaternion currentRotation = Quaternion.Euler(0f, currentYaw, 0f);
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            Quaternion nextRotation = Quaternion.RotateTowards(
                currentRotation,
                targetRotation,
                _rotationSpeed * Time.fixedDeltaTime);

            SetYaw(nextRotation);
        }

        private void SetYaw(Quaternion rotation)
        {
            float yaw = rotation.eulerAngles.y;
            Quaternion flat = Quaternion.Euler(0f, yaw, 0f);
            _rigidbody.angularVelocity = Vector3.zero;

            if (_playworksMotion)
            {
                _yaw = yaw;
                _rigidbody.rotation = flat;
                transform.rotation = flat;
                return;
            }

            _rigidbody.MoveRotation(flat);
        }

        private void MoveBody(Vector3 position)
        {
            if (_playworksMotion)
            {
                _motionPosition = position;
                _rigidbody.position = position;
                transform.position = position;
                return;
            }

            _rigidbody.MovePosition(position);
        }

        private void Jump()
        {
            if (_playworksMotion)
            {
                float mass = Mathf.Max(_rigidbody.mass, 0.001f);
                _verticalVelocity = _jumpForce / mass;
                _hasStableGround = false;
                _airborne = true;
                _groundLockTimer = 0f;
                _groundMisses = 0;
                _exitJumpPose = _hasMoveInput;
                if (HasWallContact)
                    _motionPosition += Flatten(_collisionWallNormal).normalized * 0.15f;
            }
            else
            {
                Vector3 velocity = _rigidbody.linearVelocity;
                velocity.y = 0f;

                if (HasWallContact)
                    velocity += _collisionWallNormal * JUMP_WALL_SEPARATION;

                _rigidbody.linearVelocity = velocity;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
            _jumpTimer = _jumpCooldown;
            _coyoteTimer = 0f;

            if (_animator != null)
                _animator.SetTrigger(ANIMATOR_JUMP);

            if (_jumpSound)
                _jumpSound.Play();
        }

        private void UpdateCoyote(bool grounded)
        {
            _coyoteTimer = grounded ? COYOTE_TIME : _coyoteTimer - Time.fixedDeltaTime;
        }

        private void SeparateFromObstacles()
        {
            if (_capsule == null)
                return;

            GetCapsuleWorldPoints(out Vector3 bottom, out Vector3 top, out float radius);
            int count = Physics.OverlapCapsuleNonAlloc(
                bottom,
                top,
                radius,
                _overlapBuffer,
                ObstacleMask,
                QueryTriggerInteraction.Ignore);

            Vector3 correction = Vector3.zero;

            for (int i = 0; i < count; i++)
            {
                Collider other = _overlapBuffer[i];
                if (other == null || other == _capsule)
                    continue;

                if (Physics.ComputePenetration(
                        _capsule,
                        _rigidbody.position,
                        _rigidbody.rotation,
                        other,
                        other.transform.position,
                        other.transform.rotation,
                        out Vector3 direction,
                        out float distance) == false)
                    continue;

                if (distance < 0.001f)
                    continue;

                if (Mathf.Abs(direction.y) >= MAX_CLIMBABLE_NORMAL_Y)
                    continue;

                if (IsSteppableCollider(other))
                    continue;

                Vector3 push = Flatten(direction) * distance;
                if (push.sqrMagnitude < 0.0000001f)
                    continue;

                correction += push;
            }

            if (correction.sqrMagnitude < 0.0000001f)
                return;

            _rigidbody.position += correction;
        }

        private bool IsHeadBlocked()
        {
            return TryGetClimbableWall(Flatten(-_wallNormal), GetHeadHeight(), out _);
        }

        private bool TryGetClimbableWall(Vector3 direction, out RaycastHit hit) =>
            TryGetClimbableWall(direction, GetTorsoHeight(), out hit);

        private bool TryGetClimbableWall(Vector3 direction, float height, out RaycastHit hit)
        {
            if (TryGetWallHit(direction, height, ClimbMask, out hit) == false)
                return false;

            return hit.normal.y < MAX_CLIMBABLE_NORMAL_Y;
        }

        private bool TryGetWallHit(Vector3 direction, float height, LayerMask mask, out RaycastHit hit)
        {
            if (direction.sqrMagnitude < 0.0001f)
            {
                hit = default;
                return false;
            }

            float distance = _wallCheckDistance;
            if (_isClimbing)
                distance *= 1.25f;

            Vector3 rayOrigin = transform.position + Vector3.up * height;
            return Physics.Raycast(rayOrigin, direction, out hit, distance, mask, QueryTriggerInteraction.Ignore);
        }

        private bool TryCapsuleCast(Vector3 direction, float distance, LayerMask mask, out RaycastHit hit)
        {
            GetCapsuleWorldPoints(out Vector3 bottom, out Vector3 top, out float radius);
            float castRadius = Mathf.Max(0.01f, radius - 0.02f);
            return Physics.CapsuleCast(
                bottom,
                top,
                castRadius,
                direction,
                out hit,
                distance,
                mask,
                QueryTriggerInteraction.Ignore);
        }

        private void GetCapsuleWorldPoints(out Vector3 bottom, out Vector3 top, out float radius)
        {
            if (_capsule == null)
            {
                radius = 0.25f;
                Vector3 position = _rigidbody != null ? _rigidbody.position : transform.position;
                bottom = position + Vector3.up * radius;
                top = position + Vector3.up * (1.1f - radius);
                return;
            }

            Vector3 scale = transform.lossyScale;
            radius = _capsule.radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.z));
            float height = Mathf.Max(_capsule.height * Mathf.Abs(scale.y), radius * 2f);
            Vector3 center = transform.TransformPoint(_capsule.center);
            float shaft = height * 0.5f - radius;
            Vector3 up = transform.up;
            bottom = center - up * shaft;
            top = center + up * shaft;
        }

        private float GetCapsuleRadius()
        {
            GetCapsuleWorldPoints(out _, out _, out float radius);
            return radius;
        }

        private static Vector3 Flatten(Vector3 value)
        {
            value.y = 0f;
            return value;
        }

        private float GetTorsoHeight()
        {
            if (_capsule == null)
                return 0.5f;

            return _capsule.center.y;
        }

        private float GetHeadHeight()
        {
            if (_capsule == null)
                return _climbHeadHeight;

            return _capsule.center.y + _capsule.height * 0.5f;
        }

        private bool IsGrounded()
        {
            if (_playworksMotion)
                return TryGetGroundY(_motionPosition.sqrMagnitude > 0.0001f ? _motionPosition : transform.position, out _, 0.45f);

            float probeDistance = Mathf.Clamp(_groundCheckDistance, 0.05f, GROUND_PROBE_MAX);

            if (_capsule == null)
            {
                Vector3 rayOrigin = transform.position + Vector3.up * GROUND_RAY_OFFSET;
                return Physics.Raycast(
                    rayOrigin,
                    Vector3.down,
                    probeDistance,
                    GroundMask,
                    QueryTriggerInteraction.Ignore);
            }

            GetCapsuleWorldPoints(out Vector3 bottom, out _, out float radius);
            float sphereRadius = radius * GROUND_SPHERE_SCALE;
            Vector3 origin = bottom + Vector3.up * GROUND_RAY_OFFSET;

            if (Physics.SphereCast(
                    origin,
                    sphereRadius,
                    Vector3.down,
                    out RaycastHit hit,
                    probeDistance + GROUND_RAY_OFFSET,
                    GroundMask,
                    QueryTriggerInteraction.Ignore))
                return hit.collider != _capsule;

            return Physics.CheckSphere(
                bottom + Vector3.down * 0.04f,
                sphereRadius,
                GroundMask,
                QueryTriggerInteraction.Ignore);
        }

        private void EnsureInput()
        {
            if (_inputService != null)
                return;

            GamePlayerContext context = GamePlayerContext.Resolve();
            if (context == null)
                return;

            Construct(context.InputService);
        }

        private static bool IsControlEnabled()
        {
            GamePlayerContext context = GamePlayerContext.Resolve();
            return context == null || context.IsControlEnabled;
        }

        private void SimulatePlayworksMotion()
        {
            if (_jumpBlockTimer > 0f)
                _jumpBlockTimer -= Time.fixedDeltaTime;

            _hasMoveInput = TryGetMoveDirection(out _moveDirection, out _moveMagnitude);

            bool jumped = false;
            if (_jumpRequested)
            {
                StopClimbing();
                Jump();
                _jumpRequested = false;
                _jumpBlockTimer = _jumpBlockTime;
                jumped = true;
            }

            if (jumped == false && TryKeepClimbing())
            {
                _verticalVelocity = 0f;
                ApplyClimb();
                return;
            }

            StopClimbing();

            if (_hasMoveInput)
                ApplyRotation(_moveDirection);

            ExitJumpPose();

            float dt = Mathf.Max(Time.fixedDeltaTime, 0.0001f);
            KeepPlayworksBodyOutOfSolver();
            Vector3 position = _motionPosition;
            Vector3 planar = _hasMoveInput
                ? _moveDirection * (_moveSpeed * _moveMagnitude)
                : Vector3.zero;

            Vector3 delta = BlockPlayworksMove(position, planar * dt);
            position += delta;

            if (_groundLockTimer > 0f)
                _groundLockTimer -= dt;

            if (_verticalVelocity > 0.05f)
            {
                ApplyPlayworksGravity(ref position, dt);
                _airborne = true;
                _hasStableGround = false;
            }
            else if (_airborne || _hasStableGround == false)
            {
                ApplyPlayworksGravity(ref position, dt);
                float reach = Mathf.Clamp(-_verticalVelocity * dt + 0.12f, 0.2f, 0.45f);
                if (_verticalVelocity <= 0.05f
                    && TrySampleGround(position, position.y, 0.08f, reach, out float landedY)
                    && landedY <= position.y + 0.08f
                    && landedY >= position.y - reach)
                    LandPlayworks(ref position, landedY);
            }
            else if (TrySampleGround(position, position.y, MAX_STEP_HEIGHT, 0.15f, out float groundY)
                && groundY >= position.y - 0.15f)
            {
                if (_groundLockTimer > 0f && groundY < _groundLockY)
                    groundY = _groundLockY;

                position.y = groundY;
                _verticalVelocity = 0f;
                _stableGroundY = groundY;
                _hasStableGround = true;
                _groundMisses = 0;
            }
            else if (_groundMisses < 5)
            {
                _groundMisses++;
                if (_groundLockTimer > 0f)
                    position.y = Mathf.Max(position.y, _groundLockY);
                else if (_hasStableGround)
                    position.y = _stableGroundY;

                _verticalVelocity = 0f;
            }
            else
            {
                _airborne = true;
                _hasStableGround = false;
            }
            ResolveSolidOverlap(ref position);
            _motionPosition = position;
            _rigidbody.position = position;
            _rigidbody.rotation = Quaternion.Euler(0f, _yaw, 0f);
            UpdateCoyote(_verticalVelocity <= 0.01f && _hasStableGround);
        }

        private Vector3 BlockPlayworksMove(Vector3 position, Vector3 delta)
        {
            Vector3 flat = Flatten(delta);
            float distance = flat.magnitude;
            if (distance < 0.0001f)
                return Vector3.zero;

            Vector3 direction = flat / distance;
            float radius = GetCapsuleRadius();
            float skin = 0.06f;
            float nearest = distance;
            Vector3 wallNormal = Vector3.zero;
            bool blocked = false;
            float[] heights = { 0.12f, 0.35f, 0.75f, 1.15f };

            for (int i = 0; i < heights.Length; i++)
            {
                Vector3 origin = position + Vector3.up * heights[i];
                if (TryForwardHit(origin, direction, distance + radius, out RaycastHit hit) == false)
                    continue;

                if (hit.distance <= 0.001f || hit.normal.y >= WALKABLE_NORMAL_Y)
                    continue;

                float allowed = hit.distance - radius - skin;
                if (allowed < 0f)
                    allowed = 0f;

                if (allowed < nearest)
                {
                    nearest = allowed;
                    wallNormal = Flatten(hit.normal);
                    blocked = true;
                }
            }

            Vector3 move = direction * nearest;
            if (blocked == false || wallNormal.sqrMagnitude < 0.001f)
                return move;

            Vector3 leftover = direction * (distance - nearest);
            Vector3 slide = Vector3.ProjectOnPlane(leftover, wallNormal.normalized);
            if (slide.sqrMagnitude < 0.0001f)
                return move;

            Vector3 slideDirection = Flatten(slide).normalized;
            float slideDistance = Flatten(slide).magnitude;
            float slideAllowed = slideDistance;
            for (int i = 0; i < heights.Length; i++)
            {
                Vector3 origin = position + move + Vector3.up * heights[i];
                if (TryForwardHit(origin, slideDirection, slideDistance + radius, out RaycastHit hit) == false)
                    continue;

                if (hit.distance <= 0.001f || hit.normal.y >= WALKABLE_NORMAL_Y)
                    continue;

                float allowed = hit.distance - radius - skin;
                if (allowed < slideAllowed)
                    slideAllowed = Mathf.Max(0f, allowed);
            }

            return move + slideDirection * slideAllowed;
        }

        private void ResolveSolidOverlap(ref Vector3 position)
        {
            if (_capsule == null)
                return;

            float radius = GetCapsuleRadius();
            float height = Mathf.Max(_capsule.height, radius * 2f);
            Vector3 bottom = position + Vector3.up * radius;
            Vector3 top = position + Vector3.up * (height - radius);
            int count = Physics.OverlapCapsuleNonAlloc(
                bottom,
                top,
                radius * 0.92f,
                _overlapBuffer,
                ObstacleMask,
                QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider other = _overlapBuffer[i];
                if (IsOwnCollider(other))
                    continue;

                if (Physics.ComputePenetration(
                        _capsule,
                        position,
                        Quaternion.identity,
                        other,
                        other.transform.position,
                        other.transform.rotation,
                        out Vector3 direction,
                        out float distance))
                {
                    // Goblin reports the capsule-center depth (about half the body) as penetration.
                    // Applying that on Y sinks the feet into the floor and the next sample pulls them out.
                    if (distance < 0.001f || distance > radius || Mathf.Abs(direction.y) > 0.35f)
                        continue;

                    Vector3 push = Flatten(direction);
                    if (push.sqrMagnitude < 0.0001f)
                        continue;

                    position += push.normalized * Mathf.Min(distance, 0.12f);
                    continue;
                }

                PushOutOfBounds(other, radius, ref position);
            }
        }

        private static void PushOutOfBounds(Collider other, float radius, ref Vector3 position)
        {
            Bounds bounds = other.bounds;
            Vector3 extents = bounds.extents;
            if (extents.x < 0.01f || extents.z < 0.01f)
                return;

            if (extents.x > 2f || extents.y > 2f || extents.z > 2f)
                return;

            Vector3 min = bounds.center - extents;
            Vector3 max = bounds.center + extents;
            min.x -= radius;
            min.z -= radius;
            max.x += radius;
            max.z += radius;

            if (position.x <= min.x || position.x >= max.x || position.z <= min.z || position.z >= max.z)
                return;

            if (position.y > max.y + 0.05f || position.y + 1.8f < min.y)
                return;

            if (position.y >= max.y - 0.2f)
                return;

            float left = position.x - min.x;
            float right = max.x - position.x;
            float back = position.z - min.z;
            float forward = max.z - position.z;
            float best = left;
            Vector3 push = Vector3.left * left;

            if (right < best)
            {
                best = right;
                push = Vector3.right * right;
            }

            if (back < best)
            {
                best = back;
                push = Vector3.back * back;
            }

            if (forward < best)
                push = Vector3.forward * forward;

            position += push;
        }

        private void ApplyPlayworksGravity(ref Vector3 position, float dt)
        {
            float gravity = Physics.gravity.y;
            if (gravity > -0.1f)
                gravity = -9.81f;

            _verticalVelocity += gravity * dt;
            position.y += _verticalVelocity * dt;
        }

        private void LandPlayworks(ref Vector3 position, float groundY)
        {
            position.y = groundY;
            _verticalVelocity = 0f;
            _stableGroundY = groundY;
            _hasStableGround = true;
            _airborne = false;
            _groundMisses = 0;
            _groundLockY = groundY;
            _groundLockTimer = 0.35f;
        }

        private float GetCastAboveFeet()
        {
            if (_capsule == null)
                return 2.15f;

            return _capsule.center.y + _capsule.height * 0.5f + 0.2f;
        }

        private bool TryGetGroundY(Vector3 position, out float groundY, float extraBelow)
        {
            return TryGetSupportY(position, out groundY, extraBelow);
        }

        private bool TryGetSupportY(Vector3 position, out float groundY)
        {
            bool falling = _verticalVelocity < -0.05f;
            float reference = falling ? position.y : (_hasStableGround ? _stableGroundY : position.y);
            float drop = falling ? 4f : 0.35f;
            float rise = falling ? 0.08f : MAX_STEP_HEIGHT;
            return TrySampleGround(position, reference, rise, drop, out groundY);
        }

        private bool TryGetSupportY(Vector3 position, out float groundY, float extraBelow)
        {
            return TrySampleGround(position, position.y, MAX_STEP_HEIGHT, extraBelow, out groundY);
        }

        private bool TrySampleGround(Vector3 position, float referenceY, float maxRise, float maxDrop, out float groundY)
        {
            groundY = referenceY;
            float best = float.MinValue;
            bool found = false;
            float side = GetCapsuleRadius() + 0.05f;
            Vector3[] offsets =
            {
                Vector3.zero,
                new Vector3(side, 0f, 0f),
                new Vector3(-side, 0f, 0f),
                new Vector3(0f, 0f, side),
                new Vector3(0f, 0f, -side)
            };

            float aboveFeet = GetCastAboveFeet();
            float length = aboveFeet + Mathf.Max(maxDrop, 0.05f);
            for (int o = 0; o < offsets.Length; o++)
            {
                Vector3 origin = position + offsets[o];
                origin.y = referenceY + aboveFeet;
                int count = Physics.RaycastNonAlloc(
                    origin,
                    Vector3.down,
                    _rayHits,
                    length,
                    ObstacleMask,
                    QueryTriggerInteraction.Ignore);

                for (int i = 0; i < count; i++)
                {
                    RaycastHit hit = _rayHits[i];
                    if (IsOwnCollider(hit.collider) || hit.normal.y < WALKABLE_NORMAL_Y)
                        continue;

                    if (hit.distance <= 0.001f)
                        continue;

                    float byDistance = origin.y - hit.distance;
                    float byPoint = hit.point.y;
                    float surface = Mathf.Abs(byDistance - byPoint) <= 0.3f
                        ? byDistance
                        : (Mathf.Abs(byDistance - referenceY) <= Mathf.Abs(byPoint - referenceY) ? byDistance : byPoint);

                    if (surface > referenceY + maxRise || surface < referenceY - maxDrop)
                        continue;

                    if (surface > best)
                    {
                        best = surface;
                        found = true;
                    }
                }
            }

            if (found)
                groundY = best;

            return found;
        }

        private bool TryForwardHit(Vector3 origin, Vector3 direction, float distance, out RaycastHit hit)
        {
            hit = default;
            if (direction.sqrMagnitude < 0.0001f || distance <= 0f)
                return false;

            int count = Physics.RaycastNonAlloc(
                origin,
                direction.normalized,
                _rayHits,
                distance,
                ObstacleMask,
                QueryTriggerInteraction.Ignore);

            float best = float.MaxValue;
            bool found = false;
            for (int i = 0; i < count; i++)
            {
                RaycastHit candidate = _rayHits[i];
                if (IsOwnCollider(candidate.collider))
                    continue;

                if (candidate.distance < best)
                {
                    best = candidate.distance;
                    hit = candidate;
                    found = true;
                }
            }

            return found;
        }

        private bool IsOwnCollider(Collider other)
        {
            if (other == null || other == _capsule)
                return true;

            Transform otherTransform = other.transform;
            return otherTransform == transform || otherTransform.IsChildOf(transform);
        }
    }
}

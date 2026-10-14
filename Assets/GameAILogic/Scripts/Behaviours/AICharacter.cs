using System;
using Code.Gameplay.AI.Brains;
using Code.Gameplay.AI.Movement;
using UnityEngine;

namespace Code.Gameplay.AI.Behaviours
{
    public class AICharacter : MonoBehaviour
    {
        private const float DEFAULT_MOVE_SPEED = 5f;
        private const float DEFAULT_ROTATION_SPEED = 540f;
        private const float DEFAULT_JUMP_ARC_HEIGHT = 1.2f;
        private const float DEFAULT_NOISE_AMPLITUDE = 0.3f;
        private const float DEFAULT_NOISE_FREQUENCY = 1.1f;
        private const float DEFAULT_MAX_LANE_OFFSET = 0.45f;
        private const float GOLDEN_ANGLE = 2.399963f;
        private const float GOLDEN_RATIO_CONJUGATE = 0.618034f;

        [SerializeField] private Animator _animator;
        [SerializeField, Min(0f)] private float _moveSpeed = DEFAULT_MOVE_SPEED;
        [SerializeField, Min(0f)] private float _rotationSpeed = DEFAULT_ROTATION_SPEED;
        [SerializeField, Min(0f)] private float _jumpArcHeight = DEFAULT_JUMP_ARC_HEIGHT;
        [SerializeField, Min(0f)] private float _noiseAmplitude = DEFAULT_NOISE_AMPLITUDE;
        [SerializeField, Min(0f)] private float _noiseFrequency = DEFAULT_NOISE_FREQUENCY;
        [SerializeField, Min(0f)] private float _maxLaneOffset = DEFAULT_MAX_LANE_OFFSET;

        private AIBrain _brain;
        private BotKinematicMover _mover;
        private float _noisePhase;
        private float _laneOffset;
        private float _speedMultiplier = 1f;
        private bool _isActive;
        private bool _hasStarted;
        private bool _hasRequestedRestart;

        public BotKinematicMover Mover => _mover;
        public float MoveSpeed => _moveSpeed * _speedMultiplier;
        public float JumpArcHeight => _jumpArcHeight;
        public float NoiseAmplitude => _noiseAmplitude;
        public float NoiseFrequency => _noiseFrequency;
        public float NoisePhase => _noisePhase;
        public float LaneOffset => _laneOffset;
        public bool IsActive => _isActive;
        public bool IsFinished => _brain == null || _brain.IsFinished;
        public event Action<AICharacter> RouteFinished;

        /// <summary>
        /// False until the first brain tick. Lets a route tell a fresh spawn apart from
        /// a mid-run route handoff, which must not snap the bot.
        /// </summary>
        public bool HasStarted => _hasStarted;

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            ConfigureAnimator();
            ConfigureRigidbody();

            _mover = new BotKinematicMover(transform, _animator, _rotationSpeed);
        }

        private void OnEnable() => _isActive = true;

        private void OnDisable() => _isActive = false;

        /// <summary>
        /// Spreads phases and lanes across bots with a low-discrepancy sequence, so five bots
        /// get five visibly different lines without any per-bot authoring.
        /// </summary>
        public void Initialize(AIBrain brain, int index)
        {
            _noisePhase = index * GOLDEN_ANGLE;
            _laneOffset = ((index * GOLDEN_RATIO_CONJUGATE % 1f) * 2f - 1f) * _maxLaneOffset;
            SetBrain(brain);
        }

        public void SetBrain(AIBrain brain) => _brain = brain;

        public void SetSpeedMultiplier(float multiplier) => _speedMultiplier = Mathf.Max(0f, multiplier);

        public void Tick(float deltaTime)
        {
            if (_brain == null)
                return;

            _brain.Tick(this, deltaTime);
            _hasStarted = true;

            if (!_brain.IsFinished || _hasRequestedRestart)
                return;

            _hasRequestedRestart = true;
            RouteFinished?.Invoke(this);
        }

        public bool TryRespawn() => _brain != null && _brain.TryRespawn(this);

        public void Restart(AIBrain brain)
        {
            _hasStarted = false;
            _hasRequestedRestart = false;
            _mover.SetLocomotionSpeed(0f);
            SetBrain(brain);
        }

        private void ConfigureAnimator()
        {
            if (_animator == null)
                return;

            _animator.applyRootMotion = false;
        }

        /// <summary>
        /// Bots are driven by the route, not by the solver, but they keep a kinematic body
        /// so respawn triggers still fire for them.
        /// </summary>
        private void ConfigureRigidbody()
        {
            if (!TryGetComponent(out Rigidbody body))
                return;

            body.isKinematic = true;
            body.useGravity = false;
            body.interpolation = RigidbodyInterpolation.None;
            body.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
}

using Code.Gameplay.PlayerLogic;
using Code.Gameplay.PlayerLogic.Input;
using Source.Scripts.Tutorial;
using UnityEngine;

public class PackshotController : MonoBehaviour
{
    private const float MoveInputDeadzone = 0.1f;
    private const float LookInputDeadzoneSqr = 0.0001f;

    [SerializeField] private TutorialManager _tutorialManager;
    [SerializeField] private GameObject _packshot;
    [SerializeField] private float _activateTime;

    private float _time;

    private void Start() { if (_tutorialManager) _tutorialManager.ButtonPressed += ResetTime; }
    private void OnDestroy() { if (_tutorialManager) _tutorialManager.ButtonPressed -= ResetTime; }
    private void Update()
    {
        if (HasPlayerMovement())
            ResetTime();

        if (_time >= _activateTime)
            ActivatePackhot();

        _time += Time.deltaTime;
    }
    public void ResetTime() => _time = 0f;

    public void Activate()
    {
        ResetTime();
        enabled = true;
    }
    private void ActivatePackhot() => _packshot.SetActive(true);
    private bool HasPlayerMovement()
    {
        GamePlayerContext context = GamePlayerContext.Instance;
        if (context == null)
            return false;

        IPlayerInputService input = context.InputService;
        if (input != null
            && (input.MoveInput.sqrMagnitude >= MoveInputDeadzone * MoveInputDeadzone
                || input.IsJumpPressed))
            return true;

        IPlayerLookInputService look = context.LookInputService;
        return look != null && look.LookDelta.sqrMagnitude >= LookInputDeadzoneSqr;
    }
}
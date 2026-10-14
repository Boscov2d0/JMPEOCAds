using System.Collections;
using System.Collections.Generic;
using Code.Gameplay.AI.Services;
using Code.Gameplay.PlayerLogic;
using Code.Gameplay.PlayerLogic.UI;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [SerializeField] private CanvasSelector _canvasSelector;
    [SerializeField] private GamePlayerJoystick _gamePlayerJoystick;
    [SerializeField] private PlayerHUDView _playerHUDView;
    [SerializeField] private Text _timerText;
    [SerializeField] private List<Transform> _doors;
    [SerializeField] private float _doorOpenSpeed = 90f;
    [SerializeField] private float _timeToStart = 5f;
    [SerializeField] private PackshotController _packshot;
    [SerializeField] private GameTimeController _gameTimer;
    [SerializeField] private AISpawnController[] _bots;

    [System.NonSerialized] private List<Text> _countdownTexts = new List<Text>();
    private float _timeLeft;
    private bool _isStarted;

    private void Awake()
    {
        _canvasSelector.ChangeCanvas += SetUIElement;

        SetPlayerControl(false);
        HoldPackshot();
        HoldBots();

        _timeLeft = _timeToStart;
        if (_countdownTexts == null)
            _countdownTexts = new List<Text>();

        CacheCountdownTexts();
    }
    private void SetUIElement(IActiveCanvas canvas) 
    {
        Text previous = _timerText;
        _playerHUDView = canvas as PlayerHUDView;
        _timerText = _playerHUDView != null ? _playerHUDView.StartGameTimeText : null;
        RememberCountdownText(_timerText);

        if (previous != null && previous != _timerText)
            HideCountdownText(previous);

        if (_timerText == null)
            return;

        if (_isStarted)
        {
            HideCountdownText(_timerText);
            ActivateTopPanel();
            return;
        }

        ShowCountdownText(_timerText, FormatCountdown(_timeLeft));
    }
    private void Update()
    {
        if (_isStarted)
            return;

        _timeLeft -= Time.deltaTime;
        SetTimerTexts(FormatCountdown(_timeLeft));

        if (_timeLeft > 0f)
            return;

        _isStarted = true;
        OpenDoors();
        SetPlayerControl(true);
        StartBots();
        ActivateFinger();
        ActivateTopPanel();
        ActivatePackshot();
        ActivateGameTimer();
        SetTimersActive(false);
    }

    private void CacheCountdownTexts()
    {
        if (_canvasSelector == null)
            return;

        _canvasSelector.CollectCountdownTexts(_countdownTexts);
    }

    private void RememberCountdownText(Text timer)
    {
        if (timer == null || _countdownTexts.Contains(timer))
            return;

        _countdownTexts.Add(timer);
    }

    private static string FormatCountdown(float timeLeft)
    {
        int seconds = Mathf.CeilToInt(timeLeft);
        if (seconds < 1)
            seconds = timeLeft > 0f ? 1 : 0;

        return seconds.ToString();
    }

    private void SetTimerTexts(string value)
    {
        if (_timerText != null && _timerText.gameObject.activeInHierarchy)
            _timerText.text = value;

        for (int i = 0; i < _countdownTexts.Count; i++)
        {
            Text timer = _countdownTexts[i];
            if (timer == null || timer == _timerText || !timer.gameObject.activeInHierarchy)
                continue;

            timer.text = value;
        }
    }

    private void SetTimersActive(bool isActive)
    {
        if (isActive)
        {
            ShowCountdownText(_timerText, FormatCountdown(_timeLeft));
            return;
        }

        HideCountdownText(_timerText);

        for (int i = 0; i < _countdownTexts.Count; i++)
            HideCountdownText(_countdownTexts[i]);
    }

    private static void ShowCountdownText(Text timer, string value)
    {
        if (timer == null)
            return;

        timer.gameObject.SetActive(true);
        timer.enabled = true;

        Color color = timer.color;
        color.a = 1f;
        timer.color = color;
        timer.text = value;
    }

    private static void HideCountdownText(Text timer)
    {
        if (timer == null || !timer.gameObject.activeSelf)
            return;

        timer.text = string.Empty;

        Color color = timer.color;
        color.a = 0f;
        timer.color = color;
        timer.enabled = false;
        timer.gameObject.SetActive(false);
    }

    private void OpenDoors()
    {
        for (int i = 0; i < _doors.Count; i++)
        {
            Transform door = _doors[i];
            if (door == null || door.childCount == 0)
                continue;

            StartCoroutine(OpenDoor(door.GetChild(0)));
        }
    }

    private IEnumerator OpenDoor(Transform door)
    {
        Quaternion target = door.localRotation * Quaternion.Euler(0f, 90f, 0f);
        if (_doorOpenSpeed <= 0f)
        {
            door.localRotation = target;
            yield break;
        }

        while (Quaternion.Angle(door.localRotation, target) > 0.1f)
        {
            door.localRotation = Quaternion.RotateTowards(
                door.localRotation,
                target,
                _doorOpenSpeed * Time.deltaTime);
            yield return null;
        }

        door.localRotation = target;
    }

    private void SetPlayerControl(bool isEnabled)
    {
        GamePlayerContext context = GamePlayerContext.Instance;
        if (context == null)
            context = FindObjectOfType<GamePlayerContext>();

        context?.SetControlEnabled(isEnabled);
    }

    private void HoldPackshot() => _packshot.enabled = false;
    private void ActivateFinger() => _gamePlayerJoystick.ActivateFingerAnimation();
    private void ActivateTopPanel() => _playerHUDView.TopPanel.SetActive(true);
    private void ActivatePackshot() => _packshot.Activate();
    private void ActivateGameTimer() => _gameTimer.Activate();
    private void HoldBots()
    {
        AISpawnController[] bots = ResolveBots();
        for (int i = 0; i < bots.Length; i++)
        {
            if (bots[i] != null)
                bots[i].SuppressAutoSpawn();
        }
    }

    private void StartBots()
    {
        AISpawnController[] bots = ResolveBots();
        for (int i = 0; i < bots.Length; i++)
        {
            if (bots[i] != null)
                bots[i].Spawn();
        }
    }

    private AISpawnController[] ResolveBots()
    {
        if (_bots != null && _bots.Length > 0)
            return _bots;

        _bots = FindObjectsOfType<AISpawnController>();
        return _bots;
    }
}
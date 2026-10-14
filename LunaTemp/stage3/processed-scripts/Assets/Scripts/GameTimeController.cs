using UnityEngine;
using UnityEngine.UI;

public class GameTimeController : MonoBehaviour
{
    [SerializeField] private CanvasSelector _canvasSelector;
    [SerializeField] private PlayerHUDView _playerHUDView;
    [SerializeField] private float _timeToEnd;
    [SerializeField] private GameObject _packshot;

    private Text _timeText;
    private float _currentTime;
    private bool _canStart;

    private void Awake()
    {
        _canvasSelector.ChangeCanvas += SetUIElement;
        _currentTime = _timeToEnd;
    }
    private void SetUIElement(IActiveCanvas canvas) 
    {
        _playerHUDView = canvas as PlayerHUDView;

        _timeText = _playerHUDView.TimeText;
        int minutes = Mathf.FloorToInt(_currentTime / 60f);
        int seconds = Mathf.FloorToInt(_currentTime % 60f);

        _timeText.text = $"{minutes:0}:{seconds:00}";
    }
    private void Update()
    {
        if (!_canStart)
            return;

        _currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(_currentTime / 60f);
        int seconds = Mathf.FloorToInt(_currentTime % 60f);

        _timeText.text = $"{minutes:0}:{seconds:00}";

        if (_currentTime <= 0)
            ActivatePackhot();
    }
    public void Activate() => _canStart = true;
    private void ActivatePackhot() => _packshot.SetActive(true);
}
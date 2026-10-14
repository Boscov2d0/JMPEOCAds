using UnityEngine;
using UnityEngine.UI;

public class SoundUIButton : MonoBehaviour
{
    [SerializeField] private Button _soundOnOffButton;
    [SerializeField] private Sprite[] _soundStateIcons;
    [SerializeField] private Image _soundStateImage;

    [SerializeField] private CanvasSelector _canvasSelector;
    private bool _isPlaying;

    private void Awake()
    {
        _canvasSelector = GetComponent<CanvasSelector>();
        _canvasSelector.ChangeCanvas += SetUIElements;
    }
    private void SetUIElements(IActiveCanvas canvas)
    {
        if (_soundOnOffButton)
            _soundOnOffButton.onClick.RemoveListener(SwitchAudioIcon);

        _soundOnOffButton = canvas.SoundButton;
        _soundStateImage = canvas.SoundImage;

        _soundOnOffButton.onClick.AddListener(SwitchAudioIcon);
    }
    private void OnDestroy() => _soundOnOffButton?.onClick.RemoveListener(SwitchAudioIcon);
    private void SwitchAudioIcon()
    {
        _isPlaying = !_isPlaying;
        int state = _isPlaying ? 0 : 1;
        _soundStateImage.sprite = _soundStateIcons[state];
    }
}
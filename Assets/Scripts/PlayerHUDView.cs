using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDView : MonoBehaviour, IActiveCanvas
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _joystickHandle;
    [SerializeField] private GameObject _finger;
    [SerializeField] private GameObject _joystick;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundButtonImage;
    [SerializeField] private GameObject _topPanel;
    [SerializeField] private Text _timeText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _startGameTimeText;

    public RectTransform Background { get => _background; }
    public RectTransform JoystickHandle { get => _joystickHandle; }
    public GameObject Finger { get => _finger; }
    public GameObject Joystick { get => _joystick; }
    public Button SoundButton { get => _soundButton; }
    public Image SoundImage { get => _soundButtonImage; }
    public GameObject TopPanel { get => _topPanel; }
    public Text TimeText { get => _timeText; }
    public Text ScoreText { get => _scoreText; }
    public Text StartGameTimeText { get => _startGameTimeText; }
}
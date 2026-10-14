using UnityEngine;

namespace Code.Gameplay.PlayerLogic.UI
{
    public class GamePlayerHUD : MonoBehaviour
    {
        [SerializeField] private GameObject _joystick;
        [SerializeField] private GameObject _jumpButton;
        [SerializeField] private bool _showControlsOnDesktop = true;

        private void Awake()
        {
            if (_joystick == null)
            {
                Transform joystick = transform.Find("Joystick");
                if (joystick != null)
                    _joystick = joystick.gameObject;
            }

            if (_jumpButton == null)
            {
                Transform jumpButton = transform.Find("JumpButton");
                if (jumpButton != null)
                    _jumpButton = jumpButton.gameObject;
            }
        }

        private void Start()
        {
            bool showMobileControls = _showControlsOnDesktop || Application.isMobilePlatform;

            if (_joystick != null)
                _joystick.SetActive(showMobileControls);

            if (_jumpButton != null)
                _jumpButton.SetActive(showMobileControls);
        }
    }
}

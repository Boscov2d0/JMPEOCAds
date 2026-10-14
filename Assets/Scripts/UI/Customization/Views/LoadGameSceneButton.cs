using Source.Scripts.UI.Customization.Appliers;
using Source.Scripts.UI.Customization.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.Scripts.UI.Customization.Views
{
    public class LoadGameSceneButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _sceneName = "Game";

        private void Awake() => _button.onClick.AddListener(LoadGame);

        private void OnDestroy() => _button.onClick.RemoveListener(LoadGame);

        private void LoadGame()
        {
            CaptureAppearance();
            SceneManager.LoadScene(_sceneName);
        }

        private static void CaptureAppearance()
        {
            CharacterAppearanceSource source = FindObjectOfType<CharacterAppearanceSource>();
            if (source != null)
            {
                source.Capture();
                return;
            }

            GameObject character = GameObject.Find("Character");
            if (character != null)
                CharacterAppearanceStore.CaptureFrom(character.transform);
        }
    }
}
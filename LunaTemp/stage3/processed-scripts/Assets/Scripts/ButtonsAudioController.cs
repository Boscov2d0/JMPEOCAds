using Source.Scripts.UI.Customization.Data;
using Source.Scripts.UI.Customization.Views;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private CustomizationScreenView _customizationScreen;
    [SerializeField] private CanvasSelector _canvasSelector;

    private readonly List<CustomizationScreenView> _screens = new List<CustomizationScreenView>();

    private void Awake() => _canvasSelector.ChangeCanvas += SetUIElements;
    private void SetUIElements(IActiveCanvas canvas)
    {
        CustomizationCanvasView customizationCanvas = canvas as CustomizationCanvasView;

        _customizationScreen = customizationCanvas.CustomizationScreenView;

        SubscribeScreens();
    }
    private void OnDisable()
    {
        for (int i = 0; i < _screens.Count; i++)
        {
            if (_screens[i] != null)
            {
                _screens[i].ItemSelected -= PlayClickSound;
                _screens[i].CategorySelected -= PlayClickSound;
            }
        }

        _screens.Clear();
    }

    private void SubscribeScreens()
    {
        if (_customizationScreen != null)
            Subscribe(_customizationScreen);

        CustomizationScreenView[] screens = Resources.FindObjectsOfTypeAll<CustomizationScreenView>();

        for (int i = 0; i < screens.Length; i++)
            Subscribe(screens[i]);
    }
    private void Subscribe(CustomizationScreenView screen)
    {
        if (screen == null || _screens.Contains(screen))
            return;

        screen.ItemSelected += PlayClickSound;
        screen.CategorySelected += PlayClickSound;
        _screens.Add(screen);
    }
    private void PlayClickSound(CustomizationCategory category, int i) => _clickSound.Play();
}
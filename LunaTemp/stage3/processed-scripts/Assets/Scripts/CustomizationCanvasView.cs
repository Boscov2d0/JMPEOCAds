using Source.Scripts.UI.Customization.Views;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationCanvasView : MonoBehaviour, IActiveCanvas
{
    [SerializeField] private Button _soundButton;
    [SerializeField] private Image _soundImage;
    [SerializeField] private RectTransform _characterSlot;
    [SerializeField] private bool _isLandscape;
    [SerializeField] private List<ButtonPulseDotween> _tutorialStepsButtons;
    [SerializeField] private CustomizationScreenView _customizationScreenView;

    public Button SoundButton { get => _soundButton; }
    public Image SoundImage { get => _soundImage; }
    public RectTransform CharacterSlot { get => _characterSlot; }
    public bool IsLandsckape { get => _isLandscape; }
    public List<ButtonPulseDotween> TutorialStepsButtons { get => _tutorialStepsButtons; }
    public CustomizationScreenView CustomizationScreenView { get => _customizationScreenView; }
}
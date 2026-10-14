using Source.Scripts.UI.Customization.Data;
using Source.Scripts.UI.Customization.Views;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private CustomizationScreenView _customizationScreen;
        [SerializeField] private List<ButtonPulseDotween> _tutorialStepsButtons = new List<ButtonPulseDotween>();
        [SerializeField] private CanvasSelector _canvasSelector;

        private readonly List<CustomizationScreenView> _screens = new List<CustomizationScreenView>();
        private CustomizationCategory _currentCategory = CustomizationCategory.Null;
        private int _currentCategoryIndex = 0;
        private int index;

        public Action ButtonPressed;

        private void Awake() => _canvasSelector.ChangeCanvas += SetElement;

        private void SetElement(IActiveCanvas canvas)
        {
            CustomizationCanvasView customizationCanvas = canvas as CustomizationCanvasView;

            _tutorialStepsButtons.Clear();

            _customizationScreen = customizationCanvas.CustomizationScreenView;

            for (int i = 0; i < customizationCanvas.TutorialStepsButtons.Count; i++)
                _tutorialStepsButtons.Add(customizationCanvas.TutorialStepsButtons[i]);

            SubscribeScreens();
            ApplyCurrentStep();
        }

        private void ApplyCurrentStep()
        {
            for (int i = 0; i < _tutorialStepsButtons.Count; i++)
            {
                ButtonPulseDotween button = _tutorialStepsButtons[i];
                if (button == null)
                    continue;

                if (i < index)
                    button.Complete();
                else if (i == index)
                    button.Activate();
                else
                    button.Deactivate();
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _screens.Count; i++)
            {
                if (_screens[i] != null)
                {
                    _screens[i].ItemSelected -= SelectItem;
                    _screens[i].CategorySelected -= SelectCategory;
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

            screen.ItemSelected += SelectItem;
            screen.CategorySelected += SelectCategory;
            _screens.Add(screen);
        }

        private void SelectItem(CustomizationCategory category, int i)
        {
            ButtonPressed?.Invoke();
            if (category == _currentCategory || (int)category <= (int)_currentCategory)
                return;

            _currentCategory = category;
            AdvanceStep();
        }

        private void SelectCategory(CustomizationCategory category, int i) 
        {
            ButtonPressed?.Invoke();
            if (i <= _currentCategoryIndex)
                return;

            _currentCategoryIndex = i;
            AdvanceStep();
        }

        private void AdvanceStep()
        {
            if (index >= 0 && index < _tutorialStepsButtons.Count && _tutorialStepsButtons[index] != null)
                _tutorialStepsButtons[index].Complete();

            index++;

            if (index >= 0 && index < _tutorialStepsButtons.Count && _tutorialStepsButtons[index] != null)
                _tutorialStepsButtons[index].Activate();
        }
    }
}
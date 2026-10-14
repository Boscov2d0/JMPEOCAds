using System;
using Source.Scripts.UI.Customization.Data;
using UnityEngine;
using Source.Scripts.UI.Customization.Handlers;
using Source.Scripts.UI.Customization.Models;
using Source.Scripts.UI.Customization.Views;

namespace Source.Scripts.UI.Customization.Presenters
{
    public class CustomizationScreenPresenter : IDisposable
    {
        private static readonly CustomizationCategory[] Categories =
        {
            CustomizationCategory.Skin,
            CustomizationCategory.Back,
            CustomizationCategory.Shoes,
            CustomizationCategory.Cap,
            CustomizationCategory.Glasses
        };

        private readonly CustomizationScreenModel _model;
        private readonly ICustomizationScreenView _view;
        private readonly ICustomizationItemHandler _itemHandler;
        private bool _isBound;

        public CustomizationScreenPresenter(
            CustomizationScreenModel model,
            ICustomizationScreenView view,
            ICustomizationItemHandler itemHandler)
        {
            _model = model;
            _view = view;
            _itemHandler = itemHandler;
        }

        public void Initialize()
        {
            if (_isBound)
                return;

            _view.CategorySelected += OnCategorySelected;
            _view.ItemSelected += OnItemSelected;
            _isBound = true;
            _model.RestoreRememberedCategory();
            Refresh();
            RestoreItemSelections();
        }

        public void Dispose()
        {
            if (!_isBound)
                return;

            _view.CategorySelected -= OnCategorySelected;
            _view.ItemSelected -= OnItemSelected;
            _isBound = false;
        }

        private void OnCategorySelected(CustomizationCategory category, int i)
        {
            if (category == CustomizationCategory.Null || !_model.TrySelectCategory(category))
                return;

            Refresh();
        }

        private void OnItemSelected(CustomizationCategory category, int itemIndex)
        {
            if (category == CustomizationCategory.Null)
                return;

            try
            {
                if (_model.SelectedCategory != category && _model.TrySelectCategory(category))
                    Refresh();

                _model.SelectItem(itemIndex);
                _itemHandler.HandleItemSelected(category, itemIndex);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }

        private void Refresh()
        {
            foreach (CustomizationCategory category in Categories)
            {
                bool isSelected = category == _model.SelectedCategory;
                _view.SetCategoryPanelActive(category, isSelected);
                _view.SetCategoryTabSelected(category, isSelected);
            }
        }

        private void RestoreItemSelections()
        {
            foreach (CustomizationCategory category in Categories)
            {
                if (_model.TryGetSelectedItemIndex(category, out int itemIndex))
                    _view.SetSelectedItem(category, itemIndex);
            }
        }
    }
}

using System;
using Source.Scripts.UI.Customization.Data;

namespace Source.Scripts.UI.Customization.Views
{
    public interface ICustomizationScreenView
    {
        event Action<CustomizationCategory, int> CategorySelected;
        event Action<CustomizationCategory, int> ItemSelected;

        void SetCategoryPanelActive(CustomizationCategory category, bool isActive);
        void SetCategoryTabSelected(CustomizationCategory category, bool isSelected);
        void SetSelectedItem(CustomizationCategory category, int itemIndex);
    }
}

using System.Collections.Generic;
using Source.Scripts.UI.Customization.Data;

namespace Source.Scripts.UI.Customization.Models
{
    public class CustomizationScreenModel
    {
        private static CustomizationCategory _rememberedCategory = CustomizationCategory.Null;

        private static readonly Dictionary<CustomizationCategory, int> _selectedItemByCategory = new Dictionary<CustomizationCategory, int>();

        public CustomizationCategory SelectedCategory { get; private set; }

        public CustomizationScreenModel(CustomizationCategory selectedCategory)
        {
            SelectedCategory = selectedCategory == CustomizationCategory.Null
                ? CustomizationCategory.Skin
                : selectedCategory;
        }

        public void RestoreRememberedCategory()
        {
            if (_rememberedCategory == CustomizationCategory.Null)
            {
                _rememberedCategory = SelectedCategory;
                return;
            }

            SelectedCategory = _rememberedCategory;
        }

        public bool TrySelectCategory(CustomizationCategory category)
        {
            if (category == CustomizationCategory.Null)
                return false;

            _rememberedCategory = category;

            if (SelectedCategory == category)
                return false;

            SelectedCategory = category;
            return true;
        }

        public void SelectItem(int itemIndex)
        {
            if (SelectedCategory == CustomizationCategory.Null)
                return;

            _selectedItemByCategory[SelectedCategory] = itemIndex;
        }

        public bool TryGetSelectedItemIndex(CustomizationCategory category, out int itemIndex)
        {
            return _selectedItemByCategory.TryGetValue(category, out itemIndex);
        }
    }
}

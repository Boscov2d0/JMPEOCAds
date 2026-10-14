using Source.Scripts.UI.Customization.Appliers;
using Source.Scripts.UI.Customization.Data;

namespace Source.Scripts.UI.Customization.Handlers
{
    public class CustomizationItemHandler : ICustomizationItemHandler
    {
        private readonly ICustomizationItemsCatalog _catalog;
        private readonly ICharacterMeshApplier _meshApplier;

        public CustomizationItemHandler(
            ICustomizationItemsCatalog catalog,
            ICharacterMeshApplier meshApplier)
        {
            _catalog = catalog;
            _meshApplier = meshApplier;
        }

        public void HandleItemSelected(CustomizationCategory category, int itemIndex)
        {
            if (category == CustomizationCategory.Null || _catalog == null || _meshApplier == null)
                return;

            if (!_catalog.TryGetChanges(category, itemIndex, out CharacterMeshChange[] changes))
                return;

            for (int i = 0; i < changes.Length; i++)
                _meshApplier.Apply(changes[i]);
        }
    }
}

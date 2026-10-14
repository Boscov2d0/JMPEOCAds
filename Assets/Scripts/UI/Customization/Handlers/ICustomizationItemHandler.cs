using Source.Scripts.UI.Customization.Data;

namespace Source.Scripts.UI.Customization.Handlers
{
    public interface ICustomizationItemHandler
    {
        void HandleItemSelected(CustomizationCategory category, int itemIndex);
    }
}

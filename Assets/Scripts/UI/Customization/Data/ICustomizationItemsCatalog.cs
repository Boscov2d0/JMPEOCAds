namespace Source.Scripts.UI.Customization.Data
{
    public interface ICustomizationItemsCatalog
    {
        bool TryGetChanges(CustomizationCategory category, int itemIndex, out CharacterMeshChange[] changes);
    }
}

using Source.Scripts.UI.Customization.Data;

namespace Source.Scripts.UI.Customization.Appliers
{
    public interface ICharacterMeshApplier
    {
        void Apply(CharacterMeshChange change);
    }
}

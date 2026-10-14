using Code.Gameplay.AI.Behaviours;

namespace Code.Gameplay.AI.Services
{
    public interface IBotTickService
    {
        void Register(AICharacter character);
        void Unregister(AICharacter character);
        void Clear();
    }
}

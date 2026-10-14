using Code.Gameplay.AI.Behaviours;

namespace Code.Gameplay.AI.Commands
{
    public interface IAIRespawnCommand
    {
        bool TryRespawn(AICharacter character);
    }
}

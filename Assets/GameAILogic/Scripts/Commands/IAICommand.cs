using Code.Gameplay.AI.Behaviours;

namespace Code.Gameplay.AI.Commands
{
    public interface IAICommand
    {
        void Enter(AICharacter character);
        AICommandStatus Tick(AICharacter character, float deltaTime);
    }
}

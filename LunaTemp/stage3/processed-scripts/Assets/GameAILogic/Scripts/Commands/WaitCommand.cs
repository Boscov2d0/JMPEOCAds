using Code.Gameplay.AI.Behaviours;

namespace Code.Gameplay.AI.Commands
{
    public class WaitCommand : IAICommand
    {
        private readonly float _waitTime;

        private float _timer;

        public WaitCommand(float waitTime) => _waitTime = waitTime;

        public void Enter(AICharacter character)
        {
            _timer = _waitTime;
            character.Mover.SetLocomotionSpeed(0f);
        }

        public AICommandStatus Tick(AICharacter character, float deltaTime)
        {
            _timer -= deltaTime;

            return _timer > 0f ? AICommandStatus.Running : AICommandStatus.Completed;
        }
    }
}

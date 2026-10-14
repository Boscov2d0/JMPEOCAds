using System;
using Code.Gameplay.AI.Behaviours;
using Code.Gameplay.AI.Commands;

namespace Code.Gameplay.AI.Brains
{
    public class AIBrain
    {
        private readonly IAICommand[] _commands;

        private int _index;
        private bool _isEntered;

        public AIBrain(params IAICommand[] commands) => _commands = commands ?? new IAICommand[0];

        public bool IsFinished => _index >= _commands.Length;

        public void Tick(AICharacter character, float deltaTime)
        {
            if (IsFinished)
                return;

            IAICommand command = _commands[_index];

            if (!_isEntered)
            {
                command.Enter(character);
                _isEntered = true;
            }

            if (command.Tick(character, deltaTime) != AICommandStatus.Completed)
                return;

            _index++;
            _isEntered = false;
        }

        public bool TryRespawn(AICharacter character)
        {
            if (IsFinished)
                return false;

            return _commands[_index] is IAIRespawnCommand respawnCommand && respawnCommand.TryRespawn(character);
        }
    }
}

using Code.Gameplay.AI.Factories;
using Code.Gameplay.AI.Waypoints;
using UnityEngine;

namespace Code.Gameplay.AI.Brains.Commands
{
    public class CommandAIBrainProvider : AIBrainProvider
    {
        [SerializeField] private AIWaypoint _waypoint;

        private IAIBrainFactory _brainFactory;

        public AIWaypoint Waypoint => _waypoint;

        public void Construct(IAIBrainFactory brainFactory)
        {
            _brainFactory = brainFactory;
        }

        public override AIBrain CreateAIBrain()
        {
            if (_brainFactory == null)
            {
                _brainFactory = GameAIContext.Instance != null
                    ? GameAIContext.Instance.BrainFactory
                    : new AIBrainFactory();
            }

            return _brainFactory.CreateCommandBrain(_waypoint);
        }
    }
}

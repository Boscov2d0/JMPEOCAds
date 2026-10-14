using Code.Gameplay.AI.Brains;
using Code.Gameplay.AI.Commands;
using Code.Gameplay.AI.Waypoints;

namespace Code.Gameplay.AI.Factories
{
    public class AIBrainFactory : IAIBrainFactory
    {
        public AIBrain CreateCommandBrain(AIWaypoint waypoint)
        {
            if (waypoint == null)
                return null;

            IAICommand command = waypoint.CreateCommand();

            return command == null ? null : new AIBrain(command);
        }
    }
}

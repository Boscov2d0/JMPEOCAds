using Code.Gameplay.AI.Brains;
using Code.Gameplay.AI.Waypoints;

namespace Code.Gameplay.AI.Factories
{
    public interface IAIBrainFactory
    {
        AIBrain CreateCommandBrain(AIWaypoint waypoint);
    }
}

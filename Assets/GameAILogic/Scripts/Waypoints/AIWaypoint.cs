using Code.Gameplay.AI.Commands;
using UnityEngine;

namespace Code.Gameplay.AI.Waypoints
{
    public abstract class AIWaypoint : MonoBehaviour
    {
        public abstract IAICommand CreateCommand();
    }
}

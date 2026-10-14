using Code.Gameplay.AI.Commands;
using Code.Gameplay.AI.Services;
using UnityEngine;

namespace Code.Gameplay.AI.Waypoints
{
    public class FieldPatrolWaypoint : AIWaypoint
    {
        [SerializeField] private BotFieldMap _fieldMap;

        public BotFieldMap FieldMap => _fieldMap != null ? _fieldMap : GetComponent<BotFieldMap>();

        public override IAICommand CreateCommand()
        {
            BotFieldMap map = _fieldMap != null ? _fieldMap : GetComponent<BotFieldMap>();
            return map != null ? new FieldPatrolCommand(map) : null;
        }
    }
}

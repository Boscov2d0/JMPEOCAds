using Code.Gameplay.AI.Commands;
using UnityEngine;

namespace Code.Gameplay.AI.Waypoints
{
    public class WaitWaypoint : AIWaypoint
    {
        [SerializeField][Min(0f)] private float _minTime = 0.25f;
        [SerializeField][Min(0f)] private float _maxTime = 1f;
        
        public override IAICommand CreateCommand()
        {
            var waitTime = Random.Range(_minTime, _maxTime);
            return new WaitCommand(waitTime);
        }
        
        private void OnValidate()
        {
            if (_minTime > _maxTime)
                _maxTime = _minTime;
        }
    }
}
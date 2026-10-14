using UnityEngine;

namespace Code.Gameplay.Common.Behaviours
{
    public interface ITransformPoint
    {
        Vector3 Position { get; }
        Transform Transform { get; }
        GameObject GameObject { get; }
    }
}

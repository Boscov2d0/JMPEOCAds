using UnityEngine;

namespace Source.Scripts.Orientation
{
    public abstract class OrientationHandler : MonoBehaviour, IOrientationHandler
    {
        protected virtual void OnEnable()
        {
            OrientationWatcher.Changed += Apply;
            Apply(OrientationWatcher.Current);
        }

        protected virtual void OnDisable()
        {
            OrientationWatcher.Changed -= Apply;
        }

        public void Apply(ScreenLayout layout)
        {
            if (layout == ScreenLayout.Portrait)
                ApplyPortrait();
            else
                ApplyLandscape();
        }

        protected abstract void ApplyPortrait();

        protected abstract void ApplyLandscape();
    }
}

using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Scripts.Orientation
{
    public class ObjectOrientationManager : OrientationHandler
    {
        [SerializeField] private GameObject _object;
        [SerializeField, FormerlySerializedAs("_lanscapePosition")] private Vector3 _landscapePosition;
        [SerializeField, FormerlySerializedAs("_portaitPosition")] private Vector3 _portraitPosition;
        [SerializeField, FormerlySerializedAs("_lanscapeScale")] private Vector3 _landscapeScale;
        [SerializeField, FormerlySerializedAs("_portaitScale")] private Vector3 _portraitScale;

        protected override void ApplyPortrait()
        {
            ApplyTransform(_portraitPosition, _portraitScale);
        }

        protected override void ApplyLandscape()
        {
            ApplyTransform(_landscapePosition, _landscapeScale);
        }

        private void ApplyTransform([Bridge.Ref] Vector3 position, [Bridge.Ref] Vector3 scale)
        {
            if (_object == null)
                return;

            if (_object.GetComponent<FitTransformToUiViewport>() != null)
                return;

            _object.transform.position = position;
            _object.transform.localScale = scale;
        }
    }
}

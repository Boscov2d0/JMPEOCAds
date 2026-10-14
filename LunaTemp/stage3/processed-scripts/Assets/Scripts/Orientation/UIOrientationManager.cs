using UnityEngine;

namespace Source.Scripts.Orientation
{
    public class UIOrientationManager : OrientationHandler
    {
        [SerializeField] private GameObject _canvasLandscape;
        [SerializeField] private GameObject _canvasPortrait;

        protected override void ApplyPortrait()
        {
            SetActive(_canvasLandscape, false);
            SetActive(_canvasPortrait, true);
        }

        protected override void ApplyLandscape()
        {
            SetActive(_canvasLandscape, true);
            SetActive(_canvasPortrait, false);
        }

        private static void SetActive(GameObject canvas, bool isActive)
        {
            if (canvas != null && canvas.activeSelf != isActive)
                canvas.SetActive(isActive);
        }
    }
}

using UnityEngine;

namespace Source.Scripts.Orientation
{
    [ExecuteAlways]
    public class UiAnchorPreset : MonoBehaviour
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxX = 1f;
        [SerializeField] private float _maxY = 1f;
        [SerializeField] private float _pivotX = 0.5f;
        [SerializeField] private float _pivotY = 0.5f;

        private void OnEnable()
        {
            Apply();
        }

        private void OnValidate() => Apply();

#if UNITY_EDITOR
        private void ApplyInEditor()
        {
            if (!Application.isPlaying)
                Apply();
        }
#endif

        public void Apply()
        {
            if (!(transform is RectTransform rect))
                return;

            rect.anchorMin = new Vector2(_minX, _minY);
            rect.anchorMax = new Vector2(_maxX, _maxY);
            rect.pivot = new Vector2(_pivotX, _pivotY);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
        }
    }
}

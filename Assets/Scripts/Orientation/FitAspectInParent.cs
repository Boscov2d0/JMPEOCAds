using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Source.Scripts.Orientation
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class FitAspectInParent : MonoBehaviour
    {
        [SerializeField] private float _aspect = 16f / 9f;

        private bool _applying;

        private void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += ApplyInEditor;
#endif
            Apply();
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall -= ApplyInEditor;
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall += ApplyInEditor;
        }

        private void ApplyInEditor()
        {
            if (this == null || Application.isPlaying)
                return;

            Apply();
        }
#endif

        private void LateUpdate()
        {
            if (Application.isPlaying)
                Apply();
        }

        public void Apply()
        {
            if (_applying || !(transform is RectTransform rect))
                return;

            RectTransform parent = rect.parent as RectTransform;
            if (parent == null)
                return;

            float parentWidth = parent.rect.width;
            float parentHeight = parent.rect.height;
            if (parentWidth <= 0.01f || parentHeight <= 0.01f || _aspect <= 0.01f)
                return;

            float parentAspect = parentWidth / parentHeight;
            float width;
            float height;
            if (parentAspect > _aspect)
            {
                height = parentHeight;
                width = height * _aspect;
            }
            else
            {
                width = parentWidth;
                height = width / _aspect;
            }

            Vector2 targetAnchor = new Vector2(0.5f, 0.5f);
            Vector2 targetSize = new Vector2(width, height);
            if (rect.anchorMin == targetAnchor &&
                rect.anchorMax == targetAnchor &&
                rect.pivot == targetAnchor &&
                rect.anchoredPosition == Vector2.zero &&
                (rect.sizeDelta - targetSize).sqrMagnitude < 0.01f &&
                rect.localScale == Vector3.one)
            {
                return;
            }

            _applying = true;
            rect.anchorMin = targetAnchor;
            rect.anchorMax = targetAnchor;
            rect.pivot = targetAnchor;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = targetSize;
            rect.localScale = Vector3.one;
            rect.localRotation = Quaternion.identity;
            _applying = false;
        }
    }
}

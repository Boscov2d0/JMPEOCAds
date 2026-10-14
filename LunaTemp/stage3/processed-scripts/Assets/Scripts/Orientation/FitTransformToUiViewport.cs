using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Source.Scripts.Orientation
{
    //[ExecuteAlways]
    [DefaultExecutionOrder(100)]
    public class FitTransformToUiViewport : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _landscapeViewport;
        [SerializeField] private RectTransform _portraitViewport;
        [SerializeField, Range(0.1f, 1f)] private float _fill = 0.92f;
        [SerializeField] private float _worldZ = 1f;
        [SerializeField] private Vector3 _worldOffset;

        [SerializeField] private CanvasSelector _canvasSelector;

        private readonly Vector3[] _worldCorners = new Vector3[8];
        private readonly Vector3[] _viewportCorners = new Vector3[4];

        private float _lastScreenWidth;
        private float _lastScreenHeight;
        private Vector3 _lastViewportSize;
        private bool _applying;
        private bool _ensuringCanvas;
        private bool _targetResolved;
        private Transform _resolvedTarget;
        private RectTransform _activeViewport;
#if UNITY_LUNA
        private const string IdleState = "Idle";

        private bool _hasLockedBounds;
        private Bounds _lockedLocalBounds;
        private bool _idleProxyReady;
        private Animator _visibleAnimator;
        private Transform _visibleRoot;
        private Transform _idleProxyRoot;
#endif

        private void OnEnable()
        {
            if (_canvasSelector == null)
                _canvasSelector = GetComponent<CanvasSelector>();

            if (_canvasSelector != null)
                _canvasSelector.ChangeCanvas += SetElement;

            _lastScreenWidth = 0;
            _targetResolved = false;
            _activeViewport = null;

            if (Application.isPlaying == false)
                Apply();
        }

        private void Awake()
        {
#if UNITY_LUNA
            EnsureIdleProxy();
#endif
        }

        private void OnDisable()
        {
            if (_canvasSelector != null)
                _canvasSelector.ChangeCanvas -= SetElement;
        }

        private void LateUpdate()
        {
#if UNITY_LUNA
            ApplyIdleProxy();
#endif
            Apply();
        }

        private void SetElement(IActiveCanvas canvas)
        {
            CustomizationCanvasView customizationCanvas = canvas as CustomizationCanvasView;
            if (customizationCanvas == null)
                return;

            _activeViewport = customizationCanvas.CharacterSlot;

            if (customizationCanvas.IsLandsckape)
                _landscapeViewport = customizationCanvas.CharacterSlot;
            else
                _portraitViewport = customizationCanvas.CharacterSlot;
        }

        public void Apply()
        {
            if (_applying)
                return;

            Transform target = ResolveTarget();
            if (target == null)
                return;

            Camera cam = _camera != null ? _camera : Camera.main;
            if (cam == null)
                return;

            if (_canvasSelector == null)
                _canvasSelector = GetComponent<CanvasSelector>();

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            bool screenChanged = screenWidth != _lastScreenWidth || screenHeight != _lastScreenHeight;

            if (Application.isPlaying == false && (screenChanged || _lastScreenWidth == 0))
                EnsureCanvas();

            SyncFromActiveCanvas();

            if (screenChanged || _lastScreenWidth == 0)
                Canvas.ForceUpdateCanvases();

            RectTransform viewport = SelectViewport();
            if (viewport == null)
                return;

            Vector3 viewportSize = viewport.rect.size;
            bool viewportChanged = (viewportSize - _lastViewportSize).sqrMagnitude > 0.01f;
            if (screenChanged == false && viewportChanged == false && Application.isPlaying == false && _lastScreenWidth != 0)
                return;

            if (TryGetLocalBounds(target, out Bounds localBounds) == false)
                return;

            viewport.GetWorldCorners(_viewportCorners);
            if (TryGetSlotLine(cam, out Vector3 worldBottom, out Vector3 worldTop) == false)
                return;

            float viewportHeight = Mathf.Abs(worldTop.y - worldBottom.y);
            if (viewportHeight <= 0.0001f || localBounds.size.y <= 0.0001f)
                return;

            float scale = viewportHeight * _fill / localBounds.size.y;
            Vector3 localFeet = new Vector3(localBounds.center.x, localBounds.min.y, localBounds.center.z);
            Vector3 feetOffset = target.rotation * (localFeet * scale);

            Vector3 position = worldBottom - feetOffset + _worldOffset;
            position.z = _worldZ;

            _applying = true;
            target.position = position;
            target.localScale = Vector3.one * scale;
            _applying = false;

            _lastScreenWidth = screenWidth;
            _lastScreenHeight = screenHeight;
            _lastViewportSize = viewportSize;
        }

#if UNITY_LUNA
        private void EnsureIdleProxy()
        {
            if (_idleProxyReady || Application.isPlaying == false)
                return;

            Transform fitTarget = _target != null ? _target : null;
            if (fitTarget == null)
                return;

            Animator animator = fitTarget.GetComponentInChildren<Animator>(true);
            if (animator == null)
                return;

            _idleProxyReady = true;
            _visibleAnimator = animator;
            _visibleRoot = animator.transform;

            GameObject proxy = Instantiate(animator.gameObject);
            proxy.name = "IdleProxy";
            proxy.transform.SetParent(null, false);
            proxy.transform.position = new Vector3(0f, -500f, 0f);

            Animator proxyAnimator = proxy.GetComponent<Animator>();
            if (proxyAnimator == null)
                proxyAnimator = proxy.GetComponentInChildren<Animator>(true);

            if (proxyAnimator != null)
            {
                proxyAnimator.applyRootMotion = false;
                proxyAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                proxyAnimator.enabled = true;
                if (proxyAnimator.runtimeAnimatorController != null)
                    proxyAnimator.Play(IdleState, 0, 0f);
            }

            Renderer[] proxyRenderers = proxy.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < proxyRenderers.Length; i++)
            {
                if (proxyRenderers[i] != null)
                    proxyRenderers[i].enabled = false;
            }

            animator.applyRootMotion = false;
            animator.enabled = false;
            _idleProxyRoot = proxy.transform;
        }

        private void ApplyIdleProxy()
        {
            EnsureIdleProxy();
            if (_visibleAnimator != null && _visibleAnimator.enabled)
                _visibleAnimator.enabled = false;

            if (_idleProxyRoot == null || _visibleRoot == null)
                return;

            CopyLocalPose(_idleProxyRoot, _visibleRoot);
        }

        private static void CopyLocalPose(Transform source, Transform destination)
        {
            int sourceCount = source.childCount;
            for (int i = 0; i < sourceCount; i++)
            {
                Transform sourceChild = source.GetChild(i);
                Transform destinationChild = FindNamedChild(destination, sourceChild.name);
                if (destinationChild == null)
                    continue;

                destinationChild.localPosition = sourceChild.localPosition;
                destinationChild.localRotation = sourceChild.localRotation;
                destinationChild.localScale = sourceChild.localScale;
                CopyLocalPose(sourceChild, destinationChild);
            }
        }

        private static Transform FindNamedChild(Transform parent, string childName)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == childName)
                    return child;
            }

            return null;
        }
#endif

        private void EnsureCanvas()
        {
            if (_canvasSelector == null || _ensuringCanvas)
                return;

            _ensuringCanvas = true;
            _canvasSelector.UpdateCanvas();
            _ensuringCanvas = false;
        }

        private void SyncFromActiveCanvas()
        {
            if (_canvasSelector == null)
                return;

            IActiveCanvas activeCanvas = _canvasSelector.ActiveCanvas;
            if (activeCanvas != null)
                SetElement(activeCanvas);
        }

        private Transform ResolveTarget()
        {
            if (_targetResolved)
                return _resolvedTarget;

            _targetResolved = true;

            if (_target != null && _target != transform && HasRenderable(_target))
                _resolvedTarget = _target;
            else
            {
                GameObject character = GameObject.Find("Character");
                if (character != null && HasRenderable(character.transform))
                    _resolvedTarget = character.transform;
            }

#if UNITY_EDITOR
            if (Application.isPlaying == false && _resolvedTarget != null && _target != _resolvedTarget)
            {
                _target = _resolvedTarget;
                EditorUtility.SetDirty(this);
            }
#endif

            return _resolvedTarget;
        }

        private static bool HasRenderable(Transform candidate)
        {
            Renderer[] renderers = candidate.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer != null && renderer is ParticleSystemRenderer == false)
                    return true;
            }

            return false;
        }

        private RectTransform SelectViewport()
        {
            if (IsUsableViewport(_activeViewport))
                return _activeViewport;

            bool isPortrait = Screen.height >= Screen.width;
            RectTransform cached = isPortrait ? _portraitViewport : _landscapeViewport;
            if (IsUsableViewport(cached))
                return cached;

            CustomizationCanvasView[] views = FindObjectsOfType<CustomizationCanvasView>();
            for (int i = 0; i < views.Length; i++)
            {
                CustomizationCanvasView view = views[i];
                if (view != null && IsUsableViewport(view.CharacterSlot))
                {
                    SetElement(view);
                    return _activeViewport;
                }
            }

            return null;
        }

        private static bool IsUsableViewport(RectTransform viewport)
        {
            if (viewport == null || viewport.gameObject.activeInHierarchy == false)
                return false;

            return viewport.lossyScale.sqrMagnitude > 0.000001f;
        }

        private bool TryGetSlotLine(Camera cam, out Vector3 worldBottom, out Vector3 worldTop)
        {
            Transform cameraTransform = cam.transform;
            Vector3 cameraPosition = cameraTransform.position;
            Vector3 right = cameraTransform.right;
            Vector3 up = cameraTransform.up;
            Vector3 forward = cameraTransform.forward;

            Vector3 bottomMid = (_viewportCorners[0] + _viewportCorners[3]) * 0.5f;
            Vector3 topMid = (_viewportCorners[1] + _viewportCorners[2]) * 0.5f;
            Vector3 slotMid = (bottomMid + topMid) * 0.5f;

            float centerX = Vector3.Dot(slotMid - cameraPosition, right);
            float bottomY = Vector3.Dot(bottomMid - cameraPosition, up);
            float topY = Vector3.Dot(topMid - cameraPosition, up);
            float depth = Vector3.Dot(forward, new Vector3(0f, 0f, _worldZ) - cameraPosition);
            Vector3 planeOrigin = cameraPosition + forward * depth;

            worldBottom = planeOrigin + right * centerX + up * bottomY;
            worldTop = planeOrigin + right * centerX + up * topY;
            worldBottom.z = _worldZ;
            worldTop.z = _worldZ;
            return true;
        }

        private bool TryGetLocalBounds(Transform root, out Bounds localBounds)
        {
#if UNITY_LUNA
            if (_hasLockedBounds)
            {
                localBounds = _lockedLocalBounds;
                return true;
            }
#endif
            bool hasMeshBounds = TryGetMeshLocalBounds(root, out Bounds meshLocalBounds);
#if UNITY_LUNA
            if (hasMeshBounds)
            {
                localBounds = meshLocalBounds;
                TryLockBounds(localBounds);
                return true;
            }
#endif
            if (TryGetRendererLocalBounds(root, out localBounds))
            {
#if UNITY_LUNA
                TryLockBounds(localBounds);
#endif
                return true;
            }

            localBounds = meshLocalBounds;
            return hasMeshBounds;
        }

#if UNITY_LUNA
        private void TryLockBounds([Bridge.Ref] Bounds bounds)
        {
            float height = Mathf.Max(bounds.size.y, bounds.extents.y * 2f);
            if (height < 0.05f || height > 30f)
                return;

            _lockedLocalBounds = bounds;
            _hasLockedBounds = true;
        }
#endif

        private bool TryGetMeshLocalBounds(Transform root, out Bounds localBounds)
        {
            localBounds = default(UnityEngine.Bounds);
            bool hasBounds = false;
            SkinnedMeshRenderer[] skinnedRenderers = root.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < skinnedRenderers.Length; i++)
            {
                SkinnedMeshRenderer renderer = skinnedRenderers[i];
                if (renderer == null || renderer.enabled == false || IsCapSlot(renderer.transform))
                    continue;
                Mesh mesh = renderer.sharedMesh;
                if (mesh == null)
                    continue;
                Bounds meshBounds = mesh.bounds;
                Vector3 extents = meshBounds.extents;
                if (extents.sqrMagnitude <= 0.000001f && meshBounds.size.sqrMagnitude <= 0.000001f)
                    continue;
                EncapsulateLocalBounds(root, renderer.transform, meshBounds, ref localBounds, ref hasBounds);
            }
            return hasBounds;
        }
        private bool TryGetRendererLocalBounds(Transform root, out Bounds localBounds)
        {
            localBounds = default(UnityEngine.Bounds);
            bool hasBounds = false;
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null || renderer.enabled == false || renderer is ParticleSystemRenderer || IsCapSlot(renderer.transform))
                    continue;
                EncapsulateWorldBounds(root, renderer.bounds, ref localBounds, ref hasBounds);
            }
            return hasBounds;
        }
        private static bool IsCapSlot(Transform slot)
        {
            string name = slot.name;
            return name == "Hat" || name == "Hair";
        }

        private void EncapsulateLocalBounds(
            Transform root,
            Transform meshTransform,
[Bridge.Ref]             Bounds meshBounds,
            ref Bounds localBounds,
            ref bool hasBounds)
        {
            Vector3 center = meshBounds.center;
            Vector3 extents = meshBounds.extents;
            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    for (int z = -1; z <= 1; z += 2)
                    {
                        Vector3 corner = center + Vector3.Scale(extents, new Vector3(x, y, z));
                        Vector3 local = root.InverseTransformPoint(meshTransform.TransformPoint(corner));
                        Encapsulate(ref localBounds, ref hasBounds, local);
                    }
                }
            }
        }

        private void EncapsulateWorldBounds(Transform root, [Bridge.Ref] Bounds world, ref Bounds localBounds, ref bool hasBounds)
        {
            Vector3 min = world.min;
            Vector3 max = world.max;
            _worldCorners[0] = new Vector3(min.x, min.y, min.z);
            _worldCorners[1] = new Vector3(min.x, min.y, max.z);
            _worldCorners[2] = new Vector3(min.x, max.y, min.z);
            _worldCorners[3] = new Vector3(min.x, max.y, max.z);
            _worldCorners[4] = new Vector3(max.x, min.y, min.z);
            _worldCorners[5] = new Vector3(max.x, min.y, max.z);
            _worldCorners[6] = new Vector3(max.x, max.y, min.z);
            _worldCorners[7] = new Vector3(max.x, max.y, max.z);

            for (int corner = 0; corner < 8; corner++)
                Encapsulate(ref localBounds, ref hasBounds, root.InverseTransformPoint(_worldCorners[corner]));
        }

        private static void Encapsulate(ref Bounds localBounds, ref bool hasBounds, [Bridge.Ref] Vector3 local)
        {
            if (hasBounds == false)
            {
                localBounds = new Bounds(local, Vector3.zero);
                hasBounds = true;
                return;
            }

            localBounds.Encapsulate(local);
        }
    }
}
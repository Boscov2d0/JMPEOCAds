using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.UI.Customization.Data
{
    [Serializable]
    public class CustomizationItemsCatalog : ICustomizationItemsCatalog
    {
        private const int ItemCount = 8;

        private static readonly Dictionary<int, SourceBinding> BindingCache = new Dictionary<int, SourceBinding>();

        [SerializeField] private GameObject[] _skinMeshes = new GameObject[ItemCount];
        [SerializeField] private GameObject[] _backMeshes = new GameObject[ItemCount];
        [SerializeField] private GameObject[] _shoeMeshes = new GameObject[ItemCount];
        [SerializeField] private GameObject[] _capHairMeshes = new GameObject[ItemCount];
        [SerializeField] private GameObject[] _capHatMeshes = new GameObject[ItemCount];
        [SerializeField] private GameObject[] _glassesMeshes = new GameObject[ItemCount];

        public void EnsureSize()
        {
            Resize(ref _skinMeshes);
            Resize(ref _backMeshes);
            Resize(ref _shoeMeshes);
            Resize(ref _capHairMeshes);
            Resize(ref _capHatMeshes);
            Resize(ref _glassesMeshes);
        }

        public bool TryGetChanges(CustomizationCategory category, int itemIndex, out CharacterMeshChange[] changes)
        {
            switch (category)
            {
                case CustomizationCategory.Null:
                    changes = new CharacterMeshChange[0];
                    return false;
                case CustomizationCategory.Skin:
                    return TryCreateSingle(CharacterPart.Body, _skinMeshes, itemIndex, out changes);
                case CustomizationCategory.Back:
                    return TryCreateSingle(CharacterPart.Backpack, _backMeshes, itemIndex, out changes);
                case CustomizationCategory.Shoes:
                    return TryCreateSingle(CharacterPart.Shoe, _shoeMeshes, itemIndex, out changes);
                case CustomizationCategory.Glasses:
                    return TryCreateSingle(CharacterPart.Glasses, _glassesMeshes, itemIndex, out changes);
                case CustomizationCategory.Cap:
                    if (!IsValidIndex(_capHairMeshes, itemIndex) && !IsValidIndex(_capHatMeshes, itemIndex))
                    {
                        changes = new CharacterMeshChange[0];
                        return false;
                    }

                    changes = new[]
                    {
                        CreateChange(CharacterPart.Hair, GetSource(_capHairMeshes, itemIndex)),
                        CreateChange(CharacterPart.Hat, GetSource(_capHatMeshes, itemIndex))
                    };
                    return true;
                default:
                    changes = new CharacterMeshChange[0];
                    return false;
            }
        }

        private static bool TryCreateSingle(
            CharacterPart part,
            GameObject[] sources,
            int itemIndex,
            out CharacterMeshChange[] changes)
        {
            if (!IsValidIndex(sources, itemIndex))
            {
                changes = new CharacterMeshChange[0];
                return false;
            }

            changes = new[] { CreateChange(part, sources[itemIndex]) };
            return true;
        }

        private static CharacterMeshChange CreateChange(CharacterPart part, GameObject source)
        {
            if (source == null)
                return CharacterMeshChange.Unequipped(part);

            if (TryGetBinding(source, out SourceBinding binding) == false)
                return new CharacterMeshChange(part, null, null, null, null, false, source);

            return new CharacterMeshChange(
                part,
                binding.Mesh,
                binding.Materials,
                binding.BoneNames,
                binding.RootBoneName,
                false,
                source);
        }

        private static GameObject GetSource(GameObject[] sources, int itemIndex)
        {
            return IsValidIndex(sources, itemIndex) ? sources[itemIndex] : null;
        }

        private static bool TryGetBinding(GameObject source, out SourceBinding binding)
        {
            binding = default;
            if (source == null)
                return false;

            int key = source.GetInstanceID();
            if (BindingCache.TryGetValue(key, out binding))
                return binding.Mesh != null;

            if (TryCapture(FindSkinnedRenderer(source), out binding) == false)
                binding = CaptureInstance(source);

            if (binding.Mesh == null)
                return false;

            BindingCache[key] = binding;
            return true;
        }

        private static SourceBinding CaptureInstance(GameObject source)
        {
            GameObject instance = UnityEngine.Object.Instantiate(source);
            instance.transform.position = new Vector3(0f, -1000f, 0f);
            instance.SetActive(false);

            TryCapture(FindSkinnedRenderer(instance), out SourceBinding binding);
            UnityEngine.Object.Destroy(instance);
            return binding;
        }

        private static bool TryCapture(SkinnedMeshRenderer renderer, out SourceBinding binding)
        {
            binding = default;
            if (renderer == null || renderer.sharedMesh == null || renderer.bones == null || renderer.bones.Length == 0)
                return false;

            Transform[] bones = renderer.bones;
            var boneNames = new string[bones.Length];
            for (int i = 0; i < bones.Length; i++)
            {
                if (bones[i] == null)
                    return false;

                boneNames[i] = bones[i].name;
            }

            binding = new SourceBinding
            {
                Mesh = renderer.sharedMesh,
                Materials = renderer.sharedMaterials,
                BoneNames = boneNames,
                RootBoneName = renderer.rootBone != null ? renderer.rootBone.name : null
            };
            return true;
        }

        private static SkinnedMeshRenderer FindSkinnedRenderer(GameObject source)
        {
            if (source == null)
                return null;

            SkinnedMeshRenderer[] skinnedRenderers = source.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            for (int i = 0; i < skinnedRenderers.Length; i++)
            {
                SkinnedMeshRenderer renderer = skinnedRenderers[i];
                if (renderer != null && renderer.sharedMesh != null)
                    return renderer;
            }

            return null;
        }

        private struct SourceBinding
        {
            public Mesh Mesh;
            public Material[] Materials;
            public string[] BoneNames;
            public string RootBoneName;
        }

        private static bool IsValidIndex(GameObject[] sources, int itemIndex)
        {
            return sources != null && itemIndex >= 0 && itemIndex < sources.Length;
        }

        private static void Resize(ref GameObject[] sources)
        {
            if (sources != null && sources.Length == ItemCount)
                return;

            var resized = new GameObject[ItemCount];
            if (sources != null)
                Array.Copy(sources, resized, Math.Min(sources.Length, ItemCount));

            sources = resized;
        }
    }
}

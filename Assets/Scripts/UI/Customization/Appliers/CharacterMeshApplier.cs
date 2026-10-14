using System.Collections.Generic;
using Source.Scripts.UI.Customization.Data;
using UnityEngine;

namespace Source.Scripts.UI.Customization.Appliers
{
    public class CharacterMeshApplier : ICharacterMeshApplier
    {
        private readonly Transform _skeletonRoot;
        private readonly IReadOnlyDictionary<CharacterPart, SkinnedMeshRenderer> _renderers;

        public CharacterMeshApplier(IReadOnlyDictionary<CharacterPart, SkinnedMeshRenderer> renderers, Transform skeletonRoot)
        {
            _renderers = renderers;
            _skeletonRoot = skeletonRoot;
        }

        public static CharacterMeshApplier FromCharacter(Transform characterRoot)
        {
            var renderers = new Dictionary<CharacterPart, SkinnedMeshRenderer>
            {
                [CharacterPart.Body] = FindRenderer(characterRoot, "Body"),
                [CharacterPart.Backpack] = FindRenderer(characterRoot, "Backpack"),
                [CharacterPart.Shoe] = FindRenderer(characterRoot, "Shoe"),
                [CharacterPart.Hair] = FindRenderer(characterRoot, "Hair"),
                [CharacterPart.Hat] = FindRenderer(characterRoot, "Hat"),
                [CharacterPart.Glasses] = FindRenderer(characterRoot, "Glasses")
            };

            foreach (KeyValuePair<CharacterPart, SkinnedMeshRenderer> pair in renderers)
            {
                if (pair.Value != null && pair.Value.sharedMesh != null)
                    CharacterSkinBinder.KeepBinding(pair.Value);
            }

            return new CharacterMeshApplier(renderers, characterRoot);
        }

        public void Apply(CharacterMeshChange change)
        {
            if (!_renderers.TryGetValue(change.Part, out SkinnedMeshRenderer renderer) || renderer == null)
            {
                Debug.LogWarning($"SkinnedMeshRenderer for {change.Part} was not found.");
                return;
            }

            if (change.Unequip)
            {
                CharacterSkinBinder.ClearSlot(renderer);
                return;
            }

            if (change.Mesh == null)
                return;

            CharacterSkinBinder.Apply(
                renderer,
                _skeletonRoot,
                change.Mesh,
                change.Materials,
                change.BoneNames,
                change.RootBoneName);
        }

        private static SkinnedMeshRenderer FindRenderer(Transform characterRoot, string childName)
        {
            if (characterRoot == null)
                return null;

            Transform child = characterRoot.Find(childName);
            if (child == null)
            {
                Debug.LogError($"Child '{childName}' was not found under '{characterRoot.name}'.");
                return null;
            }

            // Luna drops mesh-less SkinnedMeshRenderers on export and cannot AddComponent them at runtime,
            // so every slot must ship with a renderer that has a placeholder mesh.
            SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null)
                Debug.LogError($"Slot '{childName}' under '{characterRoot.name}' has no SkinnedMeshRenderer with a mesh.");

            return renderer;
        }
    }
}

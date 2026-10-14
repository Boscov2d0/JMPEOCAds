using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.UI.Customization.Appliers
{
    public static class CharacterSkinBinder
    {
        public static void ClearSlot(SkinnedMeshRenderer renderer)
        {
            if (renderer == null)
                return;

            renderer.sharedMesh = null;
            renderer.enabled = false;
        }

        public static void KeepBinding(SkinnedMeshRenderer renderer)
        {
            if (renderer == null)
                return;

            Transform[] bones = renderer.bones;
            if (bones != null && bones.Length > 0)
                renderer.bones = bones;

            Transform rootBone = renderer.rootBone;
            if (rootBone != null)
                renderer.rootBone = rootBone;
        }

        public static void Apply(
            SkinnedMeshRenderer renderer,
            Transform skeletonRoot,
            Mesh mesh,
            Material[] materials,
            string[] boneNames,
            string rootBoneName)
        {
            if (renderer == null)
                return;

            if (mesh == null)
            {
                ClearSlot(renderer);
                return;
            }

            BindBones(renderer, skeletonRoot, boneNames, rootBoneName);
            renderer.sharedMesh = mesh;

            if (materials != null && materials.Length > 0)
                renderer.sharedMaterials = materials;

            renderer.enabled = true;
        }

        private static void BindBones(
            SkinnedMeshRenderer renderer,
            Transform skeletonRoot,
            string[] boneNames,
            string rootBoneName)
        {
            if (renderer == null || skeletonRoot == null || boneNames == null || boneNames.Length == 0)
                return;

            Dictionary<string, Transform> bonesByName = BuildLookup(skeletonRoot);
            if (TryMapBones(boneNames, bonesByName, skeletonRoot, out Transform[] mappedBones) == false)
                return;

            renderer.bones = mappedBones;

            if (string.IsNullOrEmpty(rootBoneName) == false
                && bonesByName.TryGetValue(rootBoneName, out Transform rootBone))
                renderer.rootBone = rootBone;
        }

        public static string[] CopyBoneNames(SkinnedMeshRenderer renderer)
        {
            if (renderer == null)
                return null;

            Transform[] bones = renderer.bones;
            if (bones == null || bones.Length == 0)
                return null;

            var names = new string[bones.Length];
            for (int i = 0; i < bones.Length; i++)
                names[i] = bones[i] != null ? bones[i].name : null;

            return names;
        }

        public static string CopyRootBoneName(SkinnedMeshRenderer renderer)
        {
            if (renderer == null || renderer.rootBone == null)
                return null;

            return renderer.rootBone.name;
        }

        private static bool TryMapBones(
            string[] boneNames,
            Dictionary<string, Transform> bonesByName,
            Transform skeletonRoot,
            out Transform[] mappedBones)
        {
            mappedBones = new Transform[boneNames.Length];
            Transform fallback = bonesByName.TryGetValue("Head", out Transform head) ? head : skeletonRoot;

            for (int i = 0; i < boneNames.Length; i++)
            {
                string boneName = boneNames[i];
                if (string.IsNullOrEmpty(boneName) == false
                    && bonesByName.TryGetValue(boneName, out Transform bone))
                {
                    mappedBones[i] = bone;
                    continue;
                }

                mappedBones[i] = fallback;
            }

            return mappedBones.Length > 0 && fallback != null;
        }

        private static Dictionary<string, Transform> BuildLookup(Transform skeletonRoot)
        {
            var bonesByName = new Dictionary<string, Transform>();
            Transform[] transforms = skeletonRoot.GetComponentsInChildren<Transform>(true);

            for (int i = 0; i < transforms.Length; i++)
            {
                Transform bone = transforms[i];
                if (bone == null || bonesByName.ContainsKey(bone.name))
                    continue;

                bonesByName.Add(bone.name, bone);
            }

            return bonesByName;
        }
    }
}

using UnityEngine;

namespace Source.Scripts.UI.Customization.Data
{
    public readonly struct CharacterMeshChange
    {
        public CharacterMeshChange(
            CharacterPart part,
            Mesh mesh,
            Material[] materials,
            string[] boneNames,
            string rootBoneName,
            bool unequip,
            GameObject source = null)
        {
            Part = part;
            Mesh = mesh;
            Materials = materials;
            BoneNames = boneNames;
            RootBoneName = rootBoneName;
            Unequip = unequip;
            Source = source;
        }

        public static CharacterMeshChange Unequipped(CharacterPart part)
        {
            return new CharacterMeshChange(part, null, null, null, null, true);
        }

        public CharacterPart Part { get; }
        public Mesh Mesh { get; }
        public Material[] Materials { get; }
        public string[] BoneNames { get; }
        public string RootBoneName { get; }
        public bool Unequip { get; }
        public GameObject Source { get; }
    }
}

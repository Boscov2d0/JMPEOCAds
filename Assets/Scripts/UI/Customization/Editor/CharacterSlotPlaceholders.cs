using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts.UI.Customization.Editor
{
    // Luna drops SkinnedMeshRenderers without a mesh on export and cannot add them at runtime,
    // so empty customization slots keep a disabled renderer with a placeholder mesh.
    [InitializeOnLoad]
    internal static class CharacterSlotPlaceholders
    {
        private const string PlayerPrefabPath = "Assets/GamePlayerLogic/Prefabs/Player.prefab";

        private static readonly (string SlotName, string MeshPath)[] Slots =
        {
            ("Backpack", "Assets/Funny_Characters/Meshes/Backpack/Backpack_013.fbx"),
            ("Hat", "Assets/Funny_Characters/Meshes/Hats/Hat_095.fbx"),
            ("Glasses", "Assets/Funny_Characters/Meshes/Glasses/Glasses_024.fbx")
        };

        static CharacterSlotPlaceholders()
        {
            EditorApplication.delayCall += FillAll;
        }

        [MenuItem("Tools/Customization/Fill Empty Character Slots")]
        private static void FillAll()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            FillOpenScenes();
            FillPrefab(PlayerPrefabPath);
        }

        private static void FillOpenScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                    continue;

                bool changed = false;
                foreach (GameObject root in scene.GetRootGameObjects())
                    changed |= FillHierarchy(root.transform);

                if (changed)
                    EditorSceneManager.MarkSceneDirty(scene);
            }
        }

        private static void FillPrefab(string path)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(path) == null)
                return;

            GameObject contents = PrefabUtility.LoadPrefabContents(path);
            try
            {
                if (FillHierarchy(contents.transform))
                    PrefabUtility.SaveAsPrefabAsset(contents, path);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(contents);
            }
        }

        private static bool FillHierarchy(Transform root)
        {
            bool changed = false;
            SkinnedMeshRenderer[] renderers = root.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                SkinnedMeshRenderer renderer = renderers[i];
                if (renderer.sharedMesh != null)
                    continue;

                Mesh placeholder = FindPlaceholder(renderer.gameObject.name);
                if (placeholder == null)
                    continue;

                Undo.RecordObject(renderer, "Fill Empty Character Slot");
                renderer.sharedMesh = placeholder;
                renderer.enabled = false;
                EditorUtility.SetDirty(renderer);
                changed = true;
            }

            return changed;
        }

        private static Mesh FindPlaceholder(string slotName)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i].SlotName != slotName)
                    continue;

                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(Slots[i].MeshPath);
                for (int j = 0; j < assets.Length; j++)
                {
                    if (assets[j] is Mesh mesh)
                        return mesh;
                }
            }

            return null;
        }
    }
}

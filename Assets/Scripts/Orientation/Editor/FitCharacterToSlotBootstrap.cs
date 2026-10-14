using UnityEditor;
using UnityEngine;

namespace Source.Scripts.Orientation.Editor
{
    [InitializeOnLoad]
    internal static class FitCharacterToSlotBootstrap
    {
        static FitCharacterToSlotBootstrap()
        {
            EditorApplication.delayCall += AttachIfNeeded;
        }

        private static void AttachIfNeeded()
        {
            if (Application.isPlaying)
                return;

            FitTransformToUiViewport existing = Object.FindFirstObjectByType<FitTransformToUiViewport>();
            if (existing != null)
                return;

            GameObject character = GameObject.Find("Character");
            if (character == null)
                return;

            bool hasLandscapeSlot = false;
            RectTransform[] rects = Object.FindObjectsByType<RectTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < rects.Length; i++)
            {
                if (rects[i] != null && rects[i].name == "CharacterSlot")
                {
                    Transform parent = rects[i].parent;
                    while (parent != null)
                    {
                        if (parent.name == "CanvasLandscapeAdaptive (6)")
                        {
                            hasLandscapeSlot = true;
                            break;
                        }

                        parent = parent.parent;
                    }
                }

                if (hasLandscapeSlot)
                    break;
            }

            if (hasLandscapeSlot == false)
                return;

            character.AddComponent<FitTransformToUiViewport>();
        }
    }
}

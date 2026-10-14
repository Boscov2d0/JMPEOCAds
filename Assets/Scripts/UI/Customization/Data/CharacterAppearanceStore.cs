using Code.Gameplay.PlayerLogic.Behaviours;
using Source.Scripts.UI.Customization.Appliers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts.UI.Customization.Data
{
    public static class CharacterAppearanceStore
    {
        private static readonly Dictionary<string, StoredPart> PartsByChildName = new Dictionary<string, StoredPart>();
        private static bool _hasAppearance;
        private static bool _sceneHooked;

        public static bool HasAppearance => _hasAppearance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            PartsByChildName.Clear();
            _hasAppearance = false;
            _sceneHooked = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void HookSceneLoaded()
        {
            if (_sceneHooked)
                return;

            _sceneHooked = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static void CaptureFrom(Transform characterRoot)
        {
            if (characterRoot == null)
                return;

            PartsByChildName.Clear();
            _hasAppearance = false;

            for (int i = 0; i < characterRoot.childCount; i++)
            {
                Transform child = characterRoot.GetChild(i);
                if (child == null)
                    continue;

                SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();
                if (renderer == null)
                    continue;

                PartsByChildName[child.name] = new StoredPart
                {
                    Mesh = renderer.sharedMesh,
                    Materials = renderer.sharedMaterials,
                    BoneNames = CharacterSkinBinder.CopyBoneNames(renderer),
                    RootBoneName = CharacterSkinBinder.CopyRootBoneName(renderer),
                    Unequip = renderer.sharedMesh == null || renderer.enabled == false
                };
                _hasAppearance = true;
            }
        }

        public static void ApplyTo(Transform characterRoot)
        {
            if (!_hasAppearance || characterRoot == null)
                return;

            foreach (KeyValuePair<string, StoredPart> pair in PartsByChildName)
            {
                Transform child = characterRoot.Find(pair.Key);
                if (child == null)
                    continue;

                SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();
                if (renderer == null)
                    continue;

                StoredPart part = pair.Value;
                if (part.Unequip)
                {
                    CharacterSkinBinder.ClearSlot(renderer);
                    continue;
                }

                CharacterSkinBinder.Apply(
                    renderer,
                    characterRoot,
                    part.Mesh,
                    part.Materials,
                    part.BoneNames,
                    part.RootBoneName);
            }
        }

        public static bool TryApplyToSpawnedPlayer()
        {
            if (!_hasAppearance)
                return false;

            GamePlayerCharacter[] players = Object.FindObjectsOfType<GamePlayerCharacter>();

            if (players == null || players.Length == 0)
            {
                GameObject player = GameObject.Find("Player");
                if (player == null)
                    return false;

                ApplyTo(player.transform);
                return true;
            }

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                    ApplyTo(players[i].transform);
            }

            return true;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!_hasAppearance)
                return;

            if (TryApplyToSpawnedPlayer())
                return;

            var runner = new GameObject(nameof(ApplyRunner));
            runner.AddComponent<ApplyRunner>();
        }

        private struct StoredPart
        {
            public Mesh Mesh;
            public Material[] Materials;
            public string[] BoneNames;
            public string RootBoneName;
            public bool Unequip;
        }

        private sealed class ApplyRunner : MonoBehaviour
        {
            private const int MaxAttempts = 30;
            private int _attempts;

            private void LateUpdate()
            {
                if (TryApplyToSpawnedPlayer() || ++_attempts >= MaxAttempts)
                    Destroy(gameObject);
            }
        }
    }
}

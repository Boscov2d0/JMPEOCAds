using System;
using UnityEngine;

namespace Source.Scripts.Orientation
{
    public static class OrientationWatcher
    {
        public static event Action<ScreenLayout> Changed;

        public static ScreenLayout Current { get; private set; }

        public static bool IsPortrait => Current == ScreenLayout.Portrait;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Changed = null;
            Current = default(Source.Scripts.Orientation.ScreenLayout);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            Current = ReadLayout();
            var host = new GameObject(nameof(OrientationWatcher));
            host.hideFlags = HideFlags.HideAndDontSave;
            UnityEngine.Object.DontDestroyOnLoad(host);
            host.AddComponent<Driver>();
        }

        private static void Tick()
        {
            ScreenLayout layout = ReadLayout();
            if (layout == Current)
                return;

            Current = layout;
            Changed?.Invoke(Current);
        }

        private static ScreenLayout ReadLayout()
        {
            return Screen.height >= Screen.width
                ? ScreenLayout.Portrait
                : ScreenLayout.Landscape;
        }

        private sealed class Driver : MonoBehaviour
        {
            private void Update()
            {
                Tick();
            }
        }
    }
}

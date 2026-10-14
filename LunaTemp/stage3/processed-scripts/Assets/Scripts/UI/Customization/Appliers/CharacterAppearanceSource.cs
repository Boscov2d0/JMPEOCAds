using Source.Scripts.UI.Customization.Data;
using UnityEngine;

namespace Source.Scripts.UI.Customization.Appliers
{
    public class CharacterAppearanceSource : MonoBehaviour
    {
        public void Capture()
        {
            CharacterAppearanceStore.CaptureFrom(transform);
        }

        private void OnDestroy()
        {
            Capture();
        }
    }
}

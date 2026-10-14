using Code.Gameplay.PlayerLogic.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Gameplay.PlayerLogic.UI
{
    public class GamePlayerJumpButton : MonoBehaviour, IPlayerJumpButtonInput, IPointerDownHandler
    {
        public bool IsJumpPressed { get; private set; }

        private void OnEnable()
        {
            GamePlayerContext context = GamePlayerContext.Instance;
            context?.RegisterJumpButton(this);
        }

        private void OnDisable()
        {
            GamePlayerContext context = GamePlayerContext.Instance;
            context?.UnregisterJumpButton(this);
        }

        public void OnPointerDown(PointerEventData eventData) => IsJumpPressed = true;

        private void LateUpdate() => IsJumpPressed = false;
    }
}

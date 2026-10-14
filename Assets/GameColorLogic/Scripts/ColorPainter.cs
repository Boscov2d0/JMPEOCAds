using UnityEngine;

namespace Code.Gameplay.Coloring
{
    public class ColorPainter : MonoBehaviour
    {
        private ColorField _field;
        private int _ownerId;
        private Color _color;
        private bool _isPlayer;
        private bool _initialized;

        public int OwnerId => _ownerId;
        public Color Color => _color;
        public bool IsPlayer => _isPlayer;

        public void Initialize(ColorField field, int ownerId, Color color, bool isPlayer)
        {
            _field = field;
            _ownerId = ownerId;
            _color = color;
            _isPlayer = isPlayer;
            _initialized = _field != null && _ownerId != ColorField.UnpaintedOwnerId;
        }

        private void LateUpdate()
        {
            if (!_initialized)
                return;

            if (_field == null)
                _field = ColorField.Instance;

            if (_field == null)
                return;

            _field.TryPaint(transform.position, _ownerId, _color);
        }
    }
}

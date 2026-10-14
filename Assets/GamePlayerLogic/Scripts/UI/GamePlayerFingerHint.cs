using UnityEngine;

namespace Code.Gameplay.PlayerLogic.UI
{
    public class GamePlayerFingerHint : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _width = 100f;
        [SerializeField] private float _height = 80f;
        [SerializeField] private float _offsetX;
        [SerializeField] private float _offsetY;

        private RectTransform _rectTransform;
        private float _time;
        private Vector2 _startPosition;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _startPosition = _rectTransform.anchoredPosition;
        }

        private void Update()
        {
            _time += Time.deltaTime * _speed;

            float posX = Mathf.Sin(_time) * _width;
            float posY = Mathf.Sin(_time * 2f) * _height * 0.5f;

            _rectTransform.anchoredPosition = new Vector2(
                _startPosition.x + posX + _offsetX,
                _startPosition.y + posY + _offsetY);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay.Coloring
{
    public class ColorScoreText : MonoBehaviour
    {
        [SerializeField] private ColorField _colorField;
        [SerializeField] private Text _scoreText;
        [SerializeField] private TextMeshProUGUI _scoreTextTmp;

        private int _displayedScore = int.MinValue;

        private void Awake()
        {
            if (_scoreText == null)
                _scoreText = GetComponent<Text>();
            if (_scoreTextTmp == null)
                _scoreTextTmp = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            ResolveField();
            if (_colorField != null)
                _colorField.PlayerScoreChanged += Refresh;

            Refresh(_colorField != null ? _colorField.PlayerScore : 0);
        }

        private void OnDisable()
        {
            if (_colorField != null)
                _colorField.PlayerScoreChanged -= Refresh;
        }

        private void LateUpdate()
        {
            if (_colorField == null)
            {
                ResolveField();
                if (_colorField != null)
                    _colorField.PlayerScoreChanged += Refresh;
            }

            if (_colorField != null)
                Refresh(_colorField.PlayerScore);
        }

        private void ResolveField()
        {
            if (_colorField != null)
                return;

            _colorField = ColorField.Instance;
            if (_colorField == null)
                _colorField = FindObjectOfType<ColorField>();
        }

        private void Refresh(int score)
        {
            if ((_scoreText == null && _scoreTextTmp == null) || score == _displayedScore)
                return;

            _displayedScore = score;
            string value = score.ToString();
            if (_scoreText != null)
                _scoreText.text = value;
            if (_scoreTextTmp != null)
                _scoreTextTmp.text = value;
        }
    }
}

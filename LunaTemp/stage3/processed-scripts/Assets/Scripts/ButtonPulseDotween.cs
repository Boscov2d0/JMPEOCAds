using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPulseDotween : MonoBehaviour
{
    [SerializeField] private float _scaleCoefficient = 1.2f;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _pauseBefore = 0.1f;
    [SerializeField] private float _pauseAfter = 0.5f;
    [SerializeField] private float _fadeDuration = 0.2f;
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private bool _fromStart = false;
    [SerializeField] private bool _isLoop = true;
    [SerializeField] private bool _isNeedDeactivate = true;

    private Image _image;
    private Button _button;
    private Sequence _sequence;
    private Vector3 _originalScale;
    private bool _originalScaleCached;

    private void Start()
    {
        if (_fromStart)
            Activate();
        else
            CacheOriginalScale();
    }
    public void Activate()
    {
        CacheOriginalScale();
        _sequence?.Kill();
        _sequence = null;

        _targetObject.SetActive(true);
        _image = _targetObject.GetComponent<Image>();
        _button = GetComponent<Button>();

        if (_button != null)
            _button.enabled = true;

        Color baseColor = _image.color;
        baseColor.a = 0f;
        _image.color = baseColor;

        Color visibleColor = baseColor;
        visibleColor.a = 1f;

        Color invisibleColor = baseColor;
        invisibleColor.a = 0f;

        _sequence = DOTween.Sequence();

        _sequence.Append(
            DOTween.To(() => _image.color, x => _image.color = x, visibleColor, _fadeDuration)
                .SetEase(Ease.InQuad));

        _sequence.AppendInterval(_pauseBefore);

        _sequence.Append(transform.DOScale(Vector3.one * _scaleCoefficient, _duration)
            .SetEase(Ease.InOutSine));

        _sequence.Append(transform.DOScale(Vector3.one, _duration)
            .SetEase(Ease.InOutSine));

        if (_isNeedDeactivate)
        {
            _sequence.Append(
                DOTween.To(() => _image.color, x => _image.color = x, invisibleColor, _fadeDuration)
                    .SetEase(Ease.InQuad));
        }

        if (_isLoop)
        {
            _sequence.AppendInterval(_pauseAfter);
            _sequence.SetLoops(-1, LoopType.Restart);
        }
    }

    public void Complete()
    {
        Deactivate();

        if (_button == null)
            _button = GetComponent<Button>();

        if (_button != null)
            _button.enabled = true;
    }

    public void Deactivate()
    {
        CacheOriginalScale();
        _sequence?.Kill();
        _sequence = null;

        transform.localScale = _originalScale;

        if (_targetObject)
            _targetObject.SetActive(false);
    }

    private void CacheOriginalScale()
    {
        if (_originalScaleCached)
            return;

        _originalScale = transform.localScale;
        _originalScaleCached = true;
    }
}
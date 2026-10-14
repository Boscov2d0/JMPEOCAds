using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    private bool _isPlaying;
    private float _lastVolume = 0f;

    public void ToggleMute()
    {
        _isPlaying = !_isPlaying;
#if UNITY_LUNA
        // Playworks does not implement AudioMixer.GetFloat/SetFloat and logs them as unsupported.
        AudioListener.volume = _isPlaying ? 0f : 1f;
#else
        if (_audioMixer == null)
            return;

        if (_isPlaying)
        {
            _audioMixer.GetFloat("MasterVolume", out _lastVolume);
            _audioMixer.SetFloat("MasterVolume", -80f);
        }
        else
            _audioMixer.SetFloat("MasterVolume", _lastVolume);
#endif
    }
}
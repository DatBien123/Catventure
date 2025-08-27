using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private bool _muteBackgroundMusic = false;
    private bool _muteSoundFX = false;

    public static SoundManager instance;

    private AudioSource _audioSource;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    private void Update() {

    }

    public void ToggleBackgroundMusic() {
        _muteBackgroundMusic = !_muteBackgroundMusic;
        if ( _muteBackgroundMusic ) {
            _audioSource.Stop();
        } else {
            _audioSource.Play();
        }
    }


    public void ToggleSoundFX() {
        _muteSoundFX = !_muteSoundFX;
        GameEvents.OnToggleSoundFXMethod();
    }

    public bool IsSoundFXMuted() {
        return _muteSoundFX;
    }

    public void SilenceBackgroundMusic(bool silence) {
        if(_muteBackgroundMusic == false) {
            if (silence) {
                _audioSource.volume = 0f;
            } else {
                _audioSource.volume = 1f;
            }
        }
    }

}

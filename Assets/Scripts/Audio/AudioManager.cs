using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using Unity.VisualScripting;
public class AudioManager : MonoBehaviour
{
    [Header("------Audio Clip------")]
    public Sound[] sounds;

    private static bool isMuted = false;
    private static float savedVolume = 1f;

    public static AudioManager instance;

    public bool isDestroyOnLoad = false;

    public void OnEnable()
    {
        UIEventSystem.Register("Toggle Mute", ToggleMute);
    }
    public void OnDisable()
    {
        UIEventSystem.Unregister("Toggle Mute", ToggleMute);

    }
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        if(!isDestroyOnLoad)DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }


    private void Start()
    {
        if (isMuted)
            AudioListener.volume = 0f;
        else
            AudioListener.volume = savedVolume;
    }
    public void PlaySFX(string name)
    {
        // tìm âm thanh với name ta cần trong sounds
        // Đây là 1 cách viết vòng lặp ngắn gọn đơn giản và rất rất hay vì nó ngắn hơn nma nghĩa không đổi
        //tạo s để lưu và dùng hàm tìm trong Array 
        //            danh sách sound, từng sound trong danh sách và so sánh từng sound.name trong danh sách với biến tham số name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
        else
        {
            Debug.Log("Không tìm thấy SFX" + name);
            return;

        }

    }
    public void StopSFX(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null) s.source.Stop();
        else return;
    }
    public void PlayMusic(string name)
    {
        if (CheckIsMusicPlaying(name)) return;
        StopAllMusic(); // dừng tất cả trước
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
        else
        {
            Debug.Log("Không tìm thấy music" + name);
            return;
        }
    }
    public bool CheckIsMusicPlaying(string musicName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == musicName);
        if (s != null && s.type == SoundType.Music)
        {
            return s.source.isPlaying;
        }
        return false;
    }
    public void StopAllMusic()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == SoundType.Music)
            {
                if (sound.source.isPlaying)
                    sound.source.Stop();
            }
        }
    }
    public void StopAllSoundFX()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == SoundType.SFX)
            {
                if (sound.source.isPlaying)
                    sound.source.Stop();
            }
        }
    }
    public void StopAllSounds()
    {
        foreach (Sound sound in sounds)
        {

                if (sound.source.isPlaying)
                    sound.source.Stop();
        }
    }
    public void PlayPronunciation(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            savedVolume = AudioListener.volume;
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = savedVolume;
        }
    }
    public bool IsMuted()
    {
        return isMuted;
    }
}

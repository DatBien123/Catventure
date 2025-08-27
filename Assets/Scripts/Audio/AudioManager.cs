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

    public static AudioManager instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
    public void PlaySFX(string name)
    {
        // tìm âm thanh với name ta cần trong sounds
        // Đây là 1 cách viết vòng lặp ngắn gọn đơn giản và rất rất hay vì nó ngắn hơn nma nghĩa không đổi
        //tạo s để lưu và dùng hàm tìm trong Array 
        //            danh sách sound, từng sound trong danh sách và so sánh từng sound.name trong danh sách với biến tham số name
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
        else return;
    }
    public void PlayMusic(string name)
    {
        if (CheckIsMusicPlaying(name)) return;
        StopAllMusic(); // dừng tất cả trước
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
        else return;
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
    public void PlayPronunciation(AudioClip clip)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}

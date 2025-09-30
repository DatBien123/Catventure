using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct StoryData
{
    public string ID;
    public string Name;
    public Texture2D Icon;
    [TextArea] public string description;
    public bool IsUnlocked;

    public List<StoryPageData> StoryPageDatas;
}

[System.Serializable]
public struct StoryPageData
{
    public VideoClip VideoClip;
    public AudioClip StoryAudioClip;
}
[CreateAssetMenu(fileName = "Story Data", menuName = "Story/Story Data")]
public class SO_Story : ScriptableObject
{
    public StoryData Data;
}

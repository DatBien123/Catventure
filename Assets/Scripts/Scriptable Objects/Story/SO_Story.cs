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

    public List<StoryPageData> StoryPageDatas;
}

[System.Serializable]
public struct StoryPageData
{
    public Sprite StoryImage;
    public List<AudioClip> StoryAudioClips;

    public bool isDelay;
    public float delayTime;
}
[CreateAssetMenu(fileName = "Story Data", menuName = "Story/Story Data")]
public class SO_Story : ScriptableObject
{
    public StoryData Data;
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public struct MapData
{
    public int index;
    public string Name;
    public Sprite Image;
    [TextArea] public string description;

    public List<Topic> topicList;
}

[System.Serializable]
public struct Topic
{
    [Header("UI")]
    public Sprite TopicImage;
    [TextArea] public string description;
    public int enegyConsumed;
    public int coinReward;

    [Header("Others")]
    public int index;
    public string topicName;
    public TimelineAsset cutscene;
    public string minigameSceneName;
}
[CreateAssetMenu(fileName = "Map Data", menuName = "Map/Map Data")]
public class SO_Map : ScriptableObject
{
    public MapData Data;
}

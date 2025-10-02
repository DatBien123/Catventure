using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public struct MapSelectingData
{
    public SO_Map CurrentMapSelecting;
    public Topic CurrentTopic;
}

[CreateAssetMenu(fileName = "Map Selecting Data", menuName = "Map/Map Selecting Data")]
public class SO_MapSelecting : ScriptableObject
{
    public MapSelectingData Data;
}

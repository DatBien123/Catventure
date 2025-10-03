using System.Collections.Generic;

[System.Serializable]
public class MapInstance
{
    public SO_Map MapData;
    public bool isUnlock;
    public bool isSelected;

    public List<int> UnlockTopicsIndex;
    public List<int> CompletedTopicsIndex;

    public MapInstance(SO_Map map, List<int> unlockTopicsIndex, List<int> completedTopicsIndex, bool unlock, bool selected)
    {
        this.MapData = map;
        this.UnlockTopicsIndex = unlockTopicsIndex;
        this.CompletedTopicsIndex = completedTopicsIndex;
        this.isUnlock = unlock;
        this.isSelected = selected;
    }
}
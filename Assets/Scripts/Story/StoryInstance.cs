using UnityEngine;

[System.Serializable]
public class StoryInstance
{
    public SO_Story StoryData;
    public bool isUnlock;

    public StoryInstance(SO_Story story, bool unlock)
    {
        StoryData = story;
        this.isUnlock = unlock;
    }
}

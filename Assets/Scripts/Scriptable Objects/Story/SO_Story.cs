using UnityEngine;

[System.Serializable]
public struct StoryData
{
    public string ID;
    public string Name;
    public Texture2D Icon;
    [TextArea] public string description;
    public bool IsUnlocked;
}

[CreateAssetMenu(fileName = "Story Data", menuName = "Story/Story Data")]
public class SO_Story : ScriptableObject
{
    public StoryData Data;
}

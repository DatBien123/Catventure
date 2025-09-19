using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct DialogueLine
{
    [TextArea] public string text;
}

[System.Serializable]
public struct DialogueData
{
    public string Topic;
    public Sprite SpeakerImage;
    public string SpeakerName;
    public List<DialogueLine> DialogueLines;

    public UnityEvent OnDialogueFinished;

}
[CreateAssetMenu(fileName = "Dialogue Database", menuName = "Dialogue System/Dialogue Database")]
public class SO_DialogueDataBase : ScriptableObject
{
    public List<DialogueData> Datas;
}

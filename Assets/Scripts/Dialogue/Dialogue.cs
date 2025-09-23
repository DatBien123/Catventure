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
public class Dialogue : MonoBehaviour
{
    [Header("References")]
    public CharacterPlayer Player;
    public UIDialogue UIDialogue;

    public List<DialogueData> DialogueDataBase;
}

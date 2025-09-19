using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("References")]
    public CharacterPlayer Player;
    public UIDialogue UIDialogue;
    public SO_DialogueDataBase DialogueDataBase;

    private void Start()
    {
        if (Player.isFirstTimeLogin)
        {
            DialogueData dialogueData = DialogueDataBase.Datas.Find(dialogueData => dialogueData.Topic == "First Time Open Game");
            UIDialogue.SetCurrentDialogueData(dialogueData);
        }
    }
}

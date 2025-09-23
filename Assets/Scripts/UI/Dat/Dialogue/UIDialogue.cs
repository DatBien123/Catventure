using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDialogue : MonoBehaviour
{

    [Header("Dialogue UI Informations")]
    public Image SpeakerImage;
    public TextMeshProUGUI SpeakerName;
    public TextMeshProUGUI DialogueLine;

    [Header("Buttons")]
    public Button NextDialogueLine;
    public bool CanPressNext = true;

    [Header("References")]
    public Animator Animator;
    public Dialogue Dialogue;

    public DialogueData CurrentDialogueData;
    public DialogueLine CurrentDialogueLine;
    public int CurrentDialogueLineIndex = 0;

    private void Awake()
    {
        NextDialogueLine.onClick.AddListener(() => OnNextDialogueLine());
    }
    public void SetCurrentDialogueData(DialogueData currentDialogueData)
    {
        CurrentDialogueData = currentDialogueData;
        CurrentDialogueLine = CurrentDialogueData.DialogueLines[0];

        Animator.CrossFadeInFixedTime("Open Dialogue", 0.0f);
        SetupDialogueLine(0);
        Animator.gameObject.SetActive(true);
    }
    public void SetupDialogueLine(int index)
    {
        SpeakerImage.sprite = CurrentDialogueData.SpeakerImage;
        SpeakerName.text = CurrentDialogueData.SpeakerName;
        DialogueLine.text = CurrentDialogueData.DialogueLines[index].text;
    }
    public void OnNextDialogueLine()
    {
        if (!CanPressNext) return;

        if(CurrentDialogueLineIndex < CurrentDialogueData.DialogueLines.Count - 1)
        {
            CurrentDialogueLineIndex++;
            CurrentDialogueLine = CurrentDialogueData.DialogueLines[CurrentDialogueLineIndex];

            //UI
            SetupDialogueLine(CurrentDialogueLineIndex);
            ShowText(CurrentDialogueLine.text);
        }
        else
        {
            OnFinishDialogue();
        }
    }
    public void OnFinishDialogue()
    {
        CurrentDialogueLineIndex = 0;
        Animator.gameObject.SetActive(false);

        CurrentDialogueData.OnDialogueFinished?.Invoke();
    }

    #region [Typer Writer Text Effect]
    public float delay = 0.05f;

    public void ShowText(string fullText)
    {
        StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string text)
    {
        DialogueLine.text = "";
        CanPressNext = false;
        foreach (char c in text)
        {
            DialogueLine.text += c;
            yield return new WaitForSeconds(delay);
        }
        CanPressNext = true;
    }

    #endregion
}

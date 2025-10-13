using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public TMP_Text displayedText;
    public Image crossLine;
    [SerializeField] private Image icon;
    [SerializeField] private AudioClip audioClip;

    private string _word;

    private void OnEnable() {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable() {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWords(string word, Sprite sprite, AudioClip audioClip) {
        _word = word;
        displayedText.text = word;
        icon.sprite = sprite;
        this.audioClip = audioClip;
    }

    public void CorrectWord(string word, List<int> squareIndexes) {
        if (word == _word) {
            crossLine.gameObject.SetActive(true);
            WordsSpySoundManager.instance.PlaySFX(audioClip);
        }
    }

}

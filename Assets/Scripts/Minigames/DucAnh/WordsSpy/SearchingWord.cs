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

    private string _word;

    private void OnEnable() {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable() {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWords(string word, Sprite sprite) {
        _word = word;
        displayedText.text = word;
        icon.sprite = sprite;
    }

    public void CorrectWord(string word, List<int> squareIndexes) {
        if (word == _word) {
            crossLine.gameObject.SetActive(true);
        }
    }

}

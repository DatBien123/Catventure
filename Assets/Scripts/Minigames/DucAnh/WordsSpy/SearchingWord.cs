using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWord : MonoBehaviour
{
    public TMP_Text displayedText;
    public Image crossLine;

    private string _word;

    private void OnEnable() {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable() {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWords(string word) {
        _word = word;
        displayedText.text = word;
    }

    public void CorrectWord(string word, List<int> squareIndexes) {
        if (word == _word) {
            crossLine.gameObject.SetActive(true);
        }
    }

}

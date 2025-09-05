using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GameEvents;

public class GridSquare : MonoBehaviour {
    public int SquareIndex { get; set; }

    private AlphabetDataSO.LetterData _normalletterData;
    private AlphabetDataSO.LetterData _selectedletterData;
    private AlphabetDataSO.LetterData _correctletterData;

    private SpriteRenderer _displayedImage;

    private bool _selected;
    private bool _clicked;
    private int _index = -1;
    private bool _correct;

    private AudioSource _audioSource;

    public void SetIndex(int index) {
        _index = index;
    }

    public int GetIndex() { return _index; }


    private void Awake() {
        _correct = false;
        _selected = false;
        _clicked = false;
        _displayedImage = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += GameEvents_OnSelectSquare;
        GameEvents.OnCorrectWord += CorrectWord;

    }

    private void OnDisable() {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= GameEvents_OnSelectSquare;
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    private void CorrectWord(string word, List<int> squareIndexes) {
        if(_selected && squareIndexes.Contains(_index)) {
            _correct = true;
            _displayedImage.sprite = _correctletterData.image;
        }

        _selected = false;
        _clicked = false;
    }

    private void OnMouseDown() {
        OnEnableSquareSelection();
        GameEvents.EnableSquareSelectionMethod();
        CheckSquare();
        _displayedImage.sprite = _selectedletterData.image;
    }

    private void OnMouseEnter() {
        CheckSquare();
    }

    private void OnMouseUp() {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }

    private void OnEnableSquareSelection() {
        _clicked = true;
        _selected = false;
    }

    private void OnDisableSquareSelection() {
        _selected = false;
        _clicked = false;

        if(_correct == true)
            _displayedImage.sprite = _correctletterData.image;
        else
            _displayedImage.sprite = _normalletterData.image;
    }

    private void GameEvents_OnSelectSquare(Vector3 position) {
        if(this.gameObject.transform.position == position) 
            _displayedImage.sprite = _selectedletterData.image;
    }

    private void CheckSquare() {
        if(_selected == false && _clicked == true) {
            if (WordsSpySoundManager.instance.IsSoundFXMuted() == false) {
                _audioSource.Play();
            }
            _selected = true;
            GameEvents.CheckSquareMethod(_normalletterData.letter, gameObject.transform.position, _index);
        }
    }

    public void SetSprite(AlphabetDataSO.LetterData normalLetterData, AlphabetDataSO.LetterData selectedLetterData, AlphabetDataSO.LetterData correctLetterData) { 
        _normalletterData = normalLetterData;
        _selectedletterData = selectedLetterData;
        _correctletterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = _normalletterData.image;

    }



}

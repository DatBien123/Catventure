using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public GameDataSO currentGameData;
    public TMP_Text timerText;

    private float _timeLeft;
    private float _minutes;
    private float _seconds;
    private float _oneSecondDown;
    private bool _timeOut;
    private bool _stopTimer;

    private void Awake() {
        _stopTimer = false;
        _timeOut = false;
        _timeLeft = currentGameData.selectedBoardData.timeInSeconds;
        _oneSecondDown = _timeLeft - 1f;

        GameEvents.OnBoardCompleted += StopTimer;
        GameEvents.OnUnlockNextCatergory += StopTimer;

    }

    private void OnDisable() {
        GameEvents.OnBoardCompleted -= StopTimer;
        GameEvents.OnUnlockNextCatergory -= StopTimer;
    }

    private void Update() {
        if(_stopTimer == false) {
            _timeLeft -= Time.deltaTime;
        }

        if (_timeLeft <= _oneSecondDown) {
            _oneSecondDown = _timeLeft - 1f;
        }
    }

    private void StopTimer() {
        _stopTimer = true;
    }

    private void OnGUI() {
        if(_timeOut == false) {
            if (_timeLeft > 0f) {
                _minutes = Mathf.Floor(_timeLeft / 60);
                _seconds = Mathf.RoundToInt(_timeLeft % 60);

                timerText.text = _minutes.ToString("00") + ":" + _seconds.ToString("00");
            } else {
                _stopTimer = true;
                ActivateGameOverGUI();
            }
        }
    }

    private void ActivateGameOverGUI() {
        GameEvents.GameOverMethod();
        _timeOut = true;
    }

}

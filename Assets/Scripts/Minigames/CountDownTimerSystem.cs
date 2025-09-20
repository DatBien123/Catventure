using TMPro;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
// Hệ thống đếmn ngược thời gian trong mỗi minigame
public class CountDownTimerSystem : MonoBehaviour
{
    private float timeRemaining; // 60 giây
    private float timeStart;
    public bool keepingCountDown = false;
    public float second = 1f;
    public Text timerText; // Kéo Text vào đây trong Inspector
    public Action OnTimeUp; // Sự kiện thông báo hết giờ
    public int lastDisplayedSeconds = -1;
    

    // Hệ thống đếm ngược 3 2 1 bắt đầu

    public GameObject countdownUI; // object chứa text đếm ngược
    public Text countdownText; // Text hiển thị số đếm ngược
    public Color[] colors;
    public System.Action OnCountdownComplete; // Gọi khi đếm xong

    private void Awake()
    {
        timeRemaining = timeStart;
    }
    public void StartCountdown321()
    {
        gameObject.SetActive(true); 
        StartCoroutine(CountdownRoutine321());
    }
    private bool isRunning = true;

    void Update()
    {
        if (isRunning && keepingCountDown)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                int currentSeconds = Mathf.FloorToInt(timeRemaining / Time.deltaTime);
                if (currentSeconds != lastDisplayedSeconds)
                {
                    lastDisplayedSeconds = currentSeconds;
                    UpdateTimerDisplay();
                }
                UpdateTimerDisplay(); // gọi mỗi giây 1 lần
            }
            else
            {
                timeRemaining = 0;
                isRunning = false;
                UpdateTimerDisplay();
                // Gọi xử lý khi hết giờ ở đây
                OnTimeUp?.Invoke();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public bool IsEnoughTimeToWin()
    {
        if (timeRemaining > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // set thời gian bắt đầu đếm ngược
    public void SetTimeStart(float timeStart)
    {
        this.timeStart = timeStart;
        timeRemaining = timeStart;
    }
    public void StopCountDown()
    {
        keepingCountDown = false;
    }
    public void StartCountDown()
    {
        keepingCountDown = true;
    }
    public float GetCompletionTime() // lấy ra thời gian hoàn thành minigame bằng lấy thời gian bắt đầu trừ đi thời gian còn lại
    {
        return timeStart - timeRemaining;
    }
    private IEnumerator CountdownRoutine321()
    {
        countdownUI.SetActive(true);

        string[] countdowns = { "3", "2", "1", "Bắt đầu!" };
        int i = 0;
        foreach (string count in countdowns)
        {
            countdownText.text = count;
            countdownText.transform.localScale = Vector3.zero;
            countdownText.color = colors[i];
            // Hiệu ứng phóng to ra (pop)
            countdownText.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
            i++;
            yield return new WaitForSeconds(second);
        }

        countdownUI.SetActive(false);

        OnCountdownComplete?.Invoke(); // Gọi sự kiện bắt đầu minigame
    }
}

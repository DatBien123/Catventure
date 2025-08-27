using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
// Hệ thống đếmn ngược thời gian trong mỗi minigame
public class CountDownTimerSystem : MonoBehaviour
{
    private float timeRemaining = 60f; // 60 giây
    public float timeStart = 60f;
    public bool keepingCountDown = false;
    public float second = 1f;
    public Text timerText; // Kéo Text vào đây trong Inspector
    public Action<bool> OnTimeUp; // Sự kiện thông báo hết giờ
    public int lastDisplayedSeconds = -1;
    private void Awake()
    {
        timeRemaining = timeStart;
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
                if (currentSeconds != lastDisplayedSeconds) { 
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
                OnTimeUp?.Invoke(false);
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
}

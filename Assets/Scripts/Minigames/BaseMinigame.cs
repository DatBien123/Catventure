using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseMinigame : MonoBehaviour, IMinigame
{
    [Header("Optional Systems")]
    [SerializeField] protected HealthSystem health;
    [SerializeField] protected CountDownTimerSystem countDownTimer;

    [Header("UI References")]
    [SerializeField] protected GameOverScreen gameOverScreen;
    [SerializeField] protected VictoryRewardScreen victoryRewardScreen;
    [SerializeField] protected Button tapToContinueButton;
    [SerializeField] public GameObject inputBlocker;
    [SerializeField] private PopUpUI popupUI;

    public event System.Action<bool> OnGameEnd;

    protected virtual void OnEnable()
    {
        if (health != null) health.OnHealthZero += OnHealthDepleted;
        if (countDownTimer != null) countDownTimer.OnTimeUp += OnTimeUp;
        UIEventSystem.Register("Exit Game", ExitGame);
        UIEventSystem.Register("Replay", Replay);
        UIEventSystem.Register("Back To Home Menu", BackToHomeMenu);
    }

    protected virtual void OnDisable()
    {
        if (health != null) health.OnHealthZero -= OnHealthDepleted;
        if (countDownTimer != null) countDownTimer.OnTimeUp -= OnTimeUp;
        UIEventSystem.Unregister("Exit Game", ExitGame);
        UIEventSystem.Unregister("Replay", Replay);
        UIEventSystem.Unregister("Back To Home Menu", BackToHomeMenu);
    }

    public virtual void StartGame()
    {
    }

    public virtual void EndGame(bool success)
    {
        if (countDownTimer != null) countDownTimer.StopCountDown();
        OnGameEnd?.Invoke(success);

        if (success)
        {
        }
        else
        {
            inputBlocker.SetActive(false);
            gameOverScreen?.Setup();
        }
    }

    public virtual void GameOver()
    {
        EndGame(false);
    }

    protected void OnHealthDepleted() {
        Debug.Log("Hết máu");
        EndGame(false);
    }
    protected void OnTimeUp() {
        Debug.Log("Hết giờ");
        EndGame(false); }

    // tiện để các minigame con truy cập
    public HealthSystem GetHealthSystem() => health;
    public CountDownTimerSystem GetCountDownSystem() => countDownTimer;
    public void ExitGame()
    {
        // Thoát game về home menu
        Debug.Log("Quay về home menu");
        popupUI.ShowConfirm(
        "MENU",
        "Bạn muốn về Home Menu sao?",
        yesCallback: () => {
            BackToHomeMenu();
    },
        noCallback: () => {
    }
        );

    }
    public virtual void Replay()
    {
        
    }
    public void BackToHomeMenu()
    {
        Debug.Log("Về Home Menu");
        AudioManager.instance.StopAllSounds();
        SceneManager.LoadScene("Home Scene");
    }

}

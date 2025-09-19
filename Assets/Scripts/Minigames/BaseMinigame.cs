using UnityEngine;
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

    public event System.Action<bool> OnGameEnd;

    protected virtual void OnEnable()
    {
        if (health != null) health.OnHealthZero += OnHealthDepleted;
        if (countDownTimer != null) countDownTimer.OnTimeUp += OnTimeUp;
    }

    protected virtual void OnDisable()
    {
        if (health != null) health.OnHealthZero -= OnHealthDepleted;
        if (countDownTimer != null) countDownTimer.OnTimeUp -= OnTimeUp;
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
            gameOverScreen?.Setup();
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
}

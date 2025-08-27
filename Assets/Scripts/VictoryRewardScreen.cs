using UnityEngine;
using UnityEngine.UI;
public class VictoryRewardScreen : MonoBehaviour
{
    public Text catCoinFootNumberText; // số lượng xu chân mèo nhận được
    public Text languagueEnergyNumberText; // số lượng năng lượng ngôn ngữ nhận được
    public Text minigameResult;

    public void ShowRewardFITB(int catCoinFootNumber, int languagueEnergyNumber, int correctAnswer, int totalQuestions, float completionTime)
    {
        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Minigame Completed");
        catCoinFootNumberText.text = catCoinFootNumber.ToString();
        languagueEnergyNumberText.text=languagueEnergyNumber.ToString();
        minigameResult.text = $"✅ Correct Answers: {correctAnswer}/{totalQuestions} – ⏱ Time: {(int)completionTime}s";
    }
    public void ShowRewardDragDrop(int catCoinFootNumber, int languagueEnergyNumber)
    {
        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Minigame Completed");
        catCoinFootNumberText.text = catCoinFootNumber.ToString();
        languagueEnergyNumberText.text = languagueEnergyNumber.ToString();
    }
}

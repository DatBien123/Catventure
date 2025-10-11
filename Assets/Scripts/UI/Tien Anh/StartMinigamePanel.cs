using UnityEngine;
using UnityEngine.UI;
public class StartMinigamePanel : MonoBehaviour
{
    private string title;
    private string itemName;
    private string timeRequired;
    private string reward;

    public Text titleText;
    public Image itemResult;
    public Text itemNameText;
    public Text timeRequiredText;
    public Text rewardText;

    public void Setup(string title, Sprite sprite, string itemName, float timeRequired, float reward)
    {
        //Debug.Log("Setup minigame");
        this.title = title;
        this.itemName = itemName;
        this.timeRequired = timeRequired.ToString();
        this.reward = reward.ToString();

        titleText.text = this.title;
        itemResult.sprite = sprite;
        itemNameText.text = this.itemName;
        timeRequiredText.text = $"THỜI GIAN: {timeRequired}S";
        rewardText.text = $"PHẦN THƯỞNG: {reward}";
    }
}

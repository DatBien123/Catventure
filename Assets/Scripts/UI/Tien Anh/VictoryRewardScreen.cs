using UnityEngine;
using UnityEngine.UI;
public class VictoryRewardScreen : MonoBehaviour
{
    private float reward; // số lượng xu chân mèo nhận được
    public Image itemImage;
    public Text itemNameText;
    public Text rewardText;


    public void ShowRewardFITB(float reward)
    {
        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Minigame Completed");
        itemImage.gameObject.SetActive(false);
        this.reward = reward;
        rewardText.text = $"PHẦN THƯỞNG: {this.reward} XU";

    }
    public void ShowRewardDragDrop(Sprite itemSprite,string itemName, float reward)
    {
        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Minigame Completed");
        this.reward = reward;

        itemImage.sprite = itemSprite;
        itemNameText.text = itemName;   
        rewardText.text = $"PHẦN THƯỞNG: {this.reward} XU";
    }
}

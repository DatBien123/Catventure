using DG.Tweening;
using UnityEngine;
using System.Collections;

using UnityEngine.UI;
public class VictoryRewardScreen : MonoBehaviour
{
    private float reward; // số lượng xu chân mèo nhận được
    public int numberStar; // số lượng sao nhận được
    public Image itemImage;
    public Text itemNameText;
    public Text rewardText;
    public Sprite[] stars;
    public Transform starParent; // object parrent để sinh các object con star  vào trong đó
    public void ShowRewardFITB(float reward)
    {
        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Minigame Completed");
        itemImage.gameObject.SetActive(false);
        this.reward = reward;
        rewardText.text = $"PHẦN THƯỞNG: {this.reward} XU";
    }
    public void ShowRewardDragDrop(Sprite itemSprite, string itemName, float reward, int star)
    {
        numberStar = star;
        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Minigame Completed");
        this.reward = reward;
        // Show các ngôi sao dựa trên biến star nhận được
        itemImage.sprite = itemSprite;
        itemNameText.text = itemName;
        rewardText.text = $"PHẦN THƯỞNG: {this.reward} XU";
        StartCoroutine(SpawnStars());



    }
    IEnumerator SpawnStars()
    {
        yield return new WaitForSeconds(1f);
        // xóa hết sao cũ nếu có
        foreach (Transform child in starParent)
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < numberStar; i++)
        {
            // Tạo 1 UI Image mới
            GameObject starGO = new GameObject("Star", typeof(Image));
            starGO.transform.SetParent(starParent, false); // false để giữ scale/anchor theo parent
            Image starImage = starGO.GetComponent<Image>();
            starImage.sprite = stars[i]; // lấy sprite từ mảng stars
            // bắt đầu scale = 0
            starGO.transform.localScale = Vector3.zero;
            // hiệu ứng pop-up
            Sequence seq = DOTween.Sequence();
            seq.Append(starGO.transform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack));
            seq.Append(starGO.transform.DOScale(1f, 0.15f).SetEase(Ease.InOutSine));
            yield return seq.WaitForCompletion();
            //AudioManager.instance.PlaySFX("Star Pop"); // Nếu có âm thanh

        }
    }
}

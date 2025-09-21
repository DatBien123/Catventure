using UnityEngine;
using UnityEngine.UI;
public class BoilingStirringUI : MonoBehaviour
{
    public GameObject notificationDish;
    public static BoilingStirringUI Instance;
    public CraftingItemBarUI ingredientBarUI;
    public GameObject finishedText;
    public GameObject userInterface;
    public GameObject itemCraftingHolder; // Container chứa các nguyên liệu trên màn hình
    public GameObject ingredientBar;
    // 2 game object con của Canvas và là cha chứa các UI tương ứng của 2 công đoạn
    public GameObject boiling; 
    public GameObject stirring;


    public GameObject inputBlockerPanel;
    public GameObject circleArrowGuide;
    private void Awake()
    {
        Instance = this;
    }
    public void Setup(CookingStepSO step)
    {
        ingredientBarUI.InitBar(step);
    }

    public void ShowNotificationDish(Sprite sprite, string name, string description)
    {
        notificationDish.SetActive(true);   
        notificationDish.GetComponent<NotificationResult>().ShowNotificationResult(sprite, name, description);
    }
}

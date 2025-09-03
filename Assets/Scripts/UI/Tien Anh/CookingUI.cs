using UnityEngine;
using UnityEngine.UI;
public class CookingUI : MonoBehaviour
{
    public GameObject notificationDish;
    public static CookingUI Instance;
    public CraftingItemBarUI ingredientBarUI;
    private void Awake()
    {
        Instance = this;
    }
    public void Setup(CraftingRecipeSO recipe)
    {
        ingredientBarUI.Init(recipe);

    }

    public void ShowNotificationDish(Sprite sprite, string name, string description)
    {
        notificationDish.SetActive(true);   
        notificationDish.GetComponent<NotificationResult>().ShowNotificationResult(sprite, name, description);
    }
}

using UnityEngine;
using UnityEngine.UI;
public class IngredientItemUI : MonoBehaviour
{
    public Image icon;
    public Text amountText;
    public FoodSelection foodSelection;
    public void Awake()
    {
        foodSelection = FindAnyObjectByType<FoodSelection>();
    }
    public void Setup(IngredientRequirement req)
    {
        icon.sprite = req.ingredient.commonData.icon;
        Debug.Log(req.ingredient.commonData.itemName);
        int ownedNumber = 0;

        // Kiểm tra item có tồn tại không
        if (foodSelection.zera.Inventory.CheckItemExist(req.ingredient))
        {
            ownedNumber = foodSelection.zera.Inventory.GetTotalQuantity(req.ingredient);    
        }
        else
        {
            Debug.Log("Item không tồn tại trong inventory");
            ownedNumber = 0;
        }
        // Hiện tại chưa lấy ra được ta cứ để auto = 0
        amountText.text = $"{ownedNumber}/{req.requiredAmount}";
        // Nếu thiếu thì đổi màu chữ đỏ
        amountText.color = ownedNumber >= req.requiredAmount ? Color.white : Color.red;
    }
}

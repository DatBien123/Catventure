using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FoodSelectionUI : MonoBehaviour
{
    public FoodSelection foodSelection;
    // UI List Food (Danh sách các món ăn có thể nấu bên trái)


    // UI Food Information (Bảng thông tin món ăn bên phải)
    [Header("Food Information")]
    public Text foodNameText;
    public Text ownedNumberText;
    public Image foodImage;
    public Text foodDescriptionText;

    [Header("Ingredients")]
    public Transform ingredientsRequired; // object  chứa các nguyên liệu để chứa 
    public GameObject ingredientItemUIPrefab; // prefab của 1 item nguyên liệu

    
    public void ShowFoodInformation(DishSO foodData)
    {
        foodSelection.currentFood = foodData;   
        foodNameText.text = foodData.dishName;

        int ownedNumber = 0;

        // Kiểm tra item có tồn tại không
        if (foodSelection.zera.Inventory.CheckItemExist(foodData))
        {
            ownedNumber = foodSelection.zera.Inventory.GetTotalQuantity(foodData);
        }
        else
        {
            ownedNumber = 0;
        }
        ownedNumberText.text = $"SỞ HỮU: {ownedNumber}";

        foodImage.sprite = foodData.icon;
        foodDescriptionText.text = foodData.dishDescription;

        // Xoá sạch UI nguyên liệu cũ
        foreach (Transform child in ingredientsRequired)
        {
            Destroy(child.gameObject);
        }
        // Ta spawn ra các nguyên liệu yêu cầu bằng Instatiate
        foreach(var ingredientReq in foodData.ingredients)
        {
            GameObject ingredientItem = Instantiate(ingredientItemUIPrefab, ingredientsRequired);

            // lấy ra component để set data
            IngredientItemUI ui = ingredientItem.GetComponent<IngredientItemUI>();
            ui.Setup(ingredientReq);
        }
    }
}

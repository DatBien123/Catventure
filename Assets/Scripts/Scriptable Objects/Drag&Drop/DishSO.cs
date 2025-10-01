using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IngredientRequirement
{
    public SO_Consumable ingredient; // nguyên liệu
    public int requiredAmount;      // số lượng yêu cầu
}

[CreateAssetMenu(fileName = "DishSO", menuName = "Scriptable Objects/DishSO")]
public class DishSO : SO_Consumable
{
    public string dishName; // tên của món ăn
    public Sprite icon; // Hình ảnh của món ăn
    [TextArea]
    public string dishDescription; // nội dung mô tả kỹ hơn của món ăn
    public List<IngredientRequirement> ingredients; // danh sách các nguyên liệu cần thiết để được phép nấu món này
    public CookingRecipeSO recipe; // công thức để nấu ra món này
    public GameObject foodPrefab; // UI hình ảnh món ăn đã được sắp xếp hình ảnh ở trước đúng vị trí từng nguyên liệu để dùng cho minigame sắp xếp ra bát, ra đĩa để hoàn thành món ăn gì dods
}

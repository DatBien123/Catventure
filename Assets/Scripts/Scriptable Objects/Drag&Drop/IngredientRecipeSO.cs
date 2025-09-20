using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientRecipeSO", menuName = "Scriptable Objects/Ingredient Recipe")]
public class IngredientRecipeSO : ScriptableObject
{
    public string recipeName; // tên công thức
    public List<IngredientSO> requiredIngredients; // danh sách nguyên liệu / nguyên tố
    public float cookingTime; // thời gian chế tạo/nấu
    public float reward; // số lượng phần thưởng nhận được
    public DishSO result; // kết quả tạo ra

}
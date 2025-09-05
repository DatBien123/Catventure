using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipeSO", menuName = "Scriptable Objects/Crafting Recipe")]
public class CraftingRecipeSO : ScriptableObject
{
    public string recipeName; // tên công thức
    public List<CraftingItemSO> requiredItems; // danh sách nguyên liệu / nguyên tố
    public float craftingTime; // thời gian chế tạo/nấu
    public float reward; // số lượng phần thưởng nhận được
    public CraftingResultSO result; // kết quả tạo ra

}
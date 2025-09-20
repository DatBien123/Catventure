using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipeSO", menuName = "Scriptable Objects/CookingRecipeSO")]
public class CookingRecipeSO : ScriptableObject
{
    public string recipeName; // tên công thức của món ăn
    public CookingStepSO[] steps; // đây là một chuỗi các minigame nhỏ hơn cần hoàn thành để xong món ăn
    public float reward; // số lượng phần thưởng nhận được
    public DishSO dishResult; // món ăn xong khi hoàn thành nấu

}

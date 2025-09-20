using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DishSO", menuName = "Scriptable Objects/DishSO")]
public class DishSO : ScriptableObject
{
    public string dishName; // tên của món ăn
    public Sprite icon; // Hình ảnh của món ăn
    [TextArea]
    public string dishDescription; // nội dung mô tả kỹ hơn của món ăn
}

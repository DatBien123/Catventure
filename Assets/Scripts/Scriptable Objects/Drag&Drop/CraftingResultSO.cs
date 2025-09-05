using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CraftingResultSO", menuName = "Scriptable Objects/CraftingResultSO")]
public class CraftingResultSO : ScriptableObject
{
    public string resultName; // tên của món ăn
    public Sprite icon; // Hình ảnh của món ăn
    [TextArea]
    public string resultDescription; // nội dung mô tả kỹ hơn của món ăn
}

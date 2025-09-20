using UnityEngine;
 
// các công đoạn để nấu 1 món ăn StepType
public enum StepType
{
    Cutting, // thái/cắt thịt, rau các thứ
    Stirring, // khuấy đều dành cho minigame khuấy trứng ấy
    Mixing, // trộn đều các nguyên liệu trong bát cho lẫn lộn
    Frying, // Chiên/Rán nguyên liệu trên 1 cái chảo
    Grilling, // Nướng món ăn trên lửa
    BoilingStirring, // Luộc và khuấy kết hợp là dành cho minigame kiểu nấu nước dùng
    Sprinkling, // rắc gia vị
    Assembling, // Múc nguyên liệu ra bát dùng món ăn như phở, bún, ... 
    Decorating // trang trí cho các món như bánh ngọt các thứ

}
[CreateAssetMenu(fileName = "CookingStepSO", menuName = "Scriptable Objects/CookingStepSO")]
public class CookingStepSO : ScriptableObject
{
    public string stepName; // tên công đoạn
    public StepType stepType; // công đoạn tương ứng với minigame đó
    public IngredientSO[] requiredIngredients; // các nguyên liệu cần trong minigame này
    public Sprite instructionImage; // ảnh hướng dẫn chơi công đoạn này chẳng hạn
    public float timeRequired; // thời gian tối thiểu cần để chơi xong minigame này
}

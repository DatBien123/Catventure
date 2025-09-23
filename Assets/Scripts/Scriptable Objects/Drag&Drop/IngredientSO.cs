using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientProcessingData
{
    public IngredientCuttingData cuttingData;
    public IngredientBoilingStirringData boilingStirringData;
    public IngredientAssemblingData assemblingData;
}
[System.Serializable]
public class IngredientCuttingData
{
    public StepType stepType;
    public int requiredCuts;       // số lần thao tác (cắt, khuấy, trộn...)
    public Sprite[] icons; // tập hợp nhiều ảnh cắt ra sử dụng cho công đoạn Cut
    public Sprite processSprite;   // sprite sau mỗi thao tác

}
[System.Serializable]
public class IngredientBoilingStirringData
{
    public StepType stepType;
    public Sprite icon;
    public float boilingTime;
}
[System.Serializable]

public class IngredientAssemblingData
{
    public StepType stepType;
    public Sprite icon;
}

[CreateAssetMenu(fileName = "IngredientSO", menuName = "Scriptable Objects/Ingredient")]
public class IngredientSO : SO_Consumable
{
    public string ingredientName; // tên tiếng Anh
    public Sprite icon; // hình ảnh
    public AudioClip pronunciation; // âm thanh phát âm khi kéo đúng
     // Danh sách cách xử lý trong từng step khác nhau
    public IngredientProcessingData processing;    //public GameObject model; ta có thể thêm prefab 3d hoặc sprite để spawn trong minigame 
}

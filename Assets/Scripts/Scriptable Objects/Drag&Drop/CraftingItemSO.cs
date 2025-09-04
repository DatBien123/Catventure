using UnityEngine;

[CreateAssetMenu(fileName = "CraftingItemSO", menuName = "Scriptable Objects/Crafting Item")]
public class CraftingItemSO : ScriptableObject
{
    public string itemName; // tên tiếng Anh
    public Sprite icon; // hình ảnh
    public AudioClip pronunciation; // âm thanh phát âm khi kéo đúng
    public DragdropItemType itemType; // loại item (Elemental, Ingredient, etc.)
}

public enum DragdropItemType
{
    Elemental,
    Ingredient
}
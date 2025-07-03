using UnityEngine;

[System.Serializable]
public struct FieldData
{
    public Sprite backgroundSprite;
    public Color backgroundColor;

    public Color textColor;
}

[System.Serializable]
public struct ButtonData
{
    public string buttonName;
    public Sprite mainIcon;
    public FieldData selectFieldData;
    public FieldData deselectdFieldData;
}

[CreateAssetMenu(fileName = "Button Data", menuName = "Inventory System/Graphic Data/Button Data")]
public class SO_ButtonData : ScriptableObject
{
    public ButtonData buttonData;

}

using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class GameDataSO : ScriptableObject
{
    public string selectedCategoryName;
    public BoardDataSO selectedBoardData;
}

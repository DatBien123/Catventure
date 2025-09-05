using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    public GameDataSO currentGameData;
    public GameLevelData levelData;

    private void Awake() {
        SelectSequentialBoardData();
    }

    private void SelectSequentialBoardData() {
        foreach (var data in levelData.data)
        {
            if (data.categoryName == currentGameData.selectedCategoryName) {
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);

                if (boardIndex < data.boardData.Count) {
                    currentGameData.selectedBoardData = data.boardData[boardIndex];
                }
                else {
                    var randomIndex = Random.Range(0, data.boardData.Count);
                    currentGameData.selectedBoardData = data.boardData[randomIndex];
                }

            }
        }
    }

}

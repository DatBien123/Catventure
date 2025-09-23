using UnityEngine;

public class FixSwordGameManager : MonoBehaviour
{
    public static FixSwordGameManager Instance { get; private set; }

    [SerializeField] private RectTransform UIWinPanel;
    [SerializeField] private int pointsTowin;

    private int currentPoints;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void AddPoint() { 
        currentPoints++;
        CheckWin();
    }

    public void CheckWin() {
        if (currentPoints >= pointsTowin) {
            UIWinPanel.gameObject.SetActive(true);
        }
    }
}

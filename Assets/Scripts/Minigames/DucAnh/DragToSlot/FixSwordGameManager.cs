using System.Collections;
using UnityEngine;

public class FixSwordGameManager : MonoBehaviour
{
    public static FixSwordGameManager Instance { get; private set; }

    [SerializeField] private RectTransform UIWinPanel;
    [SerializeField] private VictoryRewardScreen VictoryRewardScreen;
    [SerializeField] private int pointsTowin;
    [SerializeField] private int reward;

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
            StartCoroutine(OpenWinPanel());
        }
    }

    IEnumerator OpenWinPanel() {
        yield return new WaitForSeconds(1.0f);
        VictoryRewardScreen.ShowRewardFITB(reward, 3);
    }


}

using System.Collections;
using UnityEngine;

public class DongSonDrumPatternGameManager : MonoBehaviour
{
    public static DongSonDrumPatternGameManager Instance { get; private set; }

    [SerializeField] private SwordChest Chest;
    [SerializeField] private GameObject PuzzleLayers;
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

    public void ZoomInChest() {
        StartCoroutine(IEZoomInChest());
    }

    private IEnumerator IEZoomInChest() {
        int defaultVCamIndex = 0;
        int zoominVCamIndex = 1;

        CameraManager.Instance.SwitchCamera(zoominVCamIndex);

        DongSonDrumPatternUIManager.Instance.FadeTransition();

        yield return new WaitForSeconds(1);

        CameraManager.Instance.SwitchCamera(defaultVCamIndex);

        Chest.gameObject.SetActive(false);
        PuzzleLayers.SetActive(true);
    }


    public void AddPoint() {
        currentPoints++;
        CheckWin();
    }

    public void CheckWin() {
        if (currentPoints >= pointsTowin) {
            OnWinGame();
        }
    }

    void OnWinGame() {
        DongSonDrumPatternUIManager.Instance.OpenWinPanel(reward);
    }


}

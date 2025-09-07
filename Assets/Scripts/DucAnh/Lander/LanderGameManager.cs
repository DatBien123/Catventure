using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LanderGameManager : MonoBehaviour {


    public static LanderGameManager Instance { get; private set; }


    private static int levelNumber = 1;
    private static int totalScore = 0;


    public static void ResetStaticData() {
        levelNumber = 1;
        totalScore = 0;
    }


    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    [SerializeField] private List<LanderGameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;


    private int score;
    private float time;
    private bool isTimerActive;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;

        LoadCurrentLevel();
    }

    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e) {
        PauseUnpauseGame();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e) {
        isTimerActive = e.state == Lander.State.Normal;

        if (e.state == Lander.State.Normal) {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
        }
    }

    private void Update() {
        if (isTimerActive) {
            time += Time.deltaTime;
        }
    }

    private void LoadCurrentLevel() {
        LanderGameLevel gameLevel = GetGameLevel();
        LanderGameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }

    private LanderGameLevel GetGameLevel() {
        foreach (LanderGameLevel gameLevel in gameLevelList) {
            if (gameLevel.GetLevelNumber() == levelNumber) {
                return gameLevel;
            }
        }
        return null;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e) {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e) {
        AddScore(500);
    }

    public void AddScore(int addScoreAmount) {
        score += addScoreAmount;
        Debug.Log(score);
    }

    public int GetScore() {
        return score;
    }

    public float GetTime() {
        return time;
    }

    public int GetTotalScore() {
        return totalScore;
    }

    public void GoToNextLevel() {
        levelNumber++;
        totalScore += score;

        if (GetGameLevel() == null) {
            // No more levels
            //LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderGameOverScene);
        } else {
            // We still have more levels
            LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderGameScene);
        }
    }

    public void RetryLevel() {
        LanderSceneLoader.LoadScene(LanderSceneLoader.Scene.LanderGameScene);
    }

    public int GetLevelNumber() {
        return levelNumber;
    }

    public void PauseUnpauseGame() {
        if (Time.timeScale == 1f) {
            PauseGame();
        } else {
            UnpauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame() {
        Time.timeScale = 1f;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }

}
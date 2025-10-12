using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SongManager : MonoBehaviour {
    [Header("Note Settings")]
    [SerializeField] private GameObject notePrefab;
    [SerializeField] private float noteSpeed = 5f;
    [SerializeField] private Transform[] laneSpawnPoints; // 4 spawn points for 4 lanes

    [Header("Song Settings")]
    [SerializeField] private int reward = 100;
    [SerializeField] private float bpm = 85f;
    [SerializeField] private float firstBeatTime = 13f;
    [SerializeField] private float lastBeatTime = 20f;
    [SerializeField] private AudioSource audioSource;

    [Header("UI")]
    [SerializeField] private Image durationLine;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Text accuracyText;
    private Vector3 accuracyTextPos;

    //the current position of the song (in seconds)
    private float songPosition;

    //how much time (in seconds) has passed since the song started
    float dsptimesong;

    private float beatInterval;
    private float nextBeatTime;
    private int score = 0;
    private int combo = 0;
    private int maxCombo = 0;
    private int perfectHits = 0;
    private int greatHits = 0;
    private int goodHits = 0;
    private int missedNotes = 0;
    private int star = 3;
    private bool isPlaying;

    void Start() {
        // Calculate beat interval (60 seconds / BPM)
        beatInterval = 60f / bpm;
        nextBeatTime = Time.time + firstBeatTime; // Give firstBeatTime seconds before first note

        //record the time when the song starts
        dsptimesong = (float) AudioSettings.dspTime;

        if (audioSource != null && audioSource.clip != null) {
            audioSource.Play();
            isPlaying = true;
        }

        UpdateUI();
        accuracyTextPos = accuracyText.transform.position;
    }

    void Update() {
        if (!isPlaying) return;

        //calculate the position in seconds
        songPosition = (float) (AudioSettings.dspTime - dsptimesong);
        durationLine.fillAmount = songPosition / audioSource.clip.length;

        // Spawn notes on beat
        if ( (Time.time >= nextBeatTime) && (songPosition < audioSource.clip.length - lastBeatTime) ) {
            SpawnNote();
            nextBeatTime += beatInterval;
        }

        if (songPosition >= audioSource.clip.length) {
            OnSongEnd();
        }
    }

    private void OnSongEnd() {
        isPlaying = false;
        RhythmGameManager.Instance.OnFinishGame(reward, star, perfectHits, greatHits, goodHits, missedNotes, maxCombo, score);
    }

    void SpawnNote() {
        // Randomly choose a lane
        int randomLane = Random.Range(0, 4);

        GameObject note = Instantiate(notePrefab, laneSpawnPoints[randomLane].position, Quaternion.identity);
        NoteController noteController = note.GetComponent<NoteController>();
        noteController.lane = randomLane;
        noteController.speed = noteSpeed;
    }

    public void NoteHit(int points, string accuracy) {
        score += points;
        combo++;

        if (combo > maxCombo) {
            maxCombo = combo;
        }

        switch (accuracy) {
            case Constants.PERFECT_ACCURACY:
                perfectHits++;
                break;
            case Constants.GREAT_ACCURACY:
                greatHits++;
                break;
            case Constants.GOOD_ACCURACY:
                goodHits++;
                break;
        }

        ShowAccuracyFeedback(accuracy);
        UpdateUI();
    }

    public void NoteMissed() {
        combo = 0;
        missedNotes++;
        ShowAccuracyFeedback(Constants.MISS_ACCURACY);
        UpdateUI();
    }

    void ShowAccuracyFeedback(string feedback) {
        if (accuracyText != null) {
            accuracyText.transform.position = accuracyTextPos; // reset 
            accuracyText.text = feedback;
            accuracyText.color = GetAccuracyColor(feedback);
            accuracyText.transform.DOMoveY(accuracyTextPos.y + 20f, 0.5f);
            CancelInvoke("ClearAccuracyText");
            Invoke("ClearAccuracyText", 0.5f);
        }
    }

    void ClearAccuracyText() {
        if (accuracyText != null) {
            accuracyText.text = "";
        }
    }

    Color GetAccuracyColor(string feedback) {
        switch (feedback) {
            case Constants.PERFECT_ACCURACY: return Color.orange;
            case Constants.GREAT_ACCURACY: return Color.cyan;
            case Constants.GOOD_ACCURACY: return Color.green;
            case Constants.MISS_ACCURACY: return Color.gray;
            default: return Color.white;
        }
    }

    void UpdateUI() {
        if (scoreText != null) {
            scoreText.text = $"Điểm: {score}";
        }

        if (comboText != null) {
            comboText.text = combo > 0 ? $"Combo: {combo}" : "";
        }
    }
}
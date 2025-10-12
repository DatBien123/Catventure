using UnityEngine;

public class LaneController : MonoBehaviour {
    [SerializeField] private int laneNumber; // 0, 1, 2, or 3
    [SerializeField] private KeyCode inputKey;
    [SerializeField] private Transform hitZone;
    [SerializeField] private float hitRange = 1f;

    [SerializeField] private ParticleSystem perfectParticle;
    [SerializeField] private ParticleSystem greatParticle;
    [SerializeField] private ParticleSystem goodParticle;
    private ParticleSystem currentParticle;

    private SongManager songManager;

    void Start() {
        songManager = FindFirstObjectByType<SongManager>();
    }

    void Update() {
        if (Input.GetKeyDown(inputKey)) {
            CheckForNoteHit();
        }
    }

    private void OnMouseDown() {
        CheckForNoteHit();
    }

    void CheckForNoteHit() {
        NoteController[] notes = FindObjectsByType<NoteController>(FindObjectsSortMode.None);
        NoteController closestNote = null;
        float closestDistance = hitRange;

        foreach (NoteController note in notes) {
            if (note.lane == laneNumber) {
                float distance = Mathf.Abs(note.transform.position.y - hitZone.position.y);

                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestNote = note;
                }
            }
        }

        if (closestNote != null) {
            // Calculate accuracy
            string accuracy = Constants.PERFECT_ACCURACY;
            int score = 100;
            currentParticle = perfectParticle;

            if (closestDistance > 0.4f) {
                accuracy = Constants.GOOD_ACCURACY;
                score = 50;
                currentParticle = goodParticle;
            }
            else if (closestDistance > 0.2f) {
                accuracy = Constants.GREAT_ACCURACY;
                score = 75;
                currentParticle = greatParticle;
            }

            currentParticle.Play();
            songManager.NoteHit(score, accuracy);
            closestNote.HitNote();
        }
        else {
            songManager.NoteMissed();
        }
    }
}
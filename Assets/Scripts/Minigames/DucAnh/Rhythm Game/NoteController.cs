using UnityEngine;

public class NoteController : MonoBehaviour {
    public float speed = 5f;
    public int lane; // 0, 1, 2, or 3

    private SongManager songManager;

    void Start() {
        songManager = FindFirstObjectByType<SongManager>();
    }

    void Update() {
        // Move note downward
        transform.position += Vector3.down * speed * Time.deltaTime;

        // Destroy if below screen
        if (transform.position.y < -6f) {
            if (songManager != null) {
                songManager.NoteMissed();
            }
            Destroy(gameObject);
        }
    }

    public void HitNote() {
        Destroy(gameObject);
    }
}
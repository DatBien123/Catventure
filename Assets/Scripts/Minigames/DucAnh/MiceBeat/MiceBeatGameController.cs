using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiceBeatGameController : MonoBehaviour {
    [Header("Game Settings")]
    [SerializeField] private float noteSpeed = 5f;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float hitWindowTolerance = 0.5f;

    [Header("Lane Configuration")]
    [SerializeField] private int numberOfLanes = 4;
    [SerializeField] private float laneWidth = 2f;
    [SerializeField] private float hitLineY = -3f;
    [SerializeField] private float spawnY = 5f;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private int currentPlayerLane = 1; // Start in lane 1 (0-indexed)
    private bool isHitting = false;

    [Header("Visuals")]
    [SerializeField] private Color laneColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    [SerializeField] private Color hitLineColor = Color.green;

    private List<GameObject> lanes = new List<GameObject>();
    private List<Queue<GameObject>> laneNotes = new List<Queue<GameObject>>();
    private float nextSpawnTime;
    private int score = 0;
    private int combo = 0;

    // Input keys for each lane
    private KeyCode[] laneKeys = { KeyCode.D, KeyCode.F, KeyCode.J, KeyCode.K };

    void Start() {
        SetupCamera();
        CreateLanes();
        CreateHitLine();
        CreatePlayer();
        InitializeLaneQueues();
        nextSpawnTime = Time.time + spawnInterval;
    }

    void SetupCamera() {
        Camera.main.orthographicSize = 5f;
        Camera.main.transform.position = new Vector3(0, 0, -10);
    }

    void CreateLanes() {
        float startX = -(numberOfLanes - 1) * laneWidth / 2f;

        for (int i = 0; i < numberOfLanes; i++) {
            // Create lane background
            GameObject lane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            lane.name = "Lane_" + i;
            lane.transform.position = new Vector3(startX + i * laneWidth, 0, 1);
            lane.transform.localScale = new Vector3(laneWidth * 0.9f, 10f, 1);

            // Set lane color
            Renderer renderer = lane.GetComponent<Renderer>();
            renderer.material.color = laneColor;

            // Add lane marker at bottom
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Quad);
            marker.name = "LaneMarker_" + i;
            marker.transform.position = new Vector3(startX + i * laneWidth, hitLineY, 0.5f);
            marker.transform.localScale = new Vector3(laneWidth * 0.8f, 0.3f, 1);
            marker.GetComponent<Renderer>().material.color = Color.gray;

            // Create key indicator
            GameObject keyIndicator = new GameObject("KeyIndicator_" + i);
            keyIndicator.transform.position = new Vector3(startX + i * laneWidth, hitLineY - 1f, 0);
            TextMesh textMesh = keyIndicator.AddComponent<TextMesh>();
            textMesh.text = laneKeys[i].ToString();
            textMesh.fontSize = 24;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = Color.white;

            lanes.Add(lane);
        }
    }

    void CreateHitLine() {
        GameObject hitLine = GameObject.CreatePrimitive(PrimitiveType.Quad);
        hitLine.name = "HitLine";
        hitLine.transform.position = new Vector3(0, hitLineY, 0.9f);
        hitLine.transform.localScale = new Vector3(numberOfLanes * laneWidth, 0.1f, 1);
        hitLine.GetComponent<Renderer>().material.color = hitLineColor;
    }

    void CreatePlayer() {
        // Create player (cat with hammer placeholder)
        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.localScale = new Vector3(0.8f, 1.2f, 0.8f);

        // Position player under the lanes
        float startX = -(numberOfLanes - 1) * laneWidth / 2f;
        player.transform.position = new Vector3(startX + currentPlayerLane * laneWidth, hitLineY - 2f, 0);

        // Make player blue
        player.GetComponent<Renderer>().material.color = Color.blue;

        // Create hammer (simple cube for now)
        GameObject hammer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        hammer.name = "Hammer";
        hammer.transform.parent = player.transform;
        hammer.transform.localPosition = new Vector3(0.6f, 0.3f, 0);
        hammer.transform.localScale = new Vector3(0.4f, 0.8f, 0.3f);
        hammer.GetComponent<Renderer>().material.color = Color.brown;
    }

    void InitializeLaneQueues() {
        for (int i = 0; i < numberOfLanes; i++) {
            laneNotes.Add(new Queue<GameObject>());
        }
    }

    void Update() {
        HandleInput();
        SpawnNotes();
        UpdateNotes();
        UpdatePlayerAnimation();
    }

    void HandleInput() {
        for (int i = 0; i < laneKeys.Length && i < numberOfLanes; i++) {
            if (Input.GetKeyDown(laneKeys[i])) {
                MovePlayerToLane(i);
                CheckHit(i);
                TriggerHitAnimation();
            }
        }
    }

    void MovePlayerToLane(int laneIndex) {
        currentPlayerLane = laneIndex;
        float startX = -(numberOfLanes - 1) * laneWidth / 2f;
        float targetX = startX + laneIndex * laneWidth;

        // Smooth movement to lane
        StartCoroutine(MovePlayer(targetX));
    }

    IEnumerator MovePlayer(float targetX) {
        float startX = player.transform.position.x;
        float moveTime = 0.1f;
        float elapsedTime = 0;

        while (elapsedTime < moveTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;
            float newX = Mathf.Lerp(startX, targetX, t);
            player.transform.position = new Vector3(newX, player.transform.position.y, player.transform.position.z);
            yield return null;
        }
    }

    void TriggerHitAnimation() {
        isHitting = true;
        StartCoroutine(HammerAnimation());
    }

    IEnumerator HammerAnimation() {
        Transform hammer = player.transform.Find("Hammer");
        if (hammer != null) {
            // Rotate hammer for hit animation
            float animTime = 0.2f;
            float elapsedTime = 0;

            while (elapsedTime < animTime) {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animTime;
                float rotation = Mathf.Lerp(0, -45, t);
                hammer.localRotation = Quaternion.Euler(0, 0, rotation);
                yield return null;
            }

            // Return to original position
            elapsedTime = 0;
            while (elapsedTime < animTime) {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animTime;
                float rotation = Mathf.Lerp(-45, 0, t);
                hammer.localRotation = Quaternion.Euler(0, 0, rotation);
                yield return null;
            }
        }
        isHitting = false;
    }

    void CheckHit(int laneIndex) {
        if (laneIndex >= laneNotes.Count) return;

        Queue<GameObject> notes = laneNotes[laneIndex];
        if (notes.Count > 0) {
            GameObject firstNote = notes.Peek();
            float distance = Mathf.Abs(firstNote.transform.position.y - hitLineY);

            if (distance <= hitWindowTolerance) {
                // Hit!
                notes.Dequeue();
                Destroy(firstNote);
                score += 100;
                combo++;
                Debug.Log($"Hit! Score: {score}, Combo: {combo}");

                // Visual feedback
                StartCoroutine(HitEffect(laneIndex));
            }
            else {
                // Miss
                combo = 0;
                Debug.Log("Miss! Too early/late");
            }
        }
    }

    IEnumerator HitEffect(int laneIndex) {
        float startX = -(numberOfLanes - 1) * laneWidth / 2f;
        GameObject effect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        effect.transform.position = new Vector3(startX + laneIndex * laneWidth, hitLineY, -1);
        effect.transform.localScale = Vector3.one * 0.5f;
        effect.GetComponent<Renderer>().material.color = Color.yellow;

        float duration = 0.3f;
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float scale = Mathf.Lerp(0.5f, 1.5f, elapsedTime / duration);
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            effect.transform.localScale = Vector3.one * scale;

            Color color = effect.GetComponent<Renderer>().material.color;
            color.a = alpha;
            effect.GetComponent<Renderer>().material.color = color;

            yield return null;
        }

        Destroy(effect);
    }

    void SpawnNotes() {
        if (Time.time >= nextSpawnTime) {
            // Randomly choose a lane
            int laneIndex = Random.Range(0, numberOfLanes);
            SpawnNote(laneIndex);

            // Set next spawn time with some randomization
            nextSpawnTime = Time.time + Random.Range(spawnInterval * 0.5f, spawnInterval * 1.5f);
        }
    }

    void SpawnNote(int laneIndex) {
        float startX = -(numberOfLanes - 1) * laneWidth / 2f;

        // Create note (mouse placeholder)
        GameObject note = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        note.name = "Note_Lane" + laneIndex;
        note.transform.position = new Vector3(startX + laneIndex * laneWidth, spawnY, 0);
        note.transform.localScale = Vector3.one * 0.6f;

        // Make it red (representing mice)
        note.GetComponent<Renderer>().material.color = Color.red;

        // Add to lane queue
        laneNotes[laneIndex].Enqueue(note);
    }

    void UpdateNotes() {
        // Move all notes down
        for (int i = 0; i < laneNotes.Count; i++) {
            List<GameObject> toRemove = new List<GameObject>();

            foreach (GameObject note in laneNotes[i]) {
                if (note != null) {
                    note.transform.position += Vector3.down * noteSpeed * Time.deltaTime;

                    // Remove notes that went too far
                    if (note.transform.position.y < hitLineY - 2f) {
                        toRemove.Add(note);
                        combo = 0;
                        Debug.Log("Missed note!");
                    }
                }
            }

            // Clean up missed notes
            foreach (GameObject note in toRemove) {
                if (laneNotes[i].Contains(note)) {
                    laneNotes[i].Dequeue();
                    Destroy(note);
                }
            }
        }
    }

    void UpdatePlayerAnimation() {
        // Add idle animation or bobbing effect when not hitting
        if (!isHitting) {
            float bobAmount = Mathf.Sin(Time.time * 3f) * 0.05f;
            Vector3 pos = player.transform.position;
            pos.y = hitLineY - 2f + bobAmount;
            player.transform.position = pos;
        }
    }

    void OnGUI() {
        // Display score and combo
        GUI.skin.label.fontSize = 24;
        GUI.Label(new Rect(10, 10, 200, 30), "Score: " + score);
        GUI.Label(new Rect(10, 40, 200, 30), "Combo: " + combo);

        // Display controls
        GUI.Label(new Rect(10, Screen.height - 40, 400, 30), "Controls: D, F, J, K for lanes");
    }
}
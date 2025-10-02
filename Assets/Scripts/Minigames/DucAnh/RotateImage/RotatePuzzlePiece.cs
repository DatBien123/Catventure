using System.Collections;
using UnityEngine;

public class RotatePuzzlePiece : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rotateClip;
    [SerializeField] private AudioClip completeClip;

    [SerializeField] private float correctRotationZ;
    [SerializeField] private float rotateSpeed = 5f;

    [SerializeField] private Material defaultMat;
    [SerializeField] private Renderer spriteRenderer;
    private Color startColor = Color.white;
    private Color endColor = Color.gray;
    private float changeDuration = 1f;

    private bool rotating;
    private bool completed;

    private float randomRotationZ;

    private void Awake() {
        randomRotationZ = Random.Range(correctRotationZ - 30f + 360f, correctRotationZ + 30f); // offset = 30 degree
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, randomRotationZ);

        StartCoroutine(LerpColor());
    }

    void Update() {
        if (completed) return;

        if (!rotating) return;

        if (rotating) {
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 45f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        }

    }

    private void OnMouseDown() {
        if (completed) return;

        rotating = true;

        audioSource.Play();
    }

    private void OnMouseUp() {
        if (completed) return;

        rotating = false;

        audioSource.Stop();

        float currentZ = transform.eulerAngles.z;

        // Handle wrap-around (e.g., 359° ≈ -1° near 0°)
        float difference = Mathf.DeltaAngle(currentZ, correctRotationZ);

        if (Mathf.Abs(difference) <= 5f) {
            StopCoroutine(LerpColor());
            spriteRenderer.material = defaultMat;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, correctRotationZ); 
            audioSource.PlayOneShot(completeClip);
            completed = true;
            DongSonDrumPatternGameManager.Instance.AddPoint();
            this.enabled = false;
        }
    }

    IEnumerator LerpColor() {
        while (!completed) {
            // t goes 0 → 1 → 0 over time
            float t = Mathf.PingPong(Time.time / changeDuration, 1f);

            spriteRenderer.material.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }
    }

}

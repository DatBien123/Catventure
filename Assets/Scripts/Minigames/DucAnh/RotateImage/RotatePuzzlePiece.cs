using UnityEngine;

public class RotatePuzzlePiece : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rotateClip;
    [SerializeField] private AudioClip completeClip;

    [SerializeField] private float correctRotationZ;
    [SerializeField] private float rotateSpeed = 5f;

    private bool isCurrentLayer;
    private bool rotating;
    private bool completed;

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
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, correctRotationZ); 
            audioSource.PlayOneShot(completeClip);
            completed = true;
            DongSonDrumPatternGameManager.Instance.AddPoint();
        }
    }

}

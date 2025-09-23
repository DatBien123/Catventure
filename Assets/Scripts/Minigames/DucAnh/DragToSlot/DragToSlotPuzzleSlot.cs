using UnityEngine;

public class DragToSlotPuzzleSlot : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _completeClip;

    public void PlacedCorrectly() {
        _audioSource.PlayOneShot(_completeClip);
    }
}

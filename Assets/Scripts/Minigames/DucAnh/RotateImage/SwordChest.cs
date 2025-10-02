using UnityEngine;

public class SwordChest : MonoBehaviour
{
    [SerializeField] private AudioClip zoominClip;
    [SerializeField] private AudioSource audioSource;

    private bool clicked;

    private void OnMouseDown() {
        if (clicked) return;
        clicked = true;
        DongSonDrumPatternGameManager.Instance.ZoomInChest();
        audioSource.PlayOneShot(zoominClip);
    }


}

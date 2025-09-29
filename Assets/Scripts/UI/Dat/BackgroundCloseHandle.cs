using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundCloseHandle : MonoBehaviour, IPointerClickHandler
{
    public GameObject TargetUI;
    public AudioManager AudioManager;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager != null)
        {
            AudioManager.PlaySFX("Close");
        }

        TargetUI.gameObject.SetActive(false);
    }
}

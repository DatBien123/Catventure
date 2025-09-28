using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundCloseHandle : MonoBehaviour, IPointerClickHandler
{
    public GameObject TargetUI;
    public void OnPointerClick(PointerEventData eventData)
    {
        TargetUI.gameObject.SetActive(false);
    }
}

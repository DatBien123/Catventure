using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundTouchHandle : MonoBehaviour, IPointerClickHandler
{
    public FarmManager FarmManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Touched EMPTY background panel");
        FarmManager.gameObject.SetActive(false);
    }
}

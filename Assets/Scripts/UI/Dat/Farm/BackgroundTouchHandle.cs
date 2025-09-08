using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundTouchHandle : MonoBehaviour, IPointerClickHandler
{
    public GameObject ToolbarCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Touched EMPTY background panel");
        ToolbarCanvas.SetActive(false);
    }
}

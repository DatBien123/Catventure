using UnityEngine;
using UnityEngine.EventSystems;

public class UITutorialCanvas : MonoBehaviour, IPointerClickHandler {
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        gameObject.SetActive(false);
    }
}

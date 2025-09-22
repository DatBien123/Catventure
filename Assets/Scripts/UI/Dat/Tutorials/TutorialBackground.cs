using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialBackground : MonoBehaviour, IPointerClickHandler
{
    public bool isReceiveTouch = false;
    [Header("Components")]
    public Image ImageComponent;

    [Header("References")]
    public TutorialManager TutorialManager;

    private void Start()
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked To Tutorial Background");
            TutorialManager.AllowNextStep = true;

    }

    public void SetCanReceiveTouch(bool canReceiveTouch)
    {
        isReceiveTouch = canReceiveTouch;
        ImageComponent.raycastTarget = isReceiveTouch;
    }

}

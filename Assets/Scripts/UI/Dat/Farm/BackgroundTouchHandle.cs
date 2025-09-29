using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundTouchHandle : MonoBehaviour, IPointerClickHandler
{
    public FarmManager FarmManager;
    public bool isApplyingTouchBackground;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (FarmManager.TutorialManager.isApplyTutorials) return;
        Debug.Log("Touched EMPTY background panel");
        FarmManager.gameObject.SetActive(false);

        if(FarmManager.AudioManager != null)
        FarmManager.AudioManager.PlaySFX("Close");
    }
}

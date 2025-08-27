using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HomeButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PauseMenuScreen pauseMenu;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Pause button clicked");
        // Gọi hàm pause game
        PauseGame();
    }

    void PauseGame()
    {
        pauseMenu.Setup();
    }
}

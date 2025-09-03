using UnityEngine;

public class PauseMenuScreen : MonoBehaviour
{
    public void Setup()
    {
        this.gameObject.SetActive(true);
    }
    public void ResumeGame()
    {
        Debug.Log("Tiếp tục");
    }
    public void Quit()
    {
        Debug.Log("Thoát Game");
    }
}

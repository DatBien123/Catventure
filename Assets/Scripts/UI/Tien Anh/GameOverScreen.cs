using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void Restart() // hàm xử lý chơi lại khi player thua cuộc
    {
        Debug.Log("Chơi lại");
    }
    public void Quit() // hàm xử lý khi player muốn thoát ra ngoài màn hình menu hay gì đó
    {
        Debug.Log("Quit");
    }
    public void Setup()
    {

        this.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Game Over");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

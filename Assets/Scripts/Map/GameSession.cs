using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    public string LastSelectedMapName;
    public string LastSceneName; // 👈 Thêm biến này

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetLastScene(string sceneName)
    {
        LastSceneName = sceneName;
    }
}

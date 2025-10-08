using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    public string LastSelectedMapName;
    public string LastSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 👇 Đăng ký lắng nghe sự kiện khi scene thay đổi
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm này sẽ được gọi mỗi khi scene thay đổi
    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if(newScene.name != "Home Scene")
        {
            LastSceneName = newScene.name;
            Debug.Log($"[GameSession] Scene đã đổi sang: {LastSceneName}");
        }

    }

    private void OnDestroy()
    {
        // Hủy đăng ký để tránh lỗi khi GameSession bị destroy
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
}

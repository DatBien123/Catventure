using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    void Awake() {
        #if UNITY_ANDROID
            Application.targetFrameRate = targetFPS;
        #endif

        DontDestroyOnLoad(this);
    }
}

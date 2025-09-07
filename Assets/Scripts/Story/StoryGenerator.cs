using UnityEngine;
using UnityEngine.UI;

public class StoryGenerator : MonoBehaviour
{
    public Story StoryPrefab;

    [Header("Generator")]
    public StoryManager StoryManager;

    [Header("Buttons")]
    public Button Prev_Button;
    public Button Next_Button;

    [Header("Circle Settings")]
    public float radius = 5f; // bán kính vòng tròn
    public float radiusFactor = 16;

    [Header("Camera")]
    public Camera Camera;
    public float CameraDistanceFactor = 1.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f; // tốc độ xoay

    private Quaternion targetRotation; // rotation mục tiêu

    public Story CurrentStory;

    void Start()
    {
        radius = StoryManager.Stories.Count * radiusFactor / 3;

        GenerateStories();
        PositionCamera();

        // rotation ban đầu
        targetRotation = transform.rotation;

        // Gán sự kiện cho nút
        if (Prev_Button != null)
            Prev_Button.onClick.AddListener(OnPrevClicked);

        if (Next_Button != null)
            Next_Button.onClick.AddListener(OnNextClicked);
    }

    void Update()
    {
        // Xoay mượt về targetRotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );

        CheckStoryInFront();
    }

    // --- Các hàm xử lý nút ---
    void OnPrevClicked()
    {
        if (StoryManager == null) return;
        float step = 360f / StoryManager.Stories.Count;
        targetRotation *= Quaternion.Euler(0f, -step, 0f);
    }

    void OnNextClicked()
    {
        if (StoryManager == null) return;
        float step = 360f / StoryManager.Stories.Count;
        targetRotation *= Quaternion.Euler(0f, step, 0f);
    }

    void GenerateStories()
    {
        if (StoryManager == null || StoryPrefab == null) return;

        int count = StoryManager.Stories.Count;
        float angleStep = 360f / count;

        // Tính góc giữa Pivot -> Camera (theo trục Y)
        Vector3 dirToCamera = (Camera.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(dirToCamera.z, dirToCamera.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            // Góc xoay cho prefab i
            float angle = baseAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 pos = transform.position + new Vector3(
                Mathf.Cos(rad) * radius,
                0f,
                Mathf.Sin(rad) * radius
            );

            GameObject storyObj = Instantiate(StoryPrefab.gameObject, pos, Quaternion.identity, transform);
            storyObj.transform.LookAt(transform.position);
            storyObj.transform.rotation *= Quaternion.Euler(90f, 0f, 0f);

            storyObj.GetComponent<Story>().SetupStory(StoryManager.Stories[i]);
        }
    }

    void PositionCamera()
    {
        if (Camera == null) return;

        // Đặt camera trên mặt phẳng XZ, cách pivot 1.5 lần radius
        Vector3 offset = new Vector3(0f, 0f, -radius * CameraDistanceFactor);

        Camera.transform.position = transform.position + offset;

        Camera.transform.LookAt(transform.position);
    }

    void CheckStoryInFront()
    {
        if (Camera == null) return;

        // Lấy vị trí chính giữa camera (screen center)
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);

        // Tạo ray từ camera qua điểm giữa màn hình
        Ray ray = Camera.ScreenPointToRay(screenCenter);

        // Thực hiện raycast
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Kiểm tra object có component Story không
            Story story = hit.collider.GetComponent<Story>();
            if (story != null)
            {
                Debug.Log("Hit a Story: " + story.name);
                CurrentStory = story; // gán vào biến CurrentStory
            }
            else
            {
                Debug.Log("Hit something else: " + hit.collider.name);
            }
        }
    }
}

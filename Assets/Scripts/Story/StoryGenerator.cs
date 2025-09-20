using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Thêm namespace DOTween

public class StoryGenerator : MonoBehaviour
{
    public Story StoryPrefab;

    [Header("Generator")]
    public StoryManager StoryManager;
    public List<Story> StoryList;

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
    public float rotationDuration = 0.5f; // thời gian xoay (giây)
    private Quaternion targetRotation; // rotation mục tiêu

    public Story CurrentStory;

    public float delayShowUp = .5f;
    public float delayShowUpStories = .5f;
    public float waitTimeShowUpStory = .5f;

    public Animator Animator;

    Coroutine C_ShowUpStories;
    Coroutine C_RotateStories;

    void Start()
    {
        Animator.CrossFadeInFixedTime("Story_Open", 0.0f);
        radius = StoryManager.Stories.Count * radiusFactor / 3;
        Debug.Log($"Radius được tính: {radius}, CameraDistanceFactor: {CameraDistanceFactor}");

        GenerateStories();
        StartShowStories(delayShowUpStories);
        PositionCamera();

        // rotation ban đầu
        targetRotation = transform.rotation;

        // Gán sự kiện cho nút
        if (Prev_Button != null)
            Prev_Button.onClick.AddListener(() => StartRotateStories(-1));

        if (Next_Button != null)
            Next_Button.onClick.AddListener(() => StartRotateStories(1));
    }

    void Update()
    {
        // Xoay mượt về targetRotation (chỉ cần khi coroutine đang chạy)
        // Bỏ CheckStoryInFront khỏi Update để tối ưu
    }

    // --- Các hàm xử lý nút ---
    #region [Interact]
    void StartRotateStories(int direction)
    {
        if (StoryManager == null) return;
        if (C_RotateStories == null)
            C_RotateStories = StartCoroutine(RotateStories(direction));
    }

    IEnumerator RotateStories(int direction)
    {
        if (StoryManager.Stories.Count == 0) yield break;

        float step = 360f / StoryManager.Stories.Count;
        Debug.Log($"Góc xoay: {step} độ");
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, step * direction, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            yield return null;
        }

        // Đảm bảo rotation chính xác ở điểm cuối
        transform.rotation = endRotation;
        targetRotation = endRotation;

        C_RotateStories = null;
        // Gọi CheckStoryInFront khi xoay xong
        CheckStoryInFront();
    }

    #endregion

    #region [Positioned Stories]
    void PositionCamera()
    {
        if (Camera == null)
        {
            Debug.LogWarning("Camera bị null");
            return;
        }

        // Đặt camera trên mặt phẳng XZ, cách pivot 1.5 lần radius
        Vector3 offset = new Vector3(0f, 0f, -radius * CameraDistanceFactor);
        Debug.Log($"Camera được đặt tại: {transform.position + offset}");

        Camera.transform.position = transform.position + offset;
        Camera.transform.LookAt(transform.position);
    }

    void CheckStoryInFront()
    {
        if (Camera == null)
        {
            Debug.LogWarning("Camera bị null");
            return;
        }

        // Tạo ray trực tiếp từ vị trí camera theo hướng forward
        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        Debug.Log("Đã bắn ray từ: " + ray.origin + " hướng: " + ray.direction);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

        int layerMask = 1 << LayerMask.NameToLayer("Default");
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Story story = hit.collider.GetComponentInParent<Story>() ?? hit.collider.GetComponent<Story>();
            if (story != null)
            {
                Debug.Log("Chạm vào Story: " + (story.StoryData != null ? story.StoryData.name : "StoryData bị null"));
                CurrentStory = story;
            }
            else
            {
                Debug.Log("Chạm vào đối tượng khác: " + hit.collider.name + " tại: " + hit.point);
            }
        }
        else
        {
            Debug.Log("Raycast không chạm vào gì");
        }
    }
    #endregion

    void GenerateStories()
    {
        if (StoryManager == null || StoryPrefab == null)
        {
            Debug.LogWarning("StoryManager hoặc StoryPrefab bị null");
            return;
        }

        int count = StoryManager.Stories.Count;
        float angleStep = 360f / count;
        Vector3 dirToCamera = (Camera.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(dirToCamera.z, dirToCamera.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            float angle = baseAngle + i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 pos = transform.position + new Vector3(Mathf.Cos(rad) * radius, 0f, Mathf.Sin(rad) * radius);
            Debug.Log($"Story {i} tại vị trí: {pos}");

            GameObject storyObj = Instantiate(StoryPrefab.gameObject, pos, Quaternion.identity, transform);
            storyObj.transform.LookAt(transform.position);
            storyObj.transform.rotation *= Quaternion.Euler(90f, 0f, 0f);

            storyObj.GetComponent<Story>().SetupStory(StoryManager.Stories[i]);
            StartHoverAnimation(storyObj);
            storyObj.SetActive(false);

            StoryList.Add(storyObj.GetComponent<Story>());
        }
    }

    void StartShowStories(float showDelayTime)
    {
        if (C_ShowUpStories != null) StopCoroutine(C_ShowUpStories);
        C_ShowUpStories = StartCoroutine(ShowStories(showDelayTime));
    }

    IEnumerator ShowStories(float showDelayTime)
    {
        yield return new WaitForSeconds(delayShowUp);
        foreach (Story story in StoryList)
        {
            if (story != null)
            {
                story.gameObject.SetActive(true);
                story.StartShowUp(waitTimeShowUpStory);
                yield return new WaitForSeconds(showDelayTime);
            }
            else
            {
                Debug.LogWarning("Story trong StoryList bị null");
            }
        }
    }

    // Hàm thêm animation lơ lửng chỉ trên trục Y bằng DOTween
    private void StartHoverAnimation(GameObject target)
    {
        if (target == null) return;

        // Dừng animation cũ nếu có
        target.transform.DOKill();

        // Thiết lập chuyển động lơ lửng trên trục Y
        float hoverAmplitude = 0.5f; // Độ dịch chuyển trên trục Y (có thể điều chỉnh)
        float hoverDuration = 1.5f; // Thời gian hoàn thành một chu kỳ (có thể điều chỉnh)

        // Lưu vị trí ban đầu
        Vector3 originalPosition = target.transform.localPosition;

        // Tạo animation lên xuống trên trục Y
        target.transform.DOLocalMoveY(originalPosition.y + hoverAmplitude, hoverDuration / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // Lặp lại vô hạn và quay lại vị trí ban đầu
    }
}
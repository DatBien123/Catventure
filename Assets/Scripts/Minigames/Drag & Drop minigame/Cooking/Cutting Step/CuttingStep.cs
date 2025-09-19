using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// Đây sẽ là lớp xử lý UI, Logic của minigame (công đoạn) Cắt
// Ví dụ như là thái hành, thái thịt. Bản chất chúng cũng giống nhau là chỉ cắt nguyên liệu ra thành nhiều phần
public class CuttingStep : MonoBehaviour, ICookingStep
{
    [SerializeField] private CookingStepSO cookingStep; // thông tin dữ liệu cho công đoạn này
    [SerializeField]private CuttingStepUI stepUI; // phần UI của công đoạn cắt
    public CountDownTimerSystem countdown;

    private Sprite cuttedIngredient; // nguyên liệu đã thái lát
    private GameObject[] slices; // Các miếng con (đã ghép lại thành nguyên liệu ban đầu)
    private int maxCuts;
    [SerializeField]private int cutCount = 0;
    [SerializeField] private int currentIngredient = 0;
    public ArrowSwipeController swipeMinigame;
    public GameObject ingredientOnBoard;
    public GameObject ingredients;
    [SerializeField]private List<GameObject> cuttedPieces;
    public VictoryRewardScreen victoryRewardScreen;
    
    public void OnEnable()
    {
        ArrowSwipeController.onCuttedIngredient += RegisterCut;
        countdown.OnCountdownComplete += ShowSwipeMinigame;
    }
    public void OnDisable()
    {
        ArrowSwipeController.onCuttedIngredient -= RegisterCut;
        countdown.OnCountdownComplete -= ShowSwipeMinigame;

    }
    public void Start()
    {
    }
    public void Setup(CookingStepSO data)     // Setup là công đoạn ném dữ liệu cần thiết để cập nhật UI logic cho công đoạn này
    {
        cookingStep = data;
        gameObject.SetActive(true);
        StartStep();
    }
    public void StartStep()     // Công đoạn (minigame) sơ chế kiểu gì cũng cần hàm bắt đầu
    {
        SetupCuttingStep();
        stepUI.SetupUI(cookingStep);
        AudioManager.instance.PlayMusic("CuttingStepMusic");
        // Ban đầu bật hết các miếng lên (nguyên vẹn)
        foreach (var slice in slices)
        {
            slice.SetActive(true);
        }
        // Cho ingredient hiện ra từ trên bay xuống và pop lên
        AnimateIngredientEntry();
    }
    public void CompleteStep()     // Và sau khi đã chơi xong các công đoạn sơ chế thì sẽ kết thúc minigame này
    {
        // Hiện thông tin vật phẩm sau khi sơ chế ví dụ: Thịt bò thái lát
        Debug.Log("Hoàn thành thái các nguyên liệu");
        gameObject.SetActive(false);
        CookingManager.Instance.CompleteStep();
        
    }
    public void SetupCuttingStep()
    {
        currentIngredient = 0;
        IngredientCuttingData data = cookingStep.requiredIngredients[0].processing.cuttingData;
        slices = new GameObject[data.icons.Length];
        maxCuts = slices.Length - 1;

        // Instantiate các icons của nguyên liệu vào ingredients
        Sprite[] ingredientSprites = data.icons;
        
        for (int i = 0; i < ingredientSprites.Length; i++) {
            GameObject ingredient = new GameObject("Ingredient", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            ingredient.transform.SetParent(ingredients.gameObject.transform, false); // cutSpawnParent là Canvas hoặc group chứa
            Image image = ingredient.GetComponent<Image>();
            image.sprite  = ingredientSprites[i];   
            RectTransform rt = ingredient.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100,250);
            rt.pivot = new Vector2(0f, 0.5f);
            // Gán vào slices theo thứ tự ngược lại
            slices[data.icons.Length - 1 - i] = ingredient;
        }
        // gán cuttedIngredient bằng process Sprite
        cuttedIngredient = data.processSprite;
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) RegisterCut();

    }
    void RegisterCut()
    {
        AudioManager.instance.PlaySFX("KnifeSound");
        if (cutCount < slices.Length)
        {
            slices[cutCount].SetActive(false); // Ẩn miếng bị cắt
            cutCount++;
        // Đặt lại vị trí minigame swipe cho lần cắt tiếp theo
        if (cutCount < slices.Length)
        {
            Vector3 pos = slices[cutCount].transform.localPosition;
            swipeMinigame.gameObject.transform.localPosition = pos;
        }
            SpawnCuttedIngredient();
            if (cutCount == maxCuts)
            {
                Debug.Log("Nguyên liệu đã cắt xong");
                stepUI.kitchenKnife.GetComponent<KnifeUIDrag>().SetCanDrag(false);

                swipeMinigame.gameObject.SetActive(false);
                slices[slices.Length - 1].SetActive(false);
                GameObject cutImage = new GameObject("CuttedPiece", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                cutImage.transform.SetParent(ingredientOnBoard.gameObject.transform, false); // cutSpawnParent là Canvas hoặc group chứa

                // Gán sprite cho image
                Image img = cutImage.GetComponent<Image>();
                img.sprite = cuttedIngredient;
                RectTransform rt = cutImage.gameObject.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(200f, 200f); // width = 100, height = 80
                                                        // Vị trí: lệch x + random y
                Vector3 spawnPos = slices[slices.Length - 1].transform.localPosition;
                spawnPos.x += 100f;
                spawnPos.y += Random.Range(-50f, 50f);
                cutImage.GetComponent<RectTransform>().localPosition = spawnPos;
                cuttedPieces.Add(cutImage);
                StartCoroutine(ShowFinishedText());
            }
        }
        // Spawn phần nguyên liệu đã được thái ra 
        // Từ vị trí x hiện tại + 20-30f
        // Random từ vị trí y trong khoảng -50f đến 50f

    }
    void SpawnCuttedIngredient()
    {
        // 👉 Tạo Image lát cắt mới (không dùng prefab)
        GameObject cutImage = new GameObject("CuttedPiece", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        cutImage.transform.SetParent(ingredientOnBoard.gameObject.transform, false); // cutSpawnParent là Canvas hoặc group chứa

        // Gán sprite cho image
        Image img = cutImage.GetComponent<Image>();
        img.sprite = cuttedIngredient;
        RectTransform rt = cutImage.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200f, 200f); // width = 100, height = 80
                                                // Vị trí: lệch x + random y
        Vector3 spawnPos = slices[cutCount - 1].transform.localPosition;
        spawnPos.x += 50f;
        spawnPos.y += Random.Range(-50f, 50f);
        cutImage.GetComponent<RectTransform>().localPosition = spawnPos;
        cuttedPieces.Add(cutImage);
    }

    void AnimateIngredientEntry()
    {
        stepUI.ShowIngredientName(cookingStep.requiredIngredients[currentIngredient].ingredientName);
        RectTransform ingredientRT = ingredientOnBoard.GetComponent<RectTransform>();

        // Lưu vị trí mặc định
        Vector3 targetPos = ingredientRT.localPosition;

        // Đặt vị trí ban đầu ở phía trên ngoài màn
        Vector3 startPos = targetPos + new Vector3(0f, 600f, 0f); // bay từ trên xuống
        ingredientRT.localPosition = startPos;

        // Scale nhỏ lại trước khi pop-up
        ingredientRT.localScale = Vector3.one * 0.8f;

        // Chuỗi DOTween: bay xuống → pop-up scale
        ingredientRT.DOLocalMove(targetPos, 1f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                ingredientRT.DOScale(1.1f, 0.15f).SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        ingredientRT.DOScale(1f, 0.1f).SetEase(Ease.InQuad);
                        stepUI.HideIngredientName();
                    });
            });
        Invoke(nameof(StartCountDown321), 1f); // nếu countdown là kiểu CountdownController

    }
    public void StartCountDown321()
    {
        countdown.StartCountdown321();
    }
    public void ShowSwipeMinigame()
    {
        swipeMinigame.gameObject.SetActive(true);
        Vector3 pos = slices[0].transform.localPosition;
        swipeMinigame.gameObject.transform.localPosition = pos;
    }
    IEnumerator SetSwipePositionDelayed()
    {
        yield return new WaitForSeconds(1f); // Đợi 1 frame để đảm bảo layout UI cập nhật xong
    // spawn minigame swipe tại vết cắt đầu tiên
        swipeMinigame.gameObject.SetActive(true);
        Vector3 pos = slices[0].transform.localPosition;
        swipeMinigame.gameObject.transform.localPosition = pos;
    }
    public void GoToNextIngredient()
    {
        stepUI.kitchenKnife.GetComponent<KnifeUIDrag>().SetCanDrag(true);

        Debug.Log("Chuyển sang nguyên liệu tiếp theo");
        currentIngredient++;
        LoadNextIngredient();
        
    }
    IEnumerator ShowFinishedText()
    {
        stepUI.finishedText.GetComponent<FinishedTextUI>().ShowFinishedText();
        yield return new WaitForSeconds(3f);
        stepUI.finishedText.GetComponent<FinishedTextUI>().HideFinishedText();

        stepUI.ingredientBarUI.MarkItemAsCollected(cookingStep.requiredIngredients[currentIngredient]);
        if (currentIngredient + 1 == cookingStep.requiredIngredients.Length)
        {
            CompleteStep();
        }
        else
        {
            GoToNextIngredient();
        }
    }
    public void LoadNextIngredient()
    {
        cutCount = 0;
        foreach (GameObject obj in cuttedPieces)
        {
            Destroy(obj);   
        }
        cuttedPieces.Clear();
        IngredientCuttingData data = cookingStep.requiredIngredients[currentIngredient].processing.cuttingData;
        slices = new GameObject[data.icons.Length];
        maxCuts = slices.Length - 1;

        // Instantiate các icons của nguyên liệu vào ingredients
        Sprite[] ingredientSprites = data.icons;
        foreach(Transform child in ingredients.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            GameObject ingredient = new GameObject("Ingredient", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            ingredient.transform.SetParent(ingredients.gameObject.transform, false); // cutSpawnParent là Canvas hoặc group chứa
            Image image = ingredient.GetComponent<Image>();
            image.sprite = ingredientSprites[i];
            RectTransform rt = ingredient.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(100, 250);
            rt.pivot = new Vector2(0f, 0.5f);
            // Gán vào slices theo thứ tự ngược lại
            slices[data.icons.Length - 1 - i] = ingredient;
        }
        // gán cuttedIngredient bằng process Sprite
        cuttedIngredient = data.processSprite;
        AnimateIngredientEntry();
        StartCoroutine(SetSwipePositionDelayed());

    }









}

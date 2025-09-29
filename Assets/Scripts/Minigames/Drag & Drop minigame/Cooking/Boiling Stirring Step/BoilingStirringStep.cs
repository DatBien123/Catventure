using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoilingStirringStep : MonoBehaviour, ICookingStep
{
    public static BoilingStirringStep Instance;
    public CookingStepSO step; // dữ liệu của công đoạn này Nấu nước dùng và Khuấy. Thông tin như các nguyên liệu cần thiết, thời gian yêu cần, tên các thứ

    // Công đoạn Boiling (kéo thả nguyên liệu vào nồi)
    public Pot pot; // nồi xử lý minigame kéo thả
    public IngredientManager ingredientManager;
    public List<GameObject> ingredientPlates;
    public QTEBar quickTimeEventBar;     // QTE Bar 
    // Công đoạn Stirring (khuấy nguyên liệu trong nồi)
    public PotStirring potStirring; // nồi xử lý minigame khuấy

    public Canvas BoilingStirringUICanvas;
    public HandTutorial hand;
    public GameObject floatingTextPrefab;

    public DragDropCookingMinigame dragDropCookingMinigame;


    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {

    }
    public void Setup(CookingStepSO data)
    {
        step = data;
        gameObject.SetActive(true);
        dragDropCookingMinigame.GetCountDownSystem().StartCountdown321();
    }

    public void StartStep()
    {
        dragDropCookingMinigame.GetCountDownSystem().SetTimeStart(step.timeRequired);
        //BoilingStirringUI.Instance.userInterface.SetActive(true);
        //health.gameObject.SetActive(true);
        AudioManager.instance.PlayMusic("Cooking Background Music"); 
        Invoke(nameof(PlayInstructionLine), 1f);
        BoilingStirringUI.Instance.Setup(step);
        pot.Init(step);
        ingredientManager.InitCooking(step);
        StartHandTutorial();
    }

    public void CompleteStep()
    {
        //victoryRewardScreen.ShowRewardCookingStep(3);
        Debug.Log("Đã hoàn thành BoilingStirring Step");
        gameObject.SetActive(false);
        CookingManager.Instance.CompleteStep(); 
    }
    public void StartStirring()
    {
        StartCoroutine(ShowCircleArrowGuide());
    }
    //public void EndGame(bool success)
    //{
    //    base.EndGame(success);
    //    if (success)
    //    {
    //        Debug.Log("Player win");
    //        //boilingStirringUI.ShowNotificationDish(recipe.result.icon, recipe.result.name, recipe.result.resultDescription);
    //        Invoke(nameof(ShowTapToContinue), 3f);

    //    }
    //    else
    //    {
    //        Debug.Log("Player lose");
    //        // gọi Game Over
    //        GameOver();
    //    }
    //}
    public void HandleCookingDragAndDrop()
    {
        // Đây là hàm xử lý cái nồi nấu khi ta bắt đầu nấu ăn
        // nó sẽ rung lắc khói các thứ ấy
        //countDownTimer.StopCountDown();
    }
    public void PlayInstructionLine()
    {
        AudioManager.instance.PlaySFX("Instruction Cooking");
    }
    public void StartHandTutorial()
    {
        hand.gameObject.SetActive(true);   
        dragDropCookingMinigame.inputBlocker.SetActive(true); // ✅ Chặn tương tác
        hand.PlayHandCookingTutorial(() =>
        {
            dragDropCookingMinigame.inputBlocker.SetActive(false); // ✅ Mở khóa sau khi tay chỉ xong
            dragDropCookingMinigame.GetCountDownSystem().StartCountDown();
        });
    }
    // lấy ra đĩa chứa nguyên liệu đầu tiên của công thức món ăn
    public GameObject GetFirstIngredientInRecipe()
    {
        GameObject firstIngredient = null;
        IngredientSO ingredient = step.requiredIngredients[0];

        // duyệt qua các đĩa nguyên liệu đã tạo
        foreach (GameObject ingredientPlate in ingredientPlates)
        {
            if (ingredientPlate.GetComponentInChildren<Ingredient>().ingredientData == ingredient)
            {
                firstIngredient = ingredientPlate;
            }
        }
        return firstIngredient;
    }
    //public void ShowTapToContinue()
    //{
    //    base.ShowTapToContinue();
    //    tapToContinueButton.gameObject.SetActive(true);
    //    tapToContinueButton.onClick.RemoveAllListeners();
    //    tapToContinueButton.onClick.AddListener(() =>
    //    {
    //        //victoryRewardScreen.ShowRewardDragDrop(recipe.result.icon,recipe.result.name, recipe.reward, health.currentHealth);
    //        tapToContinueButton.gameObject.SetActive(false);
    //    });
    //}

    // Gọi ra Quick Time Event Bar để thêm màn chơi nấu nữa
    // Sau khi người chơi cho hết cả nguyên liệu cần thiết thì sẽ phải chơi tiếp Quick Time Event Bar để nấu món ăn
    public void ShowQTEBar()
    {
        DG.Tweening.Sequence seqItemCraftingHolder = DOTween.Sequence();
        seqItemCraftingHolder.Append(BoilingStirringUI.Instance.itemCraftingHolder.GetComponent<Image>().DOFade(0f, 0.5f))
            .AppendCallback(() =>
            {
                BoilingStirringUI.Instance.itemCraftingHolder.gameObject.SetActive(false);
        });
        DG.Tweening.Sequence seqIngredientBar = DOTween.Sequence();
        seqIngredientBar.Append(BoilingStirringUI.Instance.ingredientBar.GetComponent<Image>().DOFade(0f, 0.5f))
            .AppendCallback(() => 
            {
                BoilingStirringUI.Instance.ingredientBar.gameObject.SetActive(false);  
            });
        
        RectTransform rt = pot.gameObject.GetComponent<RectTransform>();
        rt.DOLocalMove(new Vector3(0f, rt.position.y, 0f), 1f).OnComplete(() => {
            quickTimeEventBar.SetupQTEBar();
        });
    }
    public void BoilingComplete()
    {
        Debug.Log("Nấu nước đã xong đến khuấy");
        pot.CookingComplete();
        BoilingStirringUI.Instance.userInterface.SetActive(true);
        BoilingStirringUI.Instance.itemCraftingHolder.SetActive(false);
        quickTimeEventBar.gameObject.SetActive(false);
        StartCoroutine(ShowFinishedText());
        Invoke(nameof(StartStirring),3f);

    }
    public void StirringComeplete()
    {
        // Khuấy xong là xong công đoạn này rồi
        Debug.Log("Khuấy đã xong");
        StartCoroutine(ShowFinishedText());
        Invoke(nameof(CompleteStep), 4f);
    }

    IEnumerator ShowFinishedText()
    {
        BoilingStirringUI.Instance.finishedText.GetComponent<FinishedTextUI>().ShowFinishedText();
        yield return new WaitForSeconds(3f);
        BoilingStirringUI.Instance.finishedText.GetComponent<FinishedTextUI>().HideFinishedText();
        Debug.Log("Hide finished Text");
        yield return new WaitForSeconds(1f);
    }



    private void OnEnable()
    {
        //UIEventSystem.Register("Start Game", StartGame);
        dragDropCookingMinigame.GetCountDownSystem().OnCountdownComplete += StartStep;
        quickTimeEventBar.onCookingCompleted += BoilingComplete;
        potStirring.onCookingCompleted += StirringComeplete;

    }
    private void OnDisable()
    {
        //UIEventSystem.Unregister("Start Game", StartGame);
        dragDropCookingMinigame.GetCountDownSystem().OnCountdownComplete -= StartStep;
        quickTimeEventBar.onCookingCompleted -= BoilingComplete;
        potStirring.onCookingCompleted -= StirringComeplete;

    }
    public void ShowCorrectEffect(string ingredientName) // Khi cho nguyên liệu/ nguyên tố đúng
    {
        // TODO: Hiện feedback visual + audio
        if (floatingTextPrefab != null)
        {
            //Debug.Log("Chạy floating text");
            GameObject objectText = Instantiate(floatingTextPrefab, BoilingStirringUICanvas.transform.position, Quaternion.identity, BoilingStirringUICanvas.transform);
            objectText.GetComponent<FloatingText>().SetText(ingredientName, Color.green);
            Destroy(objectText, 1.5f); // Hủy sau 2 giây
        }
    }

    public void ShowIncorrectEffect() // khi cho nguyên liệu sai
    {
        dragDropCookingMinigame.GetHealthSystem().DecreaseHealth(1);
        // TODO: Hiện feedback sai
        if (floatingTextPrefab != null)
        {
            //Debug.Log("Chạy floating text");
            GameObject objectText = Instantiate(floatingTextPrefab, BoilingStirringUICanvas.transform.position, Quaternion.identity, BoilingStirringUICanvas.transform);
            objectText.GetComponent<FloatingText>().SetText("WRONG", Color.red);
            Destroy(objectText, 1.5f); // Hủy sau 2 giây
        }
    }
    IEnumerator ShowCircleArrowGuide()
    {
        BoilingStirringUI.Instance.boiling.gameObject.SetActive(false);
        BoilingStirringUI.Instance.inputBlockerPanel.gameObject.SetActive(true);
        potStirring.Setup(step);
        GameObject circleArrow = BoilingStirringUI.Instance.circleArrowGuide;
        CanvasGroup canvasGroup = circleArrow.gameObject.GetComponent<CanvasGroup>();
        // Alpha nhấp nháy giữa 0 và 1 trong 3 giây
        canvasGroup.DOFade(0f, 0.5f)              // từ alpha 1 -> 0 trong 0.5s
            .SetLoops(-1, LoopType.Yoyo)          // lặp vô hạn, đi rồi về
            .SetEase(Ease.InOutSine)              // mượt mà
            .SetId("Blink");                      // đặt ID để quản lý dừng

        // Sau 3 giây thì dừng
        DOVirtual.DelayedCall(3f, () =>
        {
            DOTween.Kill("Blink");                // dừng tween nhấp nháy
            canvasGroup.alpha = 1f;
            circleArrow.gameObject.SetActive(false);
            BoilingStirringUI.Instance.inputBlockerPanel.gameObject.SetActive(false);

        });
        yield return new WaitForSeconds(2f);

    }
}

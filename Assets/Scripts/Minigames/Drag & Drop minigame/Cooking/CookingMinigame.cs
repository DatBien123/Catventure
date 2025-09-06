using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CookingMinigame : DragDropMinigame
{
    public static CookingMinigame Instance;
    public CookingUI cookingUI;
    public CraftingRecipeSO recipe;
    public Pot pot;
    public CraftingItemManager ingredientManager;
    public List<GameObject> ingredientPlates;

    // QTE Bar 
    public QTEBar quickTimeEventBar;
    // UI
    public GameObject startMinigamePanel;
    public GameObject userInterface;
    public GameObject itemCraftingHolder; // Container chứa các nguyên liệu trên màn hình
    public GameObject ingredientBar;

    private void Awake()
    {
        Instance = this;
    }
    new void Start()
    {
        startMinigamePanel.GetComponent<StartMinigamePanel>().Setup("NẤU ĂN", recipe.result.icon,recipe.result.name, recipe.craftingTime, recipe.reward);

    }
    private void OnEnable()
    {
        // Ta sẽ đăng ký sự kiện cho các hàm UIEventSystem ở đây
        UIEventSystem.Register("Start Game", StartGame);

    }
    private void OnDisable()
    {
        UIEventSystem.Unregister("Start Game", StartGame);

    }
    new void StartGame()
    {
        countDownTimer.SetTimeStart(recipe.craftingTime);
        startMinigamePanel.SetActive(false);
        userInterface.SetActive(true);
        health.gameObject.SetActive(true);
        AudioManager.instance.PlayMusic("Cooking Background Music");
        Invoke(nameof(PlayInstructionLine), 1f);
        cookingUI.Setup(recipe);
        pot.Init(recipe);
        ingredientManager.InitCooking(recipe);
        StartHandTutorial();
        pot.OnCookingComplete += EndGame;
        health.OnHealthZero += EndGame;
        countDownTimer.OnTimeUp += EndGame;
        quickTimeEventBar.onCookingCompleted += StartCooking;

    }
    new void EndGame(bool success)
    {
        base.EndGame(success);
        if (success)
        {
            Debug.Log("Player win");
            cookingUI.ShowNotificationDish(recipe.result.icon, recipe.result.name, recipe.result.resultDescription);
            Invoke(nameof(ShowTapToContinue), 3f);

        }
        else
        {
            Debug.Log("Player lose");
            // gọi Game Over
            GameOver();
        }
    }
    public void HandleCookingDragAndDrop()
    {
        // Đây là hàm xử lý cái nồi nấu khi ta bắt đầu nấu ăn
        // nó sẽ rung lắc khói các thứ ấy
        countDownTimer.StopCountDown();
    }
    public override void PlayInstructionLine()
    {
        AudioManager.instance.PlaySFX("Instruction Cooking");
    }
    public override void StartHandTutorial()
    {
        base.StartHandTutorial();
        inputBlocker.SetActive(true); // ✅ Chặn tương tác
        hand.PlayHandCookingTutorial(() =>
        {
            inputBlocker.SetActive(false); // ✅ Mở khóa sau khi tay chỉ xong
            countDownTimer.StartCountDown();
        });
    }
    // lấy ra đĩa chứa nguyên liệu đầu tiên của công thức món ăn
    public GameObject GetFirstIngredientInRecipe()
    {
        GameObject firstIngredient = null;
        CraftingItemSO ingredient = recipe.requiredItems[0];

        // duyệt qua các đĩa nguyên liệu đã tạo
        foreach (GameObject ingredientPlate in ingredientPlates)
        {
            if (ingredientPlate.GetComponentInChildren<CraftingItem>().crafttingItemData == ingredient)
            {
                firstIngredient = ingredientPlate;
            }
        }
        return firstIngredient;
    }
    new void OnDestroy()
    {
        pot.OnCookingComplete -= EndGame;
        base.OnDestroy();
    }
    new void ShowTapToContinue()
    {
        base.ShowTapToContinue();
        tapToContinueButton.gameObject.SetActive(true);
        tapToContinueButton.onClick.RemoveAllListeners();
        tapToContinueButton.onClick.AddListener(() =>
        {
            victoryRewardScreen.ShowRewardDragDrop(recipe.result.icon,recipe.result.name, recipe.reward, health.currentHealth);
            tapToContinueButton.gameObject.SetActive(false);
        });
    }

    // Gọi ra Quick Time Event Bar để thêm màn chơi nấu nữa
    // Sau khi người chơi cho hết cả nguyên liệu cần thiết thì sẽ phải chơi tiếp Quick Time Event Bar để nấu món ăn
    public void ShowQTEBar()
    {
        DG.Tweening.Sequence seqItemCraftingHolder = DOTween.Sequence();
        seqItemCraftingHolder.Append(itemCraftingHolder.GetComponent<Image>().DOFade(0f, 0.5f))
            .AppendCallback(() =>
            {
            itemCraftingHolder.gameObject.SetActive(false);
        });
        DG.Tweening.Sequence seqIngredientBar = DOTween.Sequence();
        seqIngredientBar.Append(ingredientBar.GetComponent<Image>().DOFade(0f, 0.5f))
            .AppendCallback(() => 
            {
                ingredientBar.gameObject.SetActive(false);  
            });
        
        RectTransform rt = pot.gameObject.GetComponent<RectTransform>();
        rt.DOLocalMove(new Vector3(0f,rt.position.y,0f), 1f);
        quickTimeEventBar.SetupQTEBar();
    }
    public void StartCooking()
    {
        pot.Cooking();
        userInterface.SetActive(true);
        itemCraftingHolder.SetActive(false);
        quickTimeEventBar.gameObject.SetActive(false);    }


}

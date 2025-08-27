using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ForgingSwordMinigame : DragDropMinigame
{
    public static ForgingSwordMinigame Instance;
    public ForgingSwordUI forgingSwordUI;
    public CraftingRecipeSO recipe;
    public Smithy smithy; // cái đe để rèn kiếm
    public CraftingItemManager elementalManager;
    public List<GameObject> elementals;
    private void Awake()
    {
        Instance = this;
    }
    new void Start()
    {
        StartGame();
    }
    new void StartGame()
    {
        AudioManager.instance.PlayMusic("Forging Sword Background Music");
        Invoke(nameof(PlayInstructionLine), 1f);
        forgingSwordUI.Setup(recipe);
        smithy.Init(recipe);
        elementalManager.InitForgingSword(recipe);
        StartHandTutorial();
        smithy.OnForgingComplete += EndGame;
        health.OnHealthZero += EndGame;
        countDownTimer.OnTimeUp += EndGame;

    }
    new void EndGame(bool success)
    {
        base.EndGame(success);
        if (success)
        {
            Debug.Log("Player win");
            forgingSwordUI.ShowNotificationDish(recipe.result.icon, recipe.result.name, recipe.result.resultDescription);
            Invoke(nameof(ShowTapToContinue), 3f);

        }
        else
        {
            Debug.Log("Player lose");
            // gọi Game Over
            GameOver();
        }
    }
    public void HandleForgingSword()
    {
        countDownTimer.StopCountDown();

    }
    public override void PlayInstructionLine()
    {
        AudioManager.instance.PlaySFX("Forging Sword Intruction Line");
    }
    public override void StartHandTutorial()
    {
        base.StartHandTutorial();
        inputBlocker.SetActive(true); // ✅ Chặn tương tác

        hand.PlayHandForgingSwordTutorial(() =>
        {
            inputBlocker.SetActive(false); // ✅ Mở khóa sau khi tay chỉ xong
            countDownTimer.StartCountDown();
        });
    }
    public GameObject GetFirstElementalInRecipe()
    {
        GameObject firstIngredient = null;
        CraftingItemSO elemental = recipe.requiredItems[0];

        // duyệt qua các đĩa nguyên liệu đã tạo
        foreach (GameObject ingredientPlate in elementals)
        {
            if (ingredientPlate.GetComponentInChildren<CraftingItem>().crafttingItemData == elemental)
            {
                firstIngredient = ingredientPlate;
            }
        }
        return firstIngredient;
    }
    new void OnDestroy()
    {
        smithy.OnForgingComplete -= EndGame;
        base.OnDestroy();
    }
}

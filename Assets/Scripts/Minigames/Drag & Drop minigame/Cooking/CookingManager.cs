using System;
using UnityEngine;

public class CookingManager : MonoBehaviour
{
    //Đây là lớp quản lý tổng thể quá trình nấu của 1 món ăn
    // Nó sẽ chứa công thức cần nấu món hiện tại
    // Và quản lý xong minigame (công đoạn nấu) nào thì chuyển tiếp hoặc thua các thứ
    public static CookingManager Instance;

    public CookingRecipeSO currentRecipe;
 [SerializeField]private int currentStepIndex; // bởi vì 1 món ăn sẽ có nhiều công đoạn lần lượt nên ta dùng biến này để đi lần lượt qua từng công đoạn

    public Action onFinishCooking;
    private void Awake()
    {
        Instance = this;
    }

        public void StartRecipe(CookingRecipeSO recipe)
    {
        currentRecipe = recipe;
        currentStepIndex = 0;
        LoadStep();
    }

    private void LoadStep()
    {
        if (currentStepIndex >= currentRecipe.steps.Length)
        {
            FinishRecipe();
            return;
        }

        CookingStepSO step = currentRecipe.steps[currentStepIndex];

        // Tùy stepType mà load minigame tương ứng
        switch (step.stepType)
        {
            case StepType.Cutting:
                CookingStepManager.Instance.StartCutting(step);
                break;
            case StepType.Assembling:
                CookingStepManager.Instance.StartAssembling(step);
                break;
            case StepType.BoilingStirring:
                CookingStepManager.Instance.StartBoilingStirring(step);
                break;
                // … thêm các minigame khác
        }

    }
    public void CompleteStep()
    {
        AudioManager.instance.StopAllSoundFX();
        AudioManager.instance.StopAllMusic();

        currentStepIndex++;
        LoadStep();
    }

    private void FinishRecipe()
    {
        Debug.Log("Hoàn thành món ăn: " + currentRecipe.recipeName);
        // Hiển thị kết quả, thưởng điểm, ảnh món ăn
        onFinishCooking?.Invoke();
    }
}

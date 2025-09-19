using UnityEngine;

public class CookingStepManager : MonoBehaviour
{
    // Đây sẽ là Instance chứa các công đoạn minigame nhỏ hơn trong minigame nấu ăn
    // Nơi sẽ xử lý khi đến công đoạn nào thì sẽ hiển thị UI, bắt đầu chơi công đoạn đó
    public static CookingStepManager Instance;
    public BoilingStirringStep boilingStirringStep;
    public CuttingStep cuttingStep;
    public AssemblingStep assemblingStep;
    private void Awake()
    {
        Instance = this;    
    }

    public void StartCutting(CookingStepSO step)
    {
        //Debug.Log("Chơi minigame Cutting với: " + step.requiredIngredients[0].itemName);
        // load UI minigame thái
        // Ta sẽ load các dữ liệu hình ảnh vào minigame tương ứng ở đây
        Debug.Log("Chạy công đoạn thái nguyên liệu");
        cuttingStep.Setup(step);
    }
    public void StartBoilingStirring(CookingStepSO step)
    {
        Debug.Log("Chạy công đoạn luộc và khuấy");
        boilingStirringStep.Setup(step);
    }
    public void StartAssembling(CookingStepSO step)
    {
        Debug.Log("Chạy công đoạn múc ra bát");
        assemblingStep.Setup(step); 
    }
}

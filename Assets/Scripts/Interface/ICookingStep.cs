using UnityEngine;

public interface ICookingStep
{
    public  void Setup(CookingStepSO data);
    public  void StartStep();
    public  void CompleteStep();
}

using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool isApplyTutorials = false;

    public SO_Tutorials tutorialDatabase;

    public TutorialPart currentPart;

    public int currentTutorialIndex = 0;

    public TutorialStep currentStep;

    public int currentStepIndex = 0;

    public bool AllowNextStep = false;

    private void Awake()
    {
        currentPart = tutorialDatabase.data.TutorialParts[currentTutorialIndex];
        currentStep = currentPart.TutorialSteps[currentStepIndex];

    }
    public void OnNextTutorialPart()
    {
        if(currentTutorialIndex < tutorialDatabase.data.TutorialParts.Count - 1)
        {
            currentTutorialIndex++;
            currentPart = tutorialDatabase.data.TutorialParts[currentTutorialIndex];
            currentStep = currentPart.TutorialSteps[0];
        }
    }
    public void OnNextTutorialStep()
    {
        if(currentStepIndex < currentPart.TutorialSteps.Count - 1)
        {
            currentStepIndex++;
            currentStep = currentPart.TutorialSteps[currentStepIndex];
        }
        else
        {
            OnNextTutorialPart();
        }

    }

    public void ApplyNextStep(string buttonName)
    {
        if(currentStep.stepName == buttonName)
        AllowNextStep = true;
    }

}

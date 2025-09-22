using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ETutorialType
{
    Tap,
    Drag
}
[System.Serializable]
public struct TutorialStep
{
    public string stepName;

    public ETutorialType InteractType;

    public IndicatorOffset TapOffset;

    public List<IndicatorOffset> DragOffsets;

    public bool isStepCompleted;

}
[System.Serializable]
public struct IndicatorOffset
{
    public Vector2 PositionOffset;
    public Vector2 RotationOffset;
    public Vector2 ScaleOffset;
}

[System.Serializable]
public struct TutorialPart
{
    public string id;
    public string TutorialName;
    public List<TutorialStep> TutorialSteps;
    public bool isPartCompleted;
}

[System.Serializable]
public struct TutorialData
{
    public List<TutorialPart> TutorialParts;
}
[CreateAssetMenu(fileName = "Tutorials Database", menuName = "Tutorials System/Tutorials Database")]
public class SO_Tutorials : ScriptableObject
{
    public TutorialData data;
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EBookFilterType
{
    Food,
    Vocabulary,
    Temp
}
public class UIBook : MonoBehaviour
{
    [Header("Filter Buttons")]
    public Button Food_Filter;
    public Button Vocabulary_Filter;
    public Button Temp_Filter;

    [Header("Current Filter")]
    public EBookFilterType currentFilterType;
    public Image[] LeftCollections;
    public Image[] RightCollections;

    [Header("Reference")]
    public BookManager BookManager;
    public Animator Animator;

    private void Awake()
    {
        Food_Filter.onClick.AddListener(() => FunctionTemp(EBookFilterType.Food));
        Vocabulary_Filter.onClick.AddListener(() => FunctionTemp(EBookFilterType.Vocabulary));
        Temp_Filter.onClick.AddListener(() => FunctionTemp(EBookFilterType.Temp));
    }
    public void RefreshBookUI()
    {
        List<SO_Card> FilterResult = new List<SO_Card>();
        switch (currentFilterType)
        {
            case EBookFilterType.Food:
                foreach(SO_FoodCard foodCard in BookManager.GetFoodCards())
                {

                }
                break;
            case EBookFilterType.Vocabulary:
                foreach (SO_Vocabulary vocabularyCard in BookManager.GetVocabularyCards())
                {

                }
                break;
            case EBookFilterType.Temp:
                foreach (SO_TempCard tempCard in BookManager.GetTempCards())
                {

                }
                break;
        }
    }
    public void FunctionTemp(EBookFilterType FilterType)
    {
        if (currentFilterType == FilterType) return;

        currentFilterType = FilterType;

        Animator.CrossFadeInFixedTime("Book_Open-Close", .1f);

        switch (FilterType)
        {
            case EBookFilterType.Food:
                Food_Filter.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1);
                Vocabulary_Filter.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
                Temp_Filter.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
            break;

            case EBookFilterType.Vocabulary:
                Food_Filter.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
                Vocabulary_Filter.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1);
                Temp_Filter.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
                break;

            case EBookFilterType.Temp:
                Food_Filter.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
                Vocabulary_Filter.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
                Temp_Filter.GetComponent<RectTransform>().localScale = new Vector3(1.5f, 1);
                break;
            default:

            break;
        }
    }
}

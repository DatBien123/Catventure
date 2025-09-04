using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Navigator Buttons")]
    public Button Prev_Button;
    public Button Next_Button;

    [Header("Current Filter")]
    public int CurrentCouplePage = 1;
    public int CurrentMaxCouplePage;
    public EBookFilterType CurrentFilterType;
    public Sprite UnlockIcon;
    public Image[] Collections;

    [Header("Reference")]
    public BookManager BookManager;
    public Animator Animator;

    private void Awake()
    {
        Food_Filter.onClick.AddListener(() => FunctionTemp(EBookFilterType.Food));
        Vocabulary_Filter.onClick.AddListener(() => FunctionTemp(EBookFilterType.Vocabulary));
        Temp_Filter.onClick.AddListener(() => FunctionTemp(EBookFilterType.Temp));

        Prev_Button.onClick.AddListener(() => OnPreviousPage());
        Next_Button.onClick.AddListener(() => OnNextPage());
    }
    private void Start()
    {
        FunctionTemp(EBookFilterType.Food);
    }
    #region [Filter]
    public void RefreshBookUI()
    {
        switch (CurrentFilterType)
        {
            case EBookFilterType.Food:
                int totalItems = BookManager.GetFoodCards().Count;
                Debug.Log("Total Food Card is" + totalItems.ToString());
                int startIndex = (CurrentCouplePage - 1) * 8;
                int endIndex = Mathf.Min(startIndex + 8, totalItems);

                int count = 0;
                for (int i = startIndex; i < endIndex; i++)
                {
                    Collections[count].sprite = BookManager.GetFoodCards()[i].Data.Icon;
                    count++;
                }

                if(count < 8)
                {
                    for (int i = count; i < 8; i++)
                    {
                        Collections[count].sprite = UnlockIcon;
                        count++;
                    }
                }
                break;
            case EBookFilterType.Vocabulary:

                 totalItems = BookManager.GetVocabularyCards().Count;
                Debug.Log("Total Food Card is" + totalItems.ToString());
                 startIndex = (CurrentCouplePage - 1) * 8;
                 endIndex = Mathf.Min(startIndex + 8, totalItems);

                 count = 0;
                for (int i = startIndex; i < endIndex; i++)
                {
                    Collections[count].sprite = BookManager.GetVocabularyCards()[i].Data.Icon;
                    count++;
                }

                if (count < 8)
                {
                    for (int i = count; i < 8; i++)
                    {
                        Collections[count].sprite = UnlockIcon;
                        count++;
                    }
                }

                break;
            case EBookFilterType.Temp:

                 totalItems = BookManager.GetTempCards().Count;
                Debug.Log("Total Food Card is" + totalItems.ToString());
                 startIndex = (CurrentCouplePage - 1) * 8;
                 endIndex = Mathf.Min(startIndex + 8, totalItems);

                 count = 0;
                for (int i = startIndex; i < endIndex; i++)
                {
                    Collections[count].sprite = BookManager.GetTempCards()[i].Data.Icon;
                    count++;
                }

                if (count < 8)
                {
                    for (int i = count; i < 8; i++)
                    {
                        Collections[count].sprite = UnlockIcon;
                        count++;
                    }
                }

                break;
        }
    }
    public void FunctionTemp(EBookFilterType FilterType)
    {
        if (CurrentFilterType == FilterType) return;

        CurrentFilterType = FilterType;
        CurrentCouplePage = 1; // reset về trang 1 mỗi khi đổi filter

        // Cập nhật CurrentMaxCouplePage theo filter
        int totalItems = 0;
        switch (FilterType)
        {
            case EBookFilterType.Food:
                totalItems = BookManager.GetFoodCards().Count;
                break;
            case EBookFilterType.Vocabulary:
                totalItems = BookManager.GetVocabularyCards().Count;
                break;
            case EBookFilterType.Temp:
                totalItems = BookManager.GetTempCards().Count;
                break;
        }
        CurrentMaxCouplePage = Mathf.CeilToInt((float)totalItems / 8f);
        if (CurrentMaxCouplePage == 0) CurrentMaxCouplePage = 1; // ít nhất phải có 1 trang

        Animator.CrossFadeInFixedTime("Book_Open-Close", 0.0f);

        // Update UI highlight filter
        Food_Filter.GetComponent<RectTransform>().localScale =
            FilterType == EBookFilterType.Food ? new Vector3(1.5f, 1) : new Vector3(1, 1);

        Vocabulary_Filter.GetComponent<RectTransform>().localScale =
            FilterType == EBookFilterType.Vocabulary ? new Vector3(1.5f, 1) : new Vector3(1, 1);

        Temp_Filter.GetComponent<RectTransform>().localScale =
            FilterType == EBookFilterType.Temp ? new Vector3(1.5f, 1) : new Vector3(1, 1);

        //RefreshBookUI();
    }
    #endregion

    #region [OnNextPage]
    public void OnNextPage()
    {
        if (CurrentCouplePage < CurrentMaxCouplePage)
        {
            CurrentCouplePage += 1;
            Animator.CrossFadeInFixedTime("Book_Flip_RTL", 0.0f);
            RefreshBookUI();
        }
    }
    public void OnPreviousPage()
    {
        if (CurrentCouplePage > 1)
        {
            CurrentCouplePage -= 1;
            Animator.CrossFadeInFixedTime("Book_Flip_LTR", 0.0f);
            RefreshBookUI();
        }
    }
    #endregion
}

using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum EBookFilterType
{
    None = 0,
    Food,
    Vocabulary,
    Temp
}
public struct GridLayoutData
{
    public int Left;
    public int Right;
    public int Top;
    public int Bottom;
    public Vector2 CellSize;
    public Vector2 Spacing;

    public GridLayoutData(int left, int right, int top, int bottom, Vector2 cellSize, Vector2 spacing)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
        CellSize = cellSize;
        Spacing = spacing;
    }
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
    public EBookFilterType CurrentFilterType = EBookFilterType.None;

    public Sprite UnlockIconTemp;
    public Sprite UnlockIconFood;
    public Sprite UnlockIconVolcabulary;

    public UIBookSlot[] Collections;

    [Header("Reference")]
    public UIBookSlotDetail BookSlotDetail;
    public BookManager BookManager;
    public Animator Animator;

    public GridLayoutGroup LeftGridLayoutGroup;
    public GridLayoutData CurrentLeftGridLayoutData;

    public GridLayoutGroup RightGridLayoutGroup;
    public GridLayoutData CurrentRightGridLayoutData;


    bool isFirstTimeOpen = false;

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
        RefreshBookUI();
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
                    Collections[count].SetupBookSlot(BookManager.GetFoodCards()[i]);
                    count++;
                }

                if(count < 8)
                {
                    for (int i = count; i < 8; i++)
                    {
                        Collections[count].Image.sprite = UnlockIconFood;
                        count++;
                    }
                }

                //Layout Data
                CurrentLeftGridLayoutData = new GridLayoutData(100, 0, 80, 0, new Vector2(170, 170), new Vector2(20, 50));
                LeftGridLayoutGroup.padding.left = CurrentLeftGridLayoutData.Left;
                LeftGridLayoutGroup.padding.right = CurrentLeftGridLayoutData.Right;
                LeftGridLayoutGroup.padding.top = CurrentLeftGridLayoutData.Top;
                LeftGridLayoutGroup.padding.bottom = CurrentLeftGridLayoutData.Bottom;
                LeftGridLayoutGroup.cellSize = CurrentLeftGridLayoutData.CellSize;
                LeftGridLayoutGroup.spacing = CurrentLeftGridLayoutData.Spacing;

                CurrentRightGridLayoutData = new GridLayoutData(30, 0, 80, 0, new Vector2(170, 170), new Vector2(20, 50));
                RightGridLayoutGroup.padding.left = CurrentRightGridLayoutData.Left;
                RightGridLayoutGroup.padding.right = CurrentRightGridLayoutData.Right;
                RightGridLayoutGroup.padding.top = CurrentRightGridLayoutData.Top;
                RightGridLayoutGroup.padding.bottom = CurrentRightGridLayoutData.Bottom;
                RightGridLayoutGroup.cellSize = CurrentRightGridLayoutData.CellSize;
                RightGridLayoutGroup.spacing = CurrentRightGridLayoutData.Spacing;

                break;
            case EBookFilterType.Vocabulary:

                 totalItems = BookManager.GetVocabularyCards().Count;
                Debug.Log("Total Food Card is" + totalItems.ToString());
                 startIndex = (CurrentCouplePage - 1) * 8;
                 endIndex = Mathf.Min(startIndex + 8, totalItems);

                 count = 0;
                for (int i = startIndex; i < endIndex; i++)
                {
                    Collections[count].SetupBookSlot(BookManager.GetVocabularyCards()[i]);
                    count++;
                }

                if (count < 8)
                {
                    for (int i = count; i < 8; i++)
                    {
                        Collections[count].Image.sprite = UnlockIconVolcabulary;
                        count++;
                    }
                }

                //Layout Data
                CurrentLeftGridLayoutData = new GridLayoutData(130, 0, 65, 0, new Vector2(144, 209), new Vector2(35, 11));
                LeftGridLayoutGroup.padding.left = CurrentLeftGridLayoutData.Left;
                LeftGridLayoutGroup.padding.right = CurrentLeftGridLayoutData.Right;
                LeftGridLayoutGroup.padding.top = CurrentLeftGridLayoutData.Top;
                LeftGridLayoutGroup.padding.bottom = CurrentLeftGridLayoutData.Bottom;
                LeftGridLayoutGroup.cellSize = CurrentLeftGridLayoutData.CellSize;
                LeftGridLayoutGroup.spacing = CurrentLeftGridLayoutData.Spacing;

                CurrentRightGridLayoutData = new GridLayoutData(40, 0, 65, 0, new Vector2(144, 209), new Vector2(35, 11));
                RightGridLayoutGroup.padding.left = CurrentRightGridLayoutData.Left;
                RightGridLayoutGroup.padding.right = CurrentRightGridLayoutData.Right;
                RightGridLayoutGroup.padding.top = CurrentRightGridLayoutData.Top;
                RightGridLayoutGroup.padding.bottom = CurrentRightGridLayoutData.Bottom;
                RightGridLayoutGroup.cellSize = CurrentRightGridLayoutData.CellSize;
                RightGridLayoutGroup.spacing = CurrentRightGridLayoutData.Spacing;

                break;
            case EBookFilterType.Temp:

                 totalItems = BookManager.GetTempCards().Count;
                Debug.Log("Total Food Card is" + totalItems.ToString());
                 startIndex = (CurrentCouplePage - 1) * 8;
                 endIndex = Mathf.Min(startIndex + 8, totalItems);

                 count = 0;
                for (int i = startIndex; i < endIndex; i++)
                {
                    Collections[count].SetupBookSlot(BookManager.GetTempCards()[i]);
                    count++;
                }

                if (count < 8)
                {
                    for (int i = count; i < 8; i++)
                    {
                        Collections[count].Image.sprite = UnlockIconTemp;
                        count++;
                    }
                }
                //Layout Data
                CurrentLeftGridLayoutData = new GridLayoutData(130, 0, 65, 0, new Vector2(144, 209), new Vector2(35, 11));
                LeftGridLayoutGroup.padding.left = CurrentLeftGridLayoutData.Left;
                LeftGridLayoutGroup.padding.right = CurrentLeftGridLayoutData.Right;
                LeftGridLayoutGroup.padding.top = CurrentLeftGridLayoutData.Top;
                LeftGridLayoutGroup.padding.bottom = CurrentLeftGridLayoutData.Bottom;
                LeftGridLayoutGroup.cellSize = CurrentLeftGridLayoutData.CellSize;
                LeftGridLayoutGroup.spacing = CurrentLeftGridLayoutData.Spacing;

                CurrentRightGridLayoutData = new GridLayoutData(40, 0, 65, 0, new Vector2(144, 209), new Vector2(35, 11));
                RightGridLayoutGroup.padding.left = CurrentRightGridLayoutData.Left;
                RightGridLayoutGroup.padding.right = CurrentRightGridLayoutData.Right;
                RightGridLayoutGroup.padding.top = CurrentRightGridLayoutData.Top;
                RightGridLayoutGroup.padding.bottom = CurrentRightGridLayoutData.Bottom;
                RightGridLayoutGroup.cellSize = CurrentRightGridLayoutData.CellSize;
                RightGridLayoutGroup.spacing = CurrentRightGridLayoutData.Spacing;

                break;
        }

        //Enable / Disable Prev - Next Button


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

        if(isFirstTimeOpen)
        Animator.CrossFadeInFixedTime(AnimationParams.BOOK_OPEN_AND_CLOSE, 0.0f);

        // Update UI highlight filter
        Food_Filter.GetComponent<RectTransform>().localScale =
            FilterType == EBookFilterType.Food ? new Vector3(1.5f, 1) : new Vector3(1, 1);

        Vocabulary_Filter.GetComponent<RectTransform>().localScale =
            FilterType == EBookFilterType.Vocabulary ? new Vector3(1.5f, 1) : new Vector3(1, 1);

        Temp_Filter.GetComponent<RectTransform>().localScale =
            FilterType == EBookFilterType.Temp ? new Vector3(1.5f, 1) : new Vector3(1, 1);

        isFirstTimeOpen = true;
    }
    #endregion

    #region [OnNextPage]
    public void OnNextPage()
    {
        if (CurrentCouplePage < CurrentMaxCouplePage)
        {
            CurrentCouplePage += 1;
            Animator.CrossFadeInFixedTime(AnimationParams.BOOK_FLIP_RTL, 0.0f);
            RefreshBookUI();
        }
    }
    public void OnPreviousPage()
    {
        if (CurrentCouplePage > 1)
        {
            CurrentCouplePage -= 1;
            Animator.CrossFadeInFixedTime(AnimationParams.BOOK_FLIP_LTR, 0.0f);
            RefreshBookUI();
        }
    }
    #endregion
}

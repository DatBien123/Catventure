using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IngredientManager : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public Transform ingredientsContainer;
    [SerializeField] private List<Transform> ingredientsSlotsPosition; // vị trí spawn các nguyên liệu trong vòng trên màn hình
    private void Awake()
    {

    }
    void Start()
    {

    }
    public void InitCooking(CookingStepSO step)
    {
        // Tạo bản sao của danh sách nguyên liệu để shuffle mà không làm ảnh hưởng đến dữ liệu gốc
        List<IngredientSO> shuffledIngredients = new List<IngredientSO>(step.requiredIngredients);

        // Shuffle danh sách bằng Fisher-Yates
        for (int i = shuffledIngredients.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            IngredientSO temp = shuffledIngredients[i];
            shuffledIngredients[i] = shuffledIngredients[j];
            shuffledIngredients[j] = temp;
        }

        for (int i = 0; i < shuffledIngredients.Count; i++)
        {
            IngredientSO ingredient = shuffledIngredients[i];
            GameObject ingredientObject = Instantiate(ingredientPrefab, ingredientsSlotsPosition[i].position, Quaternion.identity, ingredientsContainer);
            Image ingredientImage = ingredientObject.GetComponentInChildren<Image>();
            ingredientImage.sprite = ingredient.icon;
            ingredientObject.GetComponentInChildren<Ingredient>().ingredientData = ingredient;
            BoilingStirringStep.Instance.ingredientPlates.Add(ingredientObject);
        }
        
    }
    //public void InitForgingSword(IngredientRecipeSO recipe)
    //{
    //    // Tạo bản sao của danh sách nguyên liệu để shuffle mà không làm ảnh hưởng đến dữ liệu gốc
    //    List<IngredientSO> shuffledElementals = new List<IngredientSO>(recipe.requiredItems);

    //    // Shuffle danh sách bằng Fisher-Yates
    //    for (int i = shuffledElementals.Count - 1; i > 0; i--)
    //    {
    //        int j = Random.Range(0, i + 1);
    //        IngredientSO temp = shuffledElementals[i];
    //        shuffledElementals[i] = shuffledElementals[j];
    //        shuffledElementals[j] = temp;
    //    }

    //    for(int i = 0; i < shuffledElementals.Count; i++)
    //    {
    //        IngredientSO elemental = shuffledElementals[i];
    //        GameObject elementalObject = Instantiate(ingredientPrefab, ingredientsSlotsPosition[i].position, Quaternion.identity, ingredientsContainer);
    //        Image image = elementalObject.GetComponentInChildren<Image>();
    //        image.sprite = elemental.icon;
    //        elementalObject.GetComponentInChildren<Ingredient>().ingredientData = elemental;
    //        ForgingSwordMinigame.Instance.elementals.Add(elementalObject);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}

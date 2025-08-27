using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CraftingItemManager : MonoBehaviour
{
    public GameObject ingredientPrefab;
    public Transform ingredientContainer;
    [SerializeField] private List<Transform> ingredientsHolder;
    private void Awake()
    {

    }
    void Start()
    {

    }
    public void InitCooking(CraftingRecipeSO recipe)
    {
        // Tạo bản sao của danh sách nguyên liệu để shuffle mà không làm ảnh hưởng đến dữ liệu gốc
        List<CraftingItemSO> shuffledIngredients = new List<CraftingItemSO>(recipe.requiredItems);

        // Shuffle danh sách bằng Fisher-Yates
        for (int i = shuffledIngredients.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            CraftingItemSO temp = shuffledIngredients[i];
            shuffledIngredients[i] = shuffledIngredients[j];
            shuffledIngredients[j] = temp;
        }

        for (int i = 0; i < shuffledIngredients.Count; i++)
        {
            CraftingItemSO ingredient = shuffledIngredients[i];
            GameObject ingredientObject = Instantiate(ingredientPrefab, ingredientsHolder[i].position, Quaternion.identity, ingredientContainer);
            Image ingredientImage = ingredientObject.GetComponentInChildren<Image>();
            ingredientImage.sprite = ingredient.icon;
            ingredientObject.GetComponentInChildren<CraftingItem>().crafttingItemData = ingredient;
            CookingMinigame.Instance.ingredientPlates.Add(ingredientObject);
        }
        
    }
    public void InitForgingSword(CraftingRecipeSO recipe)
    {
        // Tạo bản sao của danh sách nguyên liệu để shuffle mà không làm ảnh hưởng đến dữ liệu gốc
        List<CraftingItemSO> shuffledElementals = new List<CraftingItemSO>(recipe.requiredItems);

        // Shuffle danh sách bằng Fisher-Yates
        for (int i = shuffledElementals.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            CraftingItemSO temp = shuffledElementals[i];
            shuffledElementals[i] = shuffledElementals[j];
            shuffledElementals[j] = temp;
        }

        for(int i = 0; i < shuffledElementals.Count; i++)
        {
            CraftingItemSO elemental = shuffledElementals[i];
            GameObject elementalObject = Instantiate(ingredientPrefab, ingredientsHolder[i].position, Quaternion.identity, ingredientContainer);
            Image image = elementalObject.GetComponentInChildren<Image>();
            image.sprite = elemental.icon;
            elementalObject.GetComponentInChildren<CraftingItem>().crafttingItemData = elemental;
            ForgingSwordMinigame.Instance.elementals.Add(elementalObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

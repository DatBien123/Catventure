using Microsoft.Win32.SafeHandles;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoodSelection : MonoBehaviour
{
    // Lớp này sẽ đảm nhận vai trò xử lý logic của Food Selection
    // Là sinh ra các món ăn có thể nấu khi để ta chọn món r nấu
    [SerializeField] public List<DishSO> allFoods; // Tất cả những món ăn có thể nấu sẽ là danh sách ở đây
    [SerializeField] private GameObject foodItemPrefab; // prefab để sinh ra item Food trong danh sách ấy
    [SerializeField] private List<FoodItemUI> foods;

    public DishSO currentFood; // Món hiện tại đang được chọn
    [SerializeField]private FoodItemUI currentSelected;
    [Header("User Interface")]
    public FoodSelectionUI foodSelectionUI;
    public Transform contentTransform; // Transform chứa các Item trong Scroll View
    [Header("Test")]
    public DragDropCookingMinigame dragDropCookingMinigame;
    public CharacterPlayer zera;

    [Header("Popup")]
    [SerializeField] private PopUpUI popupUI;

    [Header("References")]
    public TutorialManager tutorialManager;

    public void OnEnable()
    {
        UIEventSystem.Register("Cook",Cook);
        UIEventSystem.Register("Exit", Exit);

    }
    public void OnDisable()
    {
        UIEventSystem.Unregister("Cook", Cook);
        UIEventSystem.Unregister("Exit", Exit);

    }
    void Start()
    {
        // Tạo danh sách các món ăn có thể nấu ở đây 
        // Ta dùng vòng lặp foreach gì đó
        foreach (DishSO food in allFoods) {
            GameObject item = Instantiate(foodItemPrefab, contentTransform);
            FoodItemUI foodItemUI = item.GetComponent<FoodItemUI>();
            foodItemUI.Setup(food, OnFoodItemClicked, tutorialManager);
            foods.Add(foodItemUI);
        }
        // Auto chọn món đầu tiên khi mở
        if (foods.Count > 0) {
            foods[0].OnClick();
        }
    }
    // Khi player ấn nút Cook 
    public void Cook()
    {
        if(currentFood == null)
        {
            Debug.Log("Chưa chọn món ăn nào!");
            return;
        }
        bool enoughIngredients = true;
        // kiểm tra đủ nguyên liệu hay không ở đây

        foreach(var req in currentFood.ingredients)
        {
            // int owned = Inventory Tìm xem có item bằng truy vấn req.ingredient (SO_Consumable) này không? Có thì lấy ra số lượng của nó rồi trả vào owned
            int owned = zera.Inventory.GetTotalQuantity(req.ingredient);
            // kiểm tra số lượng có trong inventory có bằng hoặc lớn hơn số lượng yêu cuầ không
            if (owned < req.requiredAmount)
            {
                enoughIngredients = false;
                break;
            }
        }
        if (enoughIngredients)
        {
            popupUI.ShowConfirm(
                "Xác nhận nấu",
                "Bạn có muốn nấu món " + currentFood.dishName + " không?",
                yesCallback: () => {
                    // Thực hiện nấu
                    //foreach (var req in currentFood.ingredients)
                    //    zera.Inventory.RemoveItem(req.ingredient, req.requiredAmount);

                    dragDropCookingMinigame.Setup(currentFood.recipe);
                    foodSelectionUI.gameObject.SetActive(false);
                },
                noCallback: () => {
                    // Không nấu → quay lại UI chọn
                }
            );
        }
        else
        {
            popupUI.ShowNotify("THÔNG BÁO!",
                "KHÔNG ĐỦ NGUYÊN LIỆU NẤU " + currentFood.dishName,
                okCallback: () =>
                {

                });
        }
    }
    private void OnFoodItemClicked(DishSO dish, FoodItemUI itemUI)
    {
        // tắt món cũ
        if (currentSelected != null && currentSelected != itemUI)
        {
            currentSelected.SetSelected(false);
        }
        if (currentSelected == itemUI) return;
        // bật món mới
        currentSelected = itemUI;
        currentSelected.SetSelected(true);

        // cập nhật panel bên phải
        foodSelectionUI.ShowFoodInformation(dish);
    }
    public void Exit()
    {
        popupUI.ShowConfirm(
"MENU",
"Bạn muốn về Home Menu sao?",
yesCallback: () => {
    SceneManager.LoadScene("Home Scene");
},
noCallback: () => {
}
);
    }
}

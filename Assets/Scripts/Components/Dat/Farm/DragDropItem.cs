using FarmSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Camera")]
    public Camera mainCamera; // camera trong scene

    [Header("Filter Mask")]
    public LayerMask soilLayer;

    [Header("References")]
    public Ghost Ghost;
    public FarmManager FarmManager;

    bool isActionSucced;
    private void Awake()
    {
        // Tìm Camera
        mainCamera = FindObjectOfType<Camera>();

        // Tìm GameObject tên "Ghost"
        Ghost = FindObjectOfType<Ghost>(true); // true = includeInactive

        // Tìm component FarmManager
        FarmManager = FindObjectOfType<FarmManager>();
    }

    private void Start()
    {
        
    }
    #region [Use Cases]
    public void SetupGhost()
    {
        if(gameObject.name != "Scythe" && gameObject.name != "Can" && gameObject.name != "Pickaxe")
        {
            Ghost.gameObject.transform.localScale = 6*Vector3.one;
            SO_Tree loadedTree = Resources.Load<SO_Tree>($"Dat/Data/Tree/{gameObject.name}");
            Ghost.spriteRenderer.sprite = loadedTree.commonData.icon;
        }
        else
        {
            Ghost.gameObject.transform.localScale = Vector3.one;
            if(gameObject.name == "Scythe")Ghost.spriteRenderer.sprite = FarmManager.ScytheSprite;
            else if (gameObject.name == "Can") Ghost.spriteRenderer.sprite = FarmManager.CanSprite;
            else if (gameObject.name == "Pickaxe") Ghost.spriteRenderer.sprite = FarmManager.PickaxeSprite;
        }

    }
    #endregion

    #region [Drag Actions]
    public void OnBeginDrag(PointerEventData eventData)
    {
        // tạo ghost trong thế giới
        if (Ghost != null)
        {
            Ghost.gameObject.SetActive(true);
            Ghost.name = "Ghost - " + gameObject.name;

            SetupGhost();

            isActionSucced = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Ghost != null)
        {
            // Lấy vị trí chạm từ màn hình sang thế giới
            Vector3 screenPos = eventData.position;
            screenPos.z = Mathf.Abs(mainCamera.transform.position.z);
            // nếu cam ở -10 thì z = 10 để đưa ghost về mặt phẳng z=0

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0; // giữ ghost trên mặt phẳng 2D
            Ghost.transform.position = worldPos;

            // Kiểm tra va chạm Soil
            Collider2D hit = Physics2D.OverlapPoint(worldPos, soilLayer);
            if (hit != null)
            {
                Soil soil = hit.GetComponent<Soil>();
                if (soil != null)
                {
                    if (gameObject.name != "Scythe" && gameObject.name != "Can" && gameObject.name != "Pickaxe")
                        soil.PlantTree(gameObject.name);
                    else
                    {
                        if (gameObject.name == "Scythe")
                            soil.Harvest();
                        else if (gameObject.name == "Can")
                            soil.Watering();
                        else if (gameObject.name == "Pickaxe")
                            soil.Restoration();
                    }

                    isActionSucced = true;
                }
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Ghost != null)
        {
            Ghost.gameObject.SetActive(false);

            if (isActionSucced)
            {
                FarmManager.gameObject.SetActive(false);
            }
        }
    }
    #endregion
}

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
    public GameObject Ghost;
    public FarmManager FarmManager;

    bool isActionSucced;
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    #region [Use Cases]
    public void SetupGhost()
    {
        if(gameObject.name != "Scythe" && gameObject.name != "Can" && gameObject.name != "Pickaxe")
        {
            SO_Tree loadedTree = Resources.Load<SO_Tree>($"Dat/Data/Tree/{gameObject.name}");
            Ghost.GetComponent<SpriteRenderer>().sprite = loadedTree.commonData.icon;
        }
        else
        {
            if(gameObject.name == "Scythe")Ghost.GetComponent<SpriteRenderer>().sprite = FarmManager.ScytheSprite;
            else if (gameObject.name == "Can") Ghost.GetComponent<SpriteRenderer>().sprite = FarmManager.CanSprite;
            else if (gameObject.name == "Pickaxe") Ghost.GetComponent<SpriteRenderer>().sprite = FarmManager.PickaxeSprite;
        }

    }
    #endregion

    #region [Drag Actions]
    public void OnBeginDrag(PointerEventData eventData)
    {
        // tạo ghost trong thế giới
        if (Ghost != null)
        {
            Ghost.SetActive(true);
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
            Ghost.SetActive(false);

            if (isActionSucced)
            {
                FarmManager.gameObject.SetActive(false);
            }
        }
    }
    #endregion
}

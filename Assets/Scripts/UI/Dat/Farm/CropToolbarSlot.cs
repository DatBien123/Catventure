using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CropToolbarSlot : MonoBehaviour, IObjectPool<CropToolbarSlot>
{
    [Header("UI Informations")]
    public Image Image;
    public TextMeshProUGUI Quantity;
    public DragDropItem DragDropItem;

    public CropsInstance CropHolding;

    #region [Pool]
    public int poolID { get; set; }
    public ObjectPooler<CropToolbarSlot> pool { get; set; }
    #endregion

    public void SetupCropToolbarSlot(CropsInstance consumableInstance)
    {
        CropHolding = consumableInstance;

        Image.sprite = consumableInstance.ItemStaticData.commonData.icon;
        Quantity.text = consumableInstance.Quantity.ToString();

    }
}

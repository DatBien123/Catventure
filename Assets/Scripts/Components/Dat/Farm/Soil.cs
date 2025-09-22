
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FarmSystem
{
    [Flags]
    public enum ESoilState
    {
        None = 0,
        //Main
        HasPlant = 1 << 0,
        CanHarvest = 1 << 1,
        //Others
        Dry = 1 << 2,
        Wet = 1 << 3,
        //
        HavestedOrDigged = 1 << 4,
        //....
    }
    public class Soil : MonoBehaviour, IPointerClickHandler
    {

        [Header("Components")]
        public SpriteRenderer spriteRenderer;

        public SpriteRenderer clodSpriteRenderer;

        public SpriteRenderer treeHavestSpriteRenderer;

        public SpriteRenderer plantTreeSpriteRenderer;

        public SpriteRenderer removeTreeSpriteRenderer;

        public SpriteRenderer wateringTreeSpriteRenderer;

        public SpriteRenderer canHavestBorderSpriteRenderer;

        public SpriteRenderer canHavestIndicatorSpriteRenderer;



        [Header("Soil State")]
        public Sprite DrySprite;
        public Sprite WetSprite;
        public Sprite HasPlantSprite;
        public Sprite HavestedOrRemovePlantSprite;

        public ESoilState SoilState;

        [Header("Current Tree")]
        public Crops CurrentTree;

        [Header("Reference")]
        public Animator Animator;
        public FarmManager FarmManager;
        public DragDropItem DragDropItem;

        [Header("Events")]
        public UnityEvent OnHavestEvent;
        public UnityEvent OnPlantEvent;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
            DragDropItem = GetComponent<DragDropItem>();
        }
        public void Start()
        {
            //AddState(ESoilState.Dry);
        }

        #region [State Manager]
        public void AddState(ESoilState state)
        {
            SoilState |= state;
        }
        public void RemoveState(ESoilState state)
        {
            SoilState &= ~state;
        }
        public bool HasState(ESoilState state)
        {
            return (SoilState & state) != 0;
        }
        #endregion

        #region [OnClickEvent]
        public void OnPointerClick(PointerEventData eventData)
        {
            FarmManager.CurrentSoilSelected = this;
            Debug.Log("Current Soil is: " + FarmManager.CurrentSoilSelected.gameObject.name);

            FarmManager.gameObject.SetActive(true);

            if (HasState(ESoilState.HasPlant)) {
                // CÓ CÂY ------------------------- CHƯA THỂ THU HOẠCH
                if (!HasState(ESoilState.CanHarvest))
                // => Hiển thị ------------- Huỷ Cây
                {
                    FarmManager.RemoveToolbar.SetActive(true);
                    FarmManager.HavestToolbar.SetActive(false);
                    FarmManager.CropsToolbar.gameObject.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(false);
                }

                // CÓ CÂY ------------------------- CÓ THỂ THU HOẠCH 
                else
                // => Hiển thị ------------ Thu Hoạch
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(true);
                    FarmManager.CropsToolbar.gameObject.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(false);

                }
            }
            // KHÔNG CÓ CÂY
            else
            {
                // ĐẤT KHÔNG BỊ BỚI TUNG --- VÀ --- KHÔNG BỊ KHÔ
                if (HasState(ESoilState.HavestedOrDigged))
                // => Hiển thị ------------ Hồi Phục - Canh Tác Lại Đất
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(false);
                    FarmManager.CropsToolbar.gameObject.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(true);
                }
                // ĐẤT CHỈ BỊ KHÔ
                else if (HasState(ESoilState.Dry))
                // => Hiển thị ------------ Tưới Nước
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(false);
                    FarmManager.CropsToolbar.gameObject.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(true);
                    FarmManager.RestorationToolbar.SetActive(false);
                }

                // ĐẤT BỊ BỚI TUNG
                else if (HasState(ESoilState.Wet))
                // => Hiển thị ------------ Hồi Phục - Canh Tác Lại Đất
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(false);
                    FarmManager.CropsToolbar.gameObject.SetActive(true);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(false);
                }


            }

        }

        #endregion

        #region [Actions]
        public bool PlantTree(string treeName)
        {
            if ((SoilState & ESoilState.HasPlant) == 0 && (SoilState & ESoilState.Wet) != 0)
            {
                SO_Tree loadedTreeData = Resources.Load<SO_Tree>($"Dat/Data/Tree/{treeName}");
                if (!FarmManager.CharacterPlayer.Inventory.CheckItemExist(loadedTreeData))
                {
                    return false;
                }

                CurrentTree = Instantiate(loadedTreeData.data.TreeWorldInstance, transform).GetComponent<Crops>();
                CurrentTree.transform.localPosition = loadedTreeData.data.stageDatas[0].positionOffset;
                AddState(ESoilState.HasPlant);

                spriteRenderer.sprite = HasPlantSprite;

                plantTreeSpriteRenderer.sprite = loadedTreeData.commonData.icon;
                //plantTreeSpriteRenderer.gameObject.transform.localScale = new Vector3(-.03f, .03f, .03f);
                Animator.CrossFadeInFixedTime("Soil Plant", 0.0f);

                //Data
                FarmManager.CharacterPlayer.Inventory.RemoveItem(loadedTreeData, 1);
                Debug.Log( "Total " + loadedTreeData.name + " " + FarmManager.CharacterPlayer.Inventory.GetTotalQuantity(loadedTreeData));
                FarmManager.CropsToolbar.RefreshCropsToolbar();

                // Lưu trạng thái nông trại
                FarmSaveSystem.Save(FarmManager);

                // Lưu dữ liệu nhân vật (do thay đổi inventory)
                SaveSystem.Save(FarmManager.CharacterPlayer, FarmManager.CharacterPlayer.Inventory);

                return FarmManager.CharacterPlayer.Inventory.GetTotalQuantity(loadedTreeData) > 0;
            }

            return true;
        }
        public void Restoration()
        {
            if ((SoilState & ESoilState.HasPlant) == 0)
            {
                RemoveState(ESoilState.HavestedOrDigged);

                AddState(ESoilState.Dry);

                spriteRenderer.sprite = DrySprite;
                Animator.CrossFadeInFixedTime("Soil Remove", 0.0f);
                clodSpriteRenderer.sprite = null;

                // Lưu trạng thái nông trại
                FarmSaveSystem.Save(FarmManager);
            }
        }

        public void Watering()
        {
            if ((SoilState & ESoilState.Dry) != 0)
            {
                RemoveState(ESoilState.Dry);
                AddState(ESoilState.Wet);

                spriteRenderer.sprite = WetSprite;
                Animator.CrossFadeInFixedTime("Soil Watering", 0.0f);
                if (clodSpriteRenderer.sprite != null)
                {
                    clodSpriteRenderer.sprite = CurrentTree.TreeCurrentStage.clodData.clodWetImage;
                }

                // Lưu trạng thái nông trại
                FarmSaveSystem.Save(FarmManager);
            }


        }
        public void Harvest()
        {
            if (((SoilState & ESoilState.HasPlant) != 0)
                && CurrentTree.TreeCurrentStage.isFinalStage)
            {
                treeHavestSpriteRenderer.sprite = CurrentTree.TreeCurrentStage.stageImage;
                canHavestIndicatorSpriteRenderer.gameObject.SetActive(false);
                canHavestBorderSpriteRenderer.gameObject.SetActive(false);
                Animator.CrossFadeInFixedTime("Soil Havest", 0.0f);

                FarmManager.CharacterPlayer.Inventory.AddItem(new CropsInstance(CurrentTree.TreeDataOrigin.data.rewardData.consumReward, CurrentTree.TreeDataOrigin.data.rewardData.Harvests));


                DestroyCurrentTree();

                //Data

                // Lưu dữ liệu nhân vật (do thay đổi inventory)
                SaveSystem.Save(FarmManager.CharacterPlayer, FarmManager.CharacterPlayer.Inventory);
                // Lưu trạng thái nông trại
                FarmSaveSystem.Save(FarmManager);
            }
        }
        public void DestroyCurrentTree()
        {
            if ((SoilState & ESoilState.HasPlant) != 0)
            {
                GameObject treeGO = CurrentTree.gameObject;
                Destroy(treeGO);
                CurrentTree = null;

                RemoveState(ESoilState.HasPlant);
                RemoveState(ESoilState.CanHarvest);
                RemoveState(ESoilState.Wet);

                AddState(ESoilState.Dry);
                //AddState(ESoilState.HavestedOrDigged);

                spriteRenderer.sprite = HavestedOrRemovePlantSprite;
                clodSpriteRenderer.sprite = null;

                // Lưu trạng thái nông trại
                FarmSaveSystem.Save(FarmManager);
            }
        }
        #endregion
    }
}

using System;

using UnityEngine;
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

        [Header("Soil State")]
        public Sprite DrySprite;
        public Sprite WetSprite;
        public Sprite HasPlantSprite;
        public Sprite HavestedOrRemovePlantSprite;

        public ESoilState SoilState;

        [Header("Current Tree")]
        public Tree CurrentTree;

        [Header("Reference")]
        public FarmManager FarmManager;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void Start()
        {
            AddState(ESoilState.Dry);
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
                    FarmManager.CropsToolbar.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(false);
                }

                // CÓ CÂY ------------------------- CÓ THỂ THU HOẠCH 
                else
                // => Hiển thị ------------ Thu Hoạch
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(true);
                    FarmManager.CropsToolbar.SetActive(false);
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
                    FarmManager.CropsToolbar.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(true);
                }
                // ĐẤT CHỈ BỊ KHÔ
                else if (HasState(ESoilState.Dry))
                // => Hiển thị ------------ Tưới Nước
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(false);
                    FarmManager.CropsToolbar.SetActive(false);
                    FarmManager.WateringToolbar.SetActive(true);
                    FarmManager.RestorationToolbar.SetActive(false);
                }

                // ĐẤT BỊ BỚI TUNG
                else if (HasState(ESoilState.Wet))
                // => Hiển thị ------------ Hồi Phục - Canh Tác Lại Đất
                {
                    FarmManager.RemoveToolbar.SetActive(false);
                    FarmManager.HavestToolbar.SetActive(false);
                    FarmManager.CropsToolbar.SetActive(true);
                    FarmManager.WateringToolbar.SetActive(false);
                    FarmManager.RestorationToolbar.SetActive(false);
                }


            }

        }

        #endregion

        #region [Actions]
        public void PlantTree(string treeName)
        {
            if ((SoilState & ESoilState.HasPlant) == 0)
            {
                SO_Tree loadedTreeData = Resources.Load<SO_Tree>($"Dat/Data/Tree/{treeName}");

                CurrentTree = Instantiate(loadedTreeData.data.TreeWorldInstance, transform).GetComponent<Tree>();

                AddState(ESoilState.HasPlant);

                spriteRenderer.sprite = HasPlantSprite;
            }
        }
        public void Restoration()
        {
            RemoveState(ESoilState.HavestedOrDigged);

            AddState(ESoilState.Dry);

            spriteRenderer.sprite = DrySprite;
        }

        public void Watering()
        {
            RemoveState(ESoilState.Dry);
            AddState(ESoilState.Wet);

            spriteRenderer.sprite = WetSprite;
        }
        public void Harvest()
        {
            if (((SoilState & ESoilState.HasPlant) != 0)
                && CurrentTree.TreeCurrentStage.isFinalStage)
            {

                DestroyCurrentTree();


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
                AddState(ESoilState.HavestedOrDigged);

                spriteRenderer.sprite = HavestedOrRemovePlantSprite;
            }
        }
        #endregion
    }
}
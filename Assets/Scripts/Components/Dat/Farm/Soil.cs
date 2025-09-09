using FarmSystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.GridLayoutGroup;

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
        //....
    }
    public class Soil : MonoBehaviour, IPointerClickHandler
    {

        [Header("Components")]
        public SpriteRenderer spriteRenderer;

        [Header("Current Soil Stage")]
        public ESoilState SoilState;

        [Header("Current Tree")]
        public Tree CurrentTree;

        [Header("Reference")]
        public FarmManager FarmManager;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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

            if (HasState(ESoilState.HasPlant) && !HasState(ESoilState.CanHarvest))
            {
                FarmManager.RemoveToolbar.SetActive(true);
                FarmManager.HavestToolbar.SetActive(false);
                FarmManager.CropsToolbar.SetActive(false);
            }
            else if(HasState(ESoilState.HasPlant) && HasState(ESoilState.CanHarvest))
            {
                FarmManager.RemoveToolbar.SetActive(false);
                FarmManager.HavestToolbar.SetActive(true);
                FarmManager.CropsToolbar.SetActive(false);
            }
            else if (!HasState(ESoilState.HasPlant))
            {
                FarmManager.RemoveToolbar.SetActive(false);
                FarmManager.HavestToolbar.SetActive(false);
                FarmManager.CropsToolbar.SetActive(true);
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

                ShowFloatingText("Trồng cây");
            }
        }
        public void Harvest()
        {
            if (((SoilState & ESoilState.HasPlant) != 0)
                && CurrentTree.TreeCurrentStage.isFinalStage)
            {

                DestroyCurrentTree();
                RemoveState(ESoilState.CanHarvest);

                ShowFloatingText("Thu hoạch");
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
            }
        }
        #endregion

        #region [Debug]
        [SerializeField] private string debugText;
        void OnDrawGizmos()
        {
            if (!string.IsNullOrEmpty(debugText))
            {
                UnityEditor.Handles.Label(transform.position + Vector3.up * 1.5f, debugText);
            }
        }

        [Header("Floating Text")]
        public GameObject floatingTextPrefab; // assign prefab từ inspector

        private void ShowFloatingText(string message)
        {
            if (floatingTextPrefab != null)
            {
                GameObject textObj = Instantiate(floatingTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
                textObj.GetComponent<TextMesh>().text = message;

                Destroy(textObj, 2f); // auto xóa sau 2 giây
            }
        }

        #endregion
    }
}
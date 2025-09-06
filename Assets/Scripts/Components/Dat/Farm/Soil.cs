using FarmSystem;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

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
    public class Soil : MonoBehaviour
    {
        [Header("Components")]
        public SpriteRenderer spriteRenderer;

        [Header("Current Soil Stage")]
        public ESoilState SoilState;

        [Header("Current Tree")]
        public Tree CurrentTree;

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

        #region [Actions]
        public void PlantTree(Character planter, string treeName)
        {
            if ((SoilState & ESoilState.HasPlant) == 0)
            {
                
            }
        }
        public void Watering()
        {

        }
        public void Harvest(Character harvester)
        {
            if (((SoilState & ESoilState.HasPlant) != 0)
                && CurrentTree.TreeCurrentStage.isFinalStage)
            {
                
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


    }
}
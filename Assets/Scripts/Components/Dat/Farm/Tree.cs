using System.Collections;
using UnityEngine;

namespace FarmSystem
{

    public class Tree : MonoBehaviour
    {
        [Header("Components")]
        public SpriteRenderer spriteRenderer;

        [Header("Tree Datas")]
        public SO_Tree TreeDataOrigin;
        public SO_Tree TreeDataTemporary;

        [Header("Stage Manager")]
        [Header("Tree Current Stage")]
        public int CurrentStageIndex = 0;
        public TreeStageData TreeCurrentStage;

        Coroutine C_Growing;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            TreeDataTemporary = Instantiate(TreeDataOrigin);
            TreeCurrentStage = TreeDataTemporary.data.stageDatas[CurrentStageIndex];
            StartGrowing();
        }
        public void TransitionToNextStage()
        {
            if (!TreeCurrentStage.isFinalStage)
            {
                CurrentStageIndex++;
                TreeCurrentStage = TreeDataTemporary.data.stageDatas[CurrentStageIndex];
                spriteRenderer.sprite = TreeCurrentStage.stageImage;

                transform.localPosition = TreeCurrentStage.positionOffset;

                if (TreeCurrentStage.clodData.clodWetImage && TreeCurrentStage.clodData.clodDryImage)
                {
                    if (transform.GetComponentInParent<Soil>().HasState(ESoilState.Dry))
                    {
                        transform.GetComponentInParent<Soil>().clodSpriteRenderer.sprite = TreeCurrentStage.clodData.clodDryImage;
                    }
                    else transform.GetComponentInParent<Soil>().clodSpriteRenderer.sprite = TreeCurrentStage.clodData.clodWetImage;
                }
                else
                {
                    transform.GetComponentInParent<Soil>().clodSpriteRenderer.sprite = null;
                }

                if (TreeCurrentStage.isFinalStage)
                {
                    transform.GetComponentInParent<Soil>().AddState(ESoilState.CanHarvest);
                }
            }
        }

        public void StartGrowing()
        {
            if (C_Growing != null)
            {
                StopCoroutine(C_Growing);
            }
            StartCoroutine(Growing());
        }

        IEnumerator Growing()
        {
            while (CurrentStageIndex < TreeDataTemporary.data.stageDatas.Count)
            {
                yield return new WaitForSeconds(TreeCurrentStage.transitionTime);
                TransitionToNextStage();
            }
        }

    }
}
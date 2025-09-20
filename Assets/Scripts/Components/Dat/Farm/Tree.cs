using System.Collections;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting; // nhớ import DOTween

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
        private Tween idleTween;
        public float idleTweenFactor = 1.3f;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            TreeDataTemporary = Instantiate(TreeDataOrigin);
            TreeCurrentStage = TreeDataTemporary.data.stageDatas[CurrentStageIndex];
            StartGrowing();

            StartIdleAnimation();
        }

        private void OnDisable()
        {
            if (idleTween != null) idleTween.Kill();
        }

        void StartIdleAnimation()
        {
            // giữ scale gốc
            Vector3 baseScale = transform.localScale;

            // tween scaleY lên một chút rồi xuống, lặp vô hạn
            idleTween = transform.DOScaleY(baseScale.y * idleTweenFactor, 1f)
                .SetLoops(-1, LoopType.Yoyo) // -1 = vô hạn, Yoyo = lên xuống
                .SetEase(Ease.InOutSine);
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

                    transform.GetComponentInParent<Soil>().canHavestIndicatorSpriteRenderer.sprite = TreeDataOrigin.data.rewardData.consumReward.commonData.icon;
                    transform.GetComponentInParent<Soil>().canHavestIndicatorSpriteRenderer.gameObject.SetActive(true);

                    transform.GetComponentInParent<Soil>().Animator.CrossFadeInFixedTime("Soil Can Havest Indicate", .0f);
                }
            }
        }

        public void StartGrowing()
        {
            if (C_Growing != null)
            {
                StopCoroutine(C_Growing);
            }
            C_Growing = StartCoroutine(Growing());
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

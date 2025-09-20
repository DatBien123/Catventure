using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System;

public class PotStirring : MonoBehaviour
{
    [SerializeField] private CookingStepSO currentStep;  
    [SerializeField] private RectTransform pot;              // cái nồi
    [SerializeField] private RectTransform ladle;            // cái muôi
    [SerializeField] private Transform ingredientsInPot;     // để xoay
    [SerializeField] private CookingProgressBar cookingProgress;
    [SerializeField] private Color rawColor = Color.white;   // màu nguyên liệu sống
    [SerializeField] private Color cookedColor = new Color(0.5f, 0.25f, 0.1f); // màu nâu (chín)
    public ParticleSystem bubbleWaterVFX;
    public List<Image> ingredients;

    private Vector2 prevDir;
    private float accumulatedAngle = 0f;
    public Action onCookingCompleted;

    public void Setup(CookingStepSO step)
    {
        BoilingStirringUI.Instance.stirring.gameObject.SetActive(true);    
        for (int i = 0; i < step.requiredIngredients.Length; i++)
        {
            Vector2 randomPosition = Vector2.zero;
            randomPosition.x = UnityEngine.Random.Range(-100f, 100f);
            randomPosition.y = UnityEngine.Random.Range(-100f, 100f);
            GameObject obj = new GameObject("Ingredient", typeof(Image));
            ingredients.Add(obj.GetComponent<Image>());
            obj.transform.SetParent(ingredientsInPot,false);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = randomPosition;
            rt.localScale = Vector3.one;
            rt.sizeDelta = new Vector2(150f,150f);
            Image image = obj.GetComponent<Image>();
            image.sprite = step.requiredIngredients[i].icon;
        }
        AudioManager.instance.PlaySFX("Boiling Sound");
        bubbleWaterVFX.Play();
    }

    void Start()
    {
        cookingProgress.slider.onValueChanged.AddListener(OnCookingProgressChanged);
        // Lưu hướng ban đầu (từ nồi đến muôi)
        prevDir = (ladle.position - pot.position).normalized;

    }

    // Update is called once per frame
    void Update()
    {
        // Vector hướng từ nồi -> muôi
        Vector2 currDir = (ladle.position - pot.position).normalized;
        float deltaAngle = Vector2.SignedAngle(prevDir, currDir);

        // Check muôi có trong phạm vi nồi không
        if (RectTransformUtility.RectangleContainsScreenPoint(pot, ladle.position))
        {
            // chỉ tính ngược chiều kim đồng hồ
            if (deltaAngle > 0f)
            {
                accumulatedAngle += deltaAngle;

                // Nếu đủ 1 vòng (360 độ)
                if (accumulatedAngle >= 360f)
                {
                    accumulatedAngle = 0f;

                    // Ingredient xoay thêm 50 độ mượt mà
                    ingredientsInPot
                        .DORotate(
                            new Vector3(0, 0, ingredientsInPot.localEulerAngles.z + 180f),
                            2f,                // thời gian tween
                            RotateMode.FastBeyond360
                        );

                    // Tăng progress bar
                    cookingProgress.AddCookingProgress(20f,0.1f);

                }
            }
        }

        // cập nhật hướng trước
        prevDir = currDir;
    }
    private void OnCookingProgressChanged(float value)
    {
        float t = value / cookingProgress.slider.maxValue;

        foreach (var ingredient in ingredients)
        {
            if (ingredient != null)
                ingredient.color = Color.Lerp(rawColor, cookedColor, t);
        }

        if (value >= cookingProgress.slider.maxValue)
        {
            Debug.Log("🎉 Nấu xong rồi!");
            onCookingCompleted?.Invoke();

            // 👉 Khóa muôi
            var ladleDrag = ladle.GetComponent<LandleUIDrag>();
            if (ladleDrag != null)
            {
                ladleDrag.SetCanDrag(false);
            }
        }
    }
}

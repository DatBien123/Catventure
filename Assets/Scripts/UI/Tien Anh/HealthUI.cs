using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // sau nếu mở rộng ta có thể thêm cái sprite khác của trái tim 1/4, 1/3, 1/2 , full trái tim nữa
    public Sprite fullHeart, emptyHeart; 
    public Image[] hearts;
    public int hpPerHeart = 1; // Mở rộng thêm ta cần thay đổi 1 trái tim bằng bao nhiêu HP
    public HealthSystem healthSystem;
    private void Awake()
    {
        
    }
    void Start()
    {
        healthSystem.SetupHealth(hearts.Length * hpPerHeart);
        healthSystem.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI(healthSystem.currentHealth, healthSystem.maxHealth);
    }

    // Update is called once per frame
    void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        int totalHearts = hearts.Length;

        for (int i = 0; i < totalHearts; i++) {

            // Ta sẽ xử lý hiển thị trái tim bằng sprite tương ứng dựa vào HP hiện tại
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;

            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

    }
    private void OnDestroy()
    {
        healthSystem.OnHealthChanged -= UpdateHealthUI; 
    }
}

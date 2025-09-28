using System;
using UnityEngine;
using UnityEngine.UI;

// Hiện tại ta sẽ làm 1 trái tim = 1 hp
// Tương lai nếu muốn thay đổi 1 trái tim = 2 hp, 3hp hay 4 hp đều được
public class HealthSystem : MonoBehaviour
{
    // xử lý Logic của máu
    public  int maxHealth { get; private set; }
    public int currentHealth { get; private set; }

    public event Action<int, int> OnHealthChanged;
    public event Action OnHealthZero ;

    public void SetupHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }
    public void DecreaseHealth(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        // báo sự kiện giảm máu để cập nhật UI
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0) // máu về 0 rồi là GameOver 
        {
            OnHealthZero?.Invoke();
        }
    }
    public void HealHealth(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

}

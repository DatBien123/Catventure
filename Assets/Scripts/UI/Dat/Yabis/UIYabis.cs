using TMPro;
using UnityEngine;

public class UIYabis : MonoBehaviour
{
    [Header("References")]
    public CharacterPlayer owner;

    [Header("Data")]
    public TextMeshProUGUI Energy;
    public TextMeshProUGUI Coin;

    void Start()
    {
        Energy.text = owner.CurrentEnergy.ToString() + " / " + owner.MaxEnergy.ToString();
        Coin.text = owner.Coin.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateResourceUI()
    {
        Energy.text = owner.CurrentEnergy.ToString() + " / " + owner.MaxEnergy.ToString();
        Coin.text = owner.Coin.ToString();
    }
}

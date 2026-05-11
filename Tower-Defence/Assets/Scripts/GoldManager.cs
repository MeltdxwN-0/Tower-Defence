using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [SerializeField] private int startingGold = 600;
    [SerializeField] private TMP_Text goldText;

    private int currentGold;

    public int CurrentGold => currentGold;

    private void Start()
    {
        currentGold = startingGold;
        UpdateGoldUI();
    }

    public bool CanAfford(int cost)
    {
        return currentGold >= cost;
    }

    public bool SpendGold(int amount)
    {
        if (!CanAfford(amount))
        {
            Debug.Log("Ikke nok gold!");
            return false;
        }

        currentGold -= amount;
        UpdateGoldUI();
        return true;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }
}
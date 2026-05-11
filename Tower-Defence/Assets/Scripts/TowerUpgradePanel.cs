using UnityEngine;
using TMPro;

public class TowerUpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerStatsText;

    private Tower selectedTower;

    private void Start()
    {
        HidePanel();
    }

    public void ShowForTower(Tower tower)
    {
        selectedTower = tower;

        if (panel != null)
            panel.SetActive(true);

        UpdatePanelText();
    }

    public void HidePanel()
    {
        if (selectedTower != null)
        {
            selectedTower.HideRange();
        }

        selectedTower = null;

        if (panel != null)
            panel.SetActive(false);
    }

    private void UpdatePanelText()
    {
        if (selectedTower == null)
            return;

        if (towerNameText != null)
        {
            towerNameText.text = selectedTower.name.Replace("(Clone)", "");
        }

        if (towerStatsText != null)
        {
            towerStatsText.text =
                "Damage: " + selectedTower.Damage + "\n" +
                "Range: " + selectedTower.Range.ToString("0.0") + "\n" +
                "Fire Rate: " + selectedTower.FireRate.ToString("0.0") + "\n\n" +
                "Upgrades kommer senere";
        }
    }
}
using UnityEngine;

public class TowerPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject towerPanel;

    public void ToggleTowerPanel()
    {
        if (towerPanel == null)
            return;

        towerPanel.SetActive(!towerPanel.activeSelf);
    }

    public void OpenTowerPanel()
    {
        if (towerPanel != null)
            towerPanel.SetActive(true);
    }

    public void CloseTowerPanel()
    {
        if (towerPanel != null)
            towerPanel.SetActive(false);
    }
}

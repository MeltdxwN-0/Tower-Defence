using UnityEngine;

public class TowerPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject towerPanel;

    [Header("Pause Button Movement")]
    [SerializeField] private RectTransform pauseButton;
    [SerializeField] private Vector2 pauseButtonNormalPosition;
    [SerializeField] private Vector2 pauseButtonPanelOpenPosition;

    [Header("Start Button Movement")]
    [SerializeField] private RectTransform startButton;
    [SerializeField] private Vector2 startButtonNormalPosition;
    [SerializeField] private Vector2 startButtonPanelOpenPosition;

    private void Start()
    {
        if (towerPanel != null)
            towerPanel.SetActive(false);

        MoveButtons(false);
    }

    public void ToggleTowerPanel()
    {
        if (towerPanel == null)
            return;

        bool shouldOpen = !towerPanel.activeSelf;
        towerPanel.SetActive(shouldOpen);

        MoveButtons(shouldOpen);
    }

    public void OpenTowerPanel()
    {
        if (towerPanel == null)
            return;

        towerPanel.SetActive(true);
        MoveButtons(true);
    }

    public void CloseTowerPanel()
    {
        if (towerPanel == null)
            return;

        towerPanel.SetActive(false);
        MoveButtons(false);
    }

    private void MoveButtons(bool panelIsOpen)
    {
        if (pauseButton != null)
        {
            pauseButton.anchoredPosition = panelIsOpen
                ? pauseButtonPanelOpenPosition
                : pauseButtonNormalPosition;
        }

        if (startButton != null)
        {
            startButton.anchoredPosition = panelIsOpen
                ? startButtonPanelOpenPosition
                : startButtonNormalPosition;
        }
    }
}
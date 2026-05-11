using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TowerSelection : MonoBehaviour
{
    [SerializeField] private TowerPlacement towerPlacement;
    [SerializeField] private TowerUpgradePanel upgradePanel;
    [SerializeField] private LayerMask towerLayer;

    private Camera mainCamera;
    private Tower selectedTower;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (towerPlacement != null && towerPlacement.IsPlacingTower)
                return;

            TrySelectTower();
        }
    }

    private void TrySelectTower()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 mouseWorld2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        Collider2D hit = Physics2D.OverlapPoint(mouseWorld2D, towerLayer);

        if (hit == null)
        {
            DeselectTower();
            return;
        }

        Tower tower = hit.GetComponentInParent<Tower>();

        if (tower == null)
        {
            DeselectTower();
            return;
        }

        SelectTower(tower);
    }

    private void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.HideRange();
        }

        selectedTower = tower;
        selectedTower.ShowRange();

        if (upgradePanel != null)
        {
            upgradePanel.ShowForTower(selectedTower);
        }
    }

    private void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.HideRange();
            selectedTower = null;
        }

        if (upgradePanel != null)
        {
            upgradePanel.HidePanel();
        }
    }
}
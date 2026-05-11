using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerPlacement : MonoBehaviour
{
    [Header("Tower Prefabs")]
    [SerializeField] private GameObject selectedTowerPrefab;
    [SerializeField] private GameObject basicTowerPrefab;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap pathTilemap;

    [Header("Preview")]
    [SerializeField] private Color validPreviewColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color invalidPreviewColor = new Color(1f, 0f, 0f, 0.5f);

    [Header("UI")]
    [SerializeField] private Button cancelPlacementButton;

    [Header("Economy")]
    [SerializeField] private GoldManager goldManager;
    [SerializeField] private int basicTowerCost = 250;

    private Camera mainCamera;
    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    private GameObject previewTower;
    private SpriteRenderer previewRenderer;

    public bool IsPlacingTower => selectedTowerPrefab != null;

    private void Start()
    {
        mainCamera = Camera.main;

        if (cancelPlacementButton != null)
        {
            cancelPlacementButton.onClick.AddListener(CancelPlacement);
            cancelPlacementButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (selectedTowerPrefab != null)
        {
            UpdatePreviewPosition();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            TryPlaceTower();
        }
    }

    public void SelectBasicTower()
    {
        if (basicTowerPrefab == null)
        {
            Debug.LogError("Basic Tower Prefab er ikke koblet i Inspector!");
            return;
        }

        if (goldManager == null)
        {
            Debug.LogError("GoldManager er ikke koblet i Inspector!");
            return;
        }

        if (!goldManager.CanAfford(basicTowerCost))
        {
            Debug.Log("Ikke nok gold til Basic Tower.");
            return;
        }

        selectedTowerPrefab = basicTowerPrefab;
        CreatePreviewTower();
        ShowCancelButton(true);

        Debug.Log("Basic Tower valgt.");
    }

    public void SelectTower(GameObject towerPrefab)
    {
        if (towerPrefab == null)
            return;

        selectedTowerPrefab = towerPrefab;
        CreatePreviewTower();
        ShowCancelButton(true);
    }

    private void CreatePreviewTower()
    {
        if (previewTower != null)
        {
            Destroy(previewTower);
        }

        previewTower = Instantiate(selectedTowerPrefab);
        previewTower.name = selectedTowerPrefab.name + "_Preview";

        Tower previewTowerScript = previewTower.GetComponent<Tower>();

        float previewRange = 3f;

        if (previewTowerScript != null)
        {
            previewRange = previewTowerScript.Range;
            previewTowerScript.enabled = false;
        }

        Collider2D[] colliders = previewTower.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        SpriteRenderer[] renderers = previewTower.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = validPreviewColor;
            renderer.sortingOrder = 100;
        }

        TowerRangeVisual rangeVisual = previewTower.GetComponent<TowerRangeVisual>();

        if (rangeVisual != null)
        {
            rangeVisual.SetRange(previewRange);
            rangeVisual.Show();
        }

        previewRenderer = previewTower.GetComponent<SpriteRenderer>();
    }

    private void UpdatePreviewPosition()
    {
        if (previewTower == null)
            return;

        Vector3Int cellPosition = GetMouseCellPosition();
        Vector3 placePosition = groundTilemap.GetCellCenterWorld(cellPosition);

        previewTower.transform.position = placePosition;

        bool canBuild = CanBuildAt(cellPosition);

        if (previewRenderer != null)
        {
            previewRenderer.color = canBuild ? validPreviewColor : invalidPreviewColor;
        }
    }

    private void TryPlaceTower()
    {
        if (selectedTowerPrefab == null)
            return;

        Vector3Int cellPosition = GetMouseCellPosition();

        if (!CanBuildAt(cellPosition))
        {
            Debug.Log("Kan ikke bygge her.");
            return;
        }

        Vector3 placePosition = groundTilemap.GetCellCenterWorld(cellPosition);

        if (!goldManager.SpendGold(basicTowerCost))
        {
            Debug.Log("Ikke nok gold til å plassere tower.");
            return;
        }

        Instantiate(selectedTowerPrefab, placePosition, Quaternion.identity);
        occupiedCells.Add(cellPosition);

        DestroyPreviewTower();

        selectedTowerPrefab = null;
        ShowCancelButton(false);
    }

    private Vector3Int GetMouseCellPosition()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        return groundTilemap.WorldToCell(mouseWorldPosition);
    }

    private bool CanBuildAt(Vector3Int cellPosition)
    {
        bool hasGround = groundTilemap.HasTile(cellPosition);
        bool isPath = pathTilemap != null && pathTilemap.HasTile(cellPosition);
        bool isOccupied = occupiedCells.Contains(cellPosition);

        return hasGround && !isPath && !isOccupied;
    }

    public void CancelPlacement()
    {
        selectedTowerPrefab = null;
        DestroyPreviewTower();
        ShowCancelButton(false);

        Debug.Log("Tower placement avbrutt.");
    }

    private void DestroyPreviewTower()
    {
        if (previewTower != null)
        {
            Destroy(previewTower);
            previewTower = null;
            previewRenderer = null;
        }
    }

    private void ShowCancelButton(bool show)
    {
        if (cancelPlacementButton != null)
        {
            cancelPlacementButton.gameObject.SetActive(show);
        }
    }
}
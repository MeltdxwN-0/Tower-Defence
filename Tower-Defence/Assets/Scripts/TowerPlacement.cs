using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TowerPlacement : MonoBehaviour
{
    [Header("Tower Prefabs")]
    [SerializeField] private GameObject selectedTowerPrefab;
    [SerializeField] private GameObject basicTowerPrefab;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap pathTilemap;

    private Camera mainCamera;
    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

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

            TryPlaceTower();
        }
    }

    public void SelectBasicTower()
    {
        selectedTowerPrefab = basicTowerPrefab;
        Debug.Log("Basic Tower valgt.");
    }

    public void SelectTower(GameObject towerPrefab)
    {
        selectedTowerPrefab = towerPrefab;
    }

    private void TryPlaceTower()
    {
        if (selectedTowerPrefab == null)
            return;

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector3Int cellPosition = groundTilemap.WorldToCell(mouseWorldPosition);

        bool hasGround = groundTilemap.HasTile(cellPosition);
        bool isPath = pathTilemap != null && pathTilemap.HasTile(cellPosition);
        bool isOccupied = occupiedCells.Contains(cellPosition);

        if (!hasGround || isPath || isOccupied)
        {
            Debug.Log("Kan ikke bygge her.");
            return;
        }

        Vector3 placePosition = groundTilemap.GetCellCenterWorld(cellPosition);

        Instantiate(selectedTowerPrefab, placePosition, Quaternion.identity);
        occupiedCells.Add(cellPosition);

        selectedTowerPrefab = null;
    }
}
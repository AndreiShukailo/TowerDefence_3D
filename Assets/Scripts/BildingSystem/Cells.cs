using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour
{
    [SerializeField] private TowersGrid _towersGrid;
    [SerializeField] private Cell _cellPrefab;

    private void Awake()
    {
        var cells = gameObject.GetComponentsInChildren<Cell>();
        _towersGrid.GridArray = cells;
    }

    [ContextMenu("Generate grid")]
    private void GenerateGrid()
    {
        var cells = gameObject.GetComponentsInChildren<Cell>();
        if (cells != null)
            foreach (var cell in cells)
                DestroyImmediate(cell.gameObject);

        var size = _towersGrid.Size;

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                var cell = Instantiate(_cellPrefab, transform);
                cell.transform.position = new Vector3(0.5f, 0, 0.5f) + new Vector3(x, 0, y);
                cell.CreateCell(x, y);
            }
    }

    [ContextMenu("Update grid")]
    private void UpdateGrid()
    {
        var cells = gameObject.GetComponentsInChildren<Cell>();

        foreach (var cell in cells)
            cell.UpdateCell();
    }
}

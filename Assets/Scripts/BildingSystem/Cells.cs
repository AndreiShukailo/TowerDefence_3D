using UnityEngine;

public class Cells : MonoBehaviour
{
    [SerializeField] private Cell[] _cellPrefabs;
    [SerializeField] private TowersGrid _towersGrid;

    private void Awake()
    {
        var cells = gameObject.GetComponentsInChildren<Cell>();
        _towersGrid.CreateGrid(cells);
    }

    [ContextMenu("Generate grid")]
    private void GenerateGrid()
    {
        var cells = gameObject.GetComponentsInChildren<Cell>();
        if (cells != null)
            foreach (var cell in cells)
                DestroyImmediate(cell.gameObject);

        var size = _towersGrid.Size;

        bool white = false;

        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
            {
                var cellpref = white ? _cellPrefabs[0] : _cellPrefabs[1];
                var cell = Instantiate(cellpref, transform);
                cell.transform.position = new Vector3(0.5f, 0, 0.5f) + new Vector3(x, 0, y);
                cell.CreateCell(x, y);
                white = !white;
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

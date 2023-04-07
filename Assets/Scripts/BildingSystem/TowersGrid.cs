using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using System;
using System.Drawing;

public class TowersGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new Vector2Int(10, 10);
    public Vector2Int Size => _size;
    public Cell[] GridArray;

    private Cells _cells;
    private static Cell[,] _grid;
    private Tower _flyingTower;
    private Camera _camera;

    private void Awake()
    {
        CreateGrid();

        _camera = Camera.main;
        _cells = GetComponentInChildren<Cells>();
    }

    private void Start()
    {
        _cells.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_flyingTower != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPosition = ray.GetPoint(enter);

                var x = (int)worldPosition.x;
                var y = (int)worldPosition.z;
                var avalible = false;

                if (GridArray.Any(cell => (cell.X == x) && (cell.Y == y) && cell.IsPlasingCell && cell.AllowPlasing))
                    avalible = true;

                if (avalible)
                    _flyingTower.transform.position = new Vector3(x + 0.5f, 0, y + 0.5f);
                else
                    _flyingTower.transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);

                _flyingTower.SetTransparent(avalible);

                if (Input.GetMouseButtonUp(0))
                {
                    PlaceFlyingTower(x, y, avalible);
                }
            }
        }
    }

    private void CreateGrid()
    {
        _grid = new Cell[_size.x, _size.y];
        foreach (var cell in GridArray)
            _grid[cell.X, cell.Y] = cell;
    }

    public void StartPlacingTower(Tower towerPref)
    {
        if (_flyingTower != null)
            Destroy(_flyingTower.gameObject);

        _flyingTower = Instantiate(towerPref);
        _cells.gameObject.SetActive(true);
    }

    private void PlaceFlyingTower(int placeX, int plaseY, bool avalible)
    {
        if (avalible)
        {
            _flyingTower.SetDefaultTransparent();
            _grid[placeX, plaseY].PlaceTower();

            _cells.gameObject.SetActive(false);
        }
        else
        {
            Destroy(_flyingTower.gameObject);
            _cells.gameObject.SetActive(false);
        }

        _flyingTower = null;
    }
}

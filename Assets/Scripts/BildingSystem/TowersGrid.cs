using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class TowersGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new Vector2Int(10, 10);
    public Vector2Int Size => _size;
    public static TowersGrid Instance;
    private Cells _cells;
    private Cell[] _gridArray;
    private static Cell[,] _grid;
    private Camera _camera;
    private Plane _plane;

    private Tower _flyingTower;
    public UnityAction OnStartPlacingTower;
    public UnityAction OnStopPlacingTower;

    private void Awake()
    {
        Instance = this;
        _camera = Camera.main;
        _cells = GetComponentInChildren<Cells>();
    }

    private void Start()
    {
        _cells.gameObject.SetActive(false);
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (_flyingTower != null)
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out float enter))
            {
                Vector3 worldPosition = ray.GetPoint(enter);
                worldPosition += new Vector3(-1, 0, -2);
                var x = (int)worldPosition.x;
                var y = (int)worldPosition.z;
                var avalible = false;

                if (_gridArray.Any(cell => (cell.X == x) && (cell.Y == y) && cell.IsPlasingCell && cell.AllowPlasing))
                    avalible = true;

                if (avalible)
                    _flyingTower.transform.position = new Vector3(x + 0.5f, 0, y + 0.5f);
                else
                    _flyingTower.transform.position = new Vector3(worldPosition.x, 0.5f, worldPosition.z);

                _flyingTower.SetTransparent(avalible);

                if (Input.GetMouseButtonUp(0))
                {
                    PlaceFlyingTower(x, y, avalible);
                }
            }
        }
    }

    public void CreateGrid(Cell[] gridArray)
    {
        _gridArray = gridArray;
        _grid = new Cell[_size.x, _size.y];
        foreach (var cell in gridArray)
            _grid[cell.X, cell.Y] = cell;
    }

    public void StartPlacingTower(Tower towerPref)
    {
        if (Stats.Instance.Pay(towerPref.Coast))
        {
            _flyingTower = GlobalObjectPoller.instance.Get(towerPref);
            _cells.gameObject.SetActive(true);
            TowersController.Instance.DeactiveCurrenTower();

            OnStartPlacingTower?.Invoke();
        }
    }

    private void PlaceFlyingTower(int placeX, int plaseY, bool avalible)
    {
        if (avalible)
        {
            _flyingTower.StartBildTower(placeX, plaseY);
            _grid[placeX, plaseY].PlaceTower();

            _cells.gameObject.SetActive(false);
        }
        else
        {
            _flyingTower.gameObject.SetActive(false);
            _cells.gameObject.SetActive(false);
        }

        _flyingTower = null;

        OnStopPlacingTower?.Invoke();
    }

    public void DestroyTower(int placeX, int plaseY)
    {
        _grid[placeX, plaseY].DestroyTower();
    }
}

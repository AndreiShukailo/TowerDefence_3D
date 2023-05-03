using UnityEngine;
using UnityEngine.Events;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private LevelUI _levelUI;
    [SerializeField] private TowersGrid _towerGrid;
    [SerializeField] private float _maxX = 20;
    [SerializeField] private float _maxY = 15;
    [SerializeField] private float _minX = 10;
    [SerializeField] private float _minY = 5;
    [SerializeField] private float _zoomOutMax = 4;
    [SerializeField] private float _zoomOutMin = 2;
    public static CameraMovement Instance { get; private set; }
    private bool _allowMove = true;
    private Vector3 _tochStart;
    private Camera _camera;
    private bool _allowMooveCamera = true;
    private bool _allowTowerClick = true;
    public bool AllowTowerClick => _allowTowerClick;
    private Plane _groundPlane;
    private bool _isStrifCameraZoom = false;
    public UnityAction OnCameraMoove;
    private float _streifSpeed = 1f;
    private bool _zooming = false;

    private float minX, minY, maxX, maxY;
    private void Awake()
    {
        Instance = this;
        _camera = Camera.main;
    }

    private void OnDrawGizmos()
    {
        var distance = Camera.main.aspect * Camera.main.orthographicSize;
        var cos = 2 / Mathf.Sqrt(2);
        var minX = _minX + (Camera.main.orthographicSize * cos) + Camera.main.transform.position.y;
        var maxX = _maxX + (Camera.main.transform.position.y - (Camera.main.orthographicSize * cos));
        var minY = _minY + distance;
        var maxY = _maxY - distance;

        Gizmos.DrawLine(new Vector3(minX, 0, minY), new Vector3(maxX, 0, minY));
        Gizmos.DrawLine(new Vector3(maxX, 0, minY), new Vector3(maxX, 0, maxY));
        Gizmos.DrawLine(new Vector3(maxX, 0, maxY), new Vector3(minX, 0, maxY));
        Gizmos.DrawLine(new Vector3(minX, 0, maxY), new Vector3(minX, 0, minY));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_minX, 0, _minY), new Vector3(_maxX, 0, _minY));
        Gizmos.DrawLine(new Vector3(_maxX, 0, _minY), new Vector3(_maxX, 0, _maxY));
        Gizmos.DrawLine(new Vector3(_maxX, 0, _maxY), new Vector3(_minX, 0, _maxY));
        Gizmos.DrawLine(new Vector3(_minX, 0, _maxY), new Vector3(_minX, 0, _minY));

    }
    private void Start()
    {
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
        ResizeBorder();
    }

    private void OnEnable()
    {
        _levelUI.OnOpenScreen += StartPlacingTower;
        _levelUI.OnCloseScreen += StopPlacingTower;
        _towerGrid.OnStartPlacingTower += StartPlacingTower;
        _towerGrid.OnStopPlacingTower += StopPlacingTower;
    }

    private void OnDisable()
    {
        _levelUI.OnOpenScreen -= StartPlacingTower;
        _levelUI.OnCloseScreen -= StopPlacingTower;
        _towerGrid.OnStartPlacingTower -= StartPlacingTower;
        _towerGrid.OnStopPlacingTower -= StopPlacingTower;
    }

    private void Update()
    {
        if (_allowMove)
        {
            if (Input.touchCount == 2)
            {
                _zooming = true;
                _allowMooveCamera = false;
                var tochZero = Input.GetTouch(0);
                var tochOne = Input.GetTouch(1);

                Vector2 tochZeroPrevPos = tochZero.position - tochZero.deltaPosition;
                Vector2 tochOnePrevPos = tochOne.position - tochOne.deltaPosition;

                float prevMagnitude = (tochZeroPrevPos - tochOnePrevPos).magnitude;
                float curentMagnitude = (tochZero.position - tochOne.position).magnitude;

                float difference = curentMagnitude - prevMagnitude;
                Zoom(difference * 0.003f);

                _allowTowerClick = false;
            }
            else if (Input.touchCount == 1 || Input.GetMouseButton(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (_groundPlane.Raycast(ray, out float enter))
                {
                    Vector3 worldPosition = ray.GetPoint(enter);

                    if (Input.GetMouseButtonDown(0))
                    {
                        _allowTowerClick = true;
                        _allowMooveCamera = true;
                        _tochStart = worldPosition;
                    }
                    if (_allowMooveCamera && Input.GetMouseButton(0))
                    {
                        var direction = _tochStart - worldPosition;
                        direction.y = 0;
                        var position = _camera.transform.position + direction;
                        RestrictCamera(position);

                        if (direction != new Vector3(0, 0, 0))
                        {
                            OnCameraMoove?.Invoke();
                            _allowTowerClick = false;
                        }
                    }
                }

            }
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }

        if (_isStrifCameraZoom && !_zooming)
            StraifCameraZoom();

        _zooming = false;
    }

    private void Zoom(float increment)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - increment, _zoomOutMin - 0.2f, _zoomOutMax + 0.2f);
        if (_camera.orthographicSize < _zoomOutMin || _camera.orthographicSize > _zoomOutMax)
            _isStrifCameraZoom = true;
        else
            _isStrifCameraZoom = false;
        ResizeBorder();
        RestrictCamera(_camera.transform.position);
    }

    private void StraifCameraZoom()
    {
        var zoom = Mathf.Abs(_camera.orthographicSize - _zoomOutMin) < Mathf.Abs(_camera.orthographicSize - _zoomOutMax) ? _zoomOutMin : _zoomOutMax;
        _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, zoom, Time.deltaTime * _streifSpeed);
        if (_camera.orthographicSize == zoom)
            _isStrifCameraZoom = false;
    }

    private void RestrictCamera(Vector3 newPosition)
    {
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, minY, maxY);
        _camera.transform.position = newPosition;
    }

    private void ResizeBorder()
    {
        var distance = Camera.main.aspect * Camera.main.orthographicSize;
        var cos = 2 / Mathf.Sqrt(2);
        minX = _minX + (Camera.main.orthographicSize * cos) + Camera.main.transform.position.y;
        maxX = _maxX + (Camera.main.transform.position.y - (Camera.main.orthographicSize * cos));
        minY = _minY + distance;
        maxY = _maxY - distance;
    }

    private void StartPlacingTower()
    {
        _allowMove = false;
    }

    private void StopPlacingTower()
    {
        _allowMove = true;
    }
}

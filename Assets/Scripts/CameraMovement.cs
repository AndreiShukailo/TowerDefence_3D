using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraMovement : MonoBehaviour
{
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

    private void Awake()
    {
        Instance = this;
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        _groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void OnEnable()
    {
        TowersGrid.Instance.OnStartPlacingTower += StartPlacingTower;
        TowersGrid.Instance.OnStopPlacingTower += StopPlacingTower;
    }

    private void OnDisable()
    {
        TowersGrid.Instance.OnStartPlacingTower -= StartPlacingTower;
        TowersGrid.Instance.OnStopPlacingTower -= StopPlacingTower;
    }

    private void Update()
    {
        if (_allowMove)
        {
            if (Input.touchCount == 2)
            {
                _allowMooveCamera = false;
                var tochZero = Input.GetTouch(0);
                var tochOne = Input.GetTouch(1);

                Vector2 tochZeroPrevPos = tochZero.position - tochZero.deltaPosition;
                Vector2 tochOnePrevPos = tochOne.position - tochOne.deltaPosition;

                float prevMagnitude = (tochZeroPrevPos - tochOnePrevPos).magnitude;
                float curentMagnitude = (tochZero.position - tochOne.position).magnitude;

                float difference = curentMagnitude - prevMagnitude;
                Zoom(difference * 0.001f);

                _allowTowerClick = false;
            }
            else if (Input.touchCount <= 1)
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
                    if (Input.GetMouseButton(0) && _allowMooveCamera)
                    {
                        var direction = _tochStart - worldPosition;
                        direction.y = 0;
                        var position = _camera.transform.position + direction;
                        position.x = Mathf.Clamp(position.x, _minX, _maxX);
                        position.z = Mathf.Clamp(position.z, _minY, _maxY);
                        _camera.transform.position = position;
                        
                        if(direction != new Vector3(0,0,0))
                            _allowTowerClick = false;
                    }
                }

            }
            Zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    private void Zoom(float increment)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - increment, _zoomOutMin, _zoomOutMax);
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

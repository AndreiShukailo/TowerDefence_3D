using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private BildProgress _bildProgress;
    [SerializeField] private TowerInfoUI _towerInfoUI;
    [SerializeField] private Tower _tower;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _bildProgress.gameObject.SetActive(false);
        DisableTowerInfo();
        CameraMovement.Instance.OnCameraMoove += RotateCanvasOnCamera;
        _tower.ActiveTower += EnableTowerInfo;
        _tower.DeactiveTower += DisableTowerInfo;
        _tower.OnStartBildingTower += StartBildTower;
        _tower.OnStartBildingTower += RotateCanvasOnCamera;
        _tower.OnEndBildingTower += EndBildTower;
        _tower.OnBildingTower += _bildProgress.SetBildProgress;
        _tower.OnStartDestroying += StartBildTower;
        _tower.OnDestroying += _bildProgress.SetBildProgress;
        _tower.OnDestroyTower += EndBildTower;
    }

    private void OnDisable()
    {
        CameraMovement.Instance.OnCameraMoove -= RotateCanvasOnCamera;
        _tower.ActiveTower -= EnableTowerInfo;
        _tower.DeactiveTower -= DisableTowerInfo;
        _tower.OnStartBildingTower -= StartBildTower;
        _tower.OnEndBildingTower -= EndBildTower;
        _tower.OnBildingTower -= _bildProgress.SetBildProgress;
        _tower.OnStartDestroying -= StartBildTower;
        _tower.OnDestroying -= _bildProgress.SetBildProgress;
        _tower.OnDestroyTower -= EndBildTower;
    }

    private void RotateCanvasOnCamera()
    {
        transform.LookAt(new Vector3(_camera.transform.position.x, _camera.transform.position.y, transform.position.z));
    }

    private void StartBildTower()
    {
        _bildProgress.gameObject.SetActive(true);
    }

    private void EndBildTower()
    {
        _bildProgress.gameObject?.SetActive(false);
    }

    private void EnableTowerInfo()
    {
        _towerInfoUI.gameObject.SetActive(true);
    }

    private void DisableTowerInfo()
    {
        _towerInfoUI.gameObject.SetActive(false);
    }
}

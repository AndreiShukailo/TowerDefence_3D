using UnityEngine;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private BildProgress _bildProgress;
    [SerializeField] private Tower _tower;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _bildProgress.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        RotateCanvasOnCamera();
        CameraMovement.Instance.OnCameraMoove += RotateCanvasOnCamera;
        _tower.OnStartBildingTower += StartBildTower;
        _tower.OnEndBildingTower += EndBildTower;
        _tower.OnBildingTower += _bildProgress.SetBildProgress;
    }

    private void OnDisable()
    {
        CameraMovement.Instance.OnCameraMoove -= RotateCanvasOnCamera;
        _tower.OnStartBildingTower -= StartBildTower;
        _tower.OnEndBildingTower -= EndBildTower;
        _tower.OnBildingTower -= _bildProgress.SetBildProgress;
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
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private TowerInfoUI _towerInfoUI;
    [SerializeField] private Tower _tower;
    [SerializeField] private TMP_Text _priseForDestroy;
    [SerializeField] private TMP_Text _priseForUpgrade;
    [SerializeField] private Button _upgradeButton;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        if (_tower.NextTower != null)
            _priseForUpgrade.text = _tower.NextTower.Coast.ToString();
        _priseForDestroy.text = (_tower.Coast/2).ToString();
    }

    private void OnEnable()
    {
        DisableTowerInfo();
        CameraMovement.Instance.OnCameraMoove += RotateCanvasOnCamera;
        _tower.ActiveTower += EnableTowerInfo;
        _tower.DeactiveTower += DisableTowerInfo;
        _tower.OnStartBildingTower += RotateCanvasOnCamera;
        Stats.Instance.OnMoneyChange += UpdateUpgradeInteractible;
    }

    private void OnDisable()
    {
        CameraMovement.Instance.OnCameraMoove -= RotateCanvasOnCamera;
        _tower.ActiveTower -= EnableTowerInfo;
        _tower.DeactiveTower -= DisableTowerInfo;
        Stats.Instance.OnMoneyChange -= UpdateUpgradeInteractible;
    }

    private void RotateCanvasOnCamera()
    {
        transform.LookAt(new Vector3(_camera.transform.position.x, _camera.transform.position.y, transform.position.z));
    }

    private void EnableTowerInfo()
    {
        _towerInfoUI.gameObject.SetActive(true);
    }

    private void DisableTowerInfo()
    {
        _towerInfoUI.gameObject.SetActive(false);
    }

    private void UpdateUpgradeInteractible(int money)
    {
        if (_tower.NextTower != null)
            _upgradeButton.interactable = money < _tower.NextTower.Coast ? false : true;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private GameObject _towerInfoUI;
    [SerializeField] private Tower _tower;
    [SerializeField] private TMP_Text _priseForDestroy;
    [SerializeField] private TMP_Text _priseForUpgrade;
    [SerializeField] private Button _upgradeButton;

    private void Awake()
    {
        if (_tower.NextTower != null)
            _priseForUpgrade.text = _tower.NextTower.Coast.ToString();
        _priseForDestroy.text = (_tower.Coast/2).ToString();
    }

    private void OnEnable()
    {
        DisableTowerInfo();
        _tower.ActiveTower += EnableTowerInfo;
        _tower.DeactiveTower += DisableTowerInfo;
        Stats.Instance.OnMoneyChange += UpdateUpgradeInteractible;
    }

    private void OnDisable()
    {
        _tower.ActiveTower -= EnableTowerInfo;
        _tower.DeactiveTower -= DisableTowerInfo;
        Stats.Instance.OnMoneyChange -= UpdateUpgradeInteractible;
    }

    private void EnableTowerInfo()
    {
        _towerInfoUI.SetActive(true);
    }

    private void DisableTowerInfo()
    {
        _towerInfoUI.SetActive(false);
    }

    private void UpdateUpgradeInteractible(int money)
    {
        if (_tower.NextTower != null)
            _upgradeButton.interactable = money < _tower.NextTower.Coast ? false : true;
    }
}

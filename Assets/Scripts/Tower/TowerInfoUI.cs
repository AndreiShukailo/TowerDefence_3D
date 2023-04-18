using UnityEngine;
using UnityEngine.UI;

public class TowerInfoUI : MonoBehaviour
{
    [SerializeField] private Button _close;

    private void OnEnable()
    {
        _close.onClick.AddListener(TowersController.Instance.DeactiveCurrenTower);
    }

    private void OnDisable()
    {
        _close.onClick.RemoveListener(TowersController.Instance.DeactiveCurrenTower);
    }
}

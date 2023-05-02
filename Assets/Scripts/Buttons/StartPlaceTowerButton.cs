using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartPlaceTowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Tower _tower;
    [SerializeField] private Stats _stats;
    [SerializeField] private TMP_Text _price;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _price.text = _tower.Coast.ToString();
    }

    private void OnEnable()
    {
        _stats.OnMoneyChange += UpdateInteractible;
    }

    private void OnDisable()
    {
        _stats.OnMoneyChange += UpdateInteractible;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_button.interactable == true)
            TowersGrid.Instance.StartPlacingTower(_tower);
    }

    private void UpdateInteractible(int money)
    {
        _button.interactable = money < _tower.Coast ? false : true;
    }
}

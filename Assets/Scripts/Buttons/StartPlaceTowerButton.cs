using UnityEngine;
using UnityEngine.EventSystems;

public class StartPlaceTowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Tower _tower;

    public void OnPointerDown(PointerEventData eventData)
    {
        TowersGrid.Instance.StartPlacingTower(_tower);    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPlaceTowerButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TowersGrid _towersGrid;
    [SerializeField] private Tower _tower;

    public void OnPointerDown(PointerEventData eventData)
    {
        _towersGrid.StartPlacingTower(_tower);    
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider))]
public class Tower : MonoBehaviour
{
    private Renderer[] _mainRender;
    private Color[] _defaultColor;
    private bool _isRedy;
    private bool _isActive = false;
    public bool IsRedy => _isRedy;
    public UnityAction ActiveTower;
    public UnityAction DeactiveTower;
    private void Awake()
    {
        _mainRender = GetComponentsInChildren<Renderer>();
        _defaultColor = _mainRender.Select(render => render.material.color).ToArray();
    }

    public void BildTower()
    {
        _isRedy = true;
        SetDefaultTransparent();
    }

    public void SetTransparent(bool avalible)
    {
        foreach (var renderer in _mainRender)
            renderer.material.color = avalible ? Color.green : Color.red;
    }

    private void SetDefaultTransparent()
    {
        for (int i = 0; i < _mainRender.Length; i++)
        {
            _mainRender[i].material.color = _defaultColor[i];
        }
    }

    private void OnMouseUpAsButton()
    {
        if (CameraMovement.Instance.AllowTowerClick && _isRedy)
        {
            if (!_isActive)
                SetActiveTower();
            else
                TowersController.Instance.DeactiveCurrenTower();
        }

    }

    private void SetActiveTower()
    {
        ActiveTower?.Invoke();
        TowersController.Instance.SwitschActiveTower(this);
        _isActive = true;
    }

    public void SetDeactiveTower()
    {
        DeactiveTower?.Invoke();
        _isActive = false;
    }
}

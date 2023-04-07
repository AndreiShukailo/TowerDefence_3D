using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Renderer _mainRender;

    private Color _defaultColor;

    private void Awake()
    {
        _defaultColor = _mainRender.material.color;
    }

    public void SetTransparent(bool avalible)
    {
        _mainRender.material.color = avalible ? Color.green : Color.red;
    }

    public void SetDefaultTransparent()
    {
        _mainRender.material.color = _defaultColor;
    }
}

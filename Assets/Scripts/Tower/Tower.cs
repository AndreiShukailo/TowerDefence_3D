using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider), typeof(Outline))]
public class Tower : MonoBehaviour
{
    private Outline _outline;
    private Renderer[] _mainRender;
    private Color[] _defaultColor;
    private float _bildTime = 3f;
    private bool _isRedy = false;
    private bool _isActive = false;
    private bool _isBilding = false;
    private float _timer;

    public bool IsRedy => _isRedy;
    public UnityAction ActiveTower;
    public UnityAction DeactiveTower;
    public UnityAction OnStartBildingTower;
    public UnityAction<float, float> OnBildingTower;
    public UnityAction OnEndBildingTower;
    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _mainRender = GetComponentsInChildren<Renderer>();
        _defaultColor = _mainRender.Select(render => render.material.color).ToArray();
    }

    private void Update()
    {
        if (_isBilding)
        {
            _timer += Time.deltaTime;
            BildingTower(_timer, _bildTime);
        }
    }

    public void StartBildTower()
    {
        OnStartBildingTower?.Invoke();
        _isBilding = true;
    }

    private void BildingTower(float curren, float max)
    {
        OnBildingTower?.Invoke(curren, max);
        if (curren >= max)
            BildTower();
    }

    private void BildTower()
    {
        _isRedy = true;
        _isBilding = false;
        _timer = 0;
        SetDefaultTransparent();
        OnEndBildingTower?.Invoke();
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
        _outline.enabled = true;
        ActiveTower?.Invoke();
        TowersController.Instance.SwitschActiveTower(this);
        _isActive = true;
    }

    public void SetDeactiveTower()
    {
        _outline.enabled = false;
        DeactiveTower?.Invoke();
        _isActive = false;
    }
}

using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider), typeof(Outline))]
public class Tower : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Tower _nextTower;
    [SerializeField] private Renderer[] _towerRenderers;
    public Tower NextTower => _nextTower;
    [SerializeField] private int _coast;
    public int Coast => _coast;
    private int _x;
    private int _y;

    private Outline _outline;
    private bool _isRedy = false;
    private bool _isActive = false;
    public bool IsRedy => _isRedy;
    public UnityAction ActiveTower;
    public UnityAction DeactiveTower;
    public UnityAction OnDestroyTower;
    public UnityAction OnCreateTower;
    public UnityAction OnBildTower;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    private void OnDisable()
    {
        ResetTower();
    }

    public void UpgradeTower()
    {
        if (_nextTower != null)
        {
            if (Stats.Instance.Pay(_nextTower.Coast))
            {
                var newTower = GlobalObjectPoller.instance.Get(_nextTower);
                newTower.transform.position = transform.position;
                newTower.BildTower(_x, _y);
                TowersGrid.Instance.BildTower(_x, _y);
                this.DestroyTower();
            }
        }
    }

    private void ResetTower()
    {
        _isRedy = false;
        _isActive = false;
    }

    public void BildTower(int x, int y)
    {
        _isRedy = true;
        SetDefaultTransparent();
        OnBildTower?.Invoke();
        _x = x;
        _y = y;
    }

    public void DestroyTower()
    {
        OnDestroyTower?.Invoke();
        TowersGrid.Instance.DestroyTower(_x, _y);
        TowersController.Instance.DeactiveCurrenTower();
        gameObject.SetActive(false);
    }

    public void SetTransparent(bool avalible)
    {
        foreach (var renderer in _towerRenderers)
            renderer.material.color = avalible ? Color.green : Color.red;
    }

    private void SetDefaultTransparent()
    {
        foreach (var renderer in _towerRenderers)
            renderer.material.color = Color.white;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CameraMovement.Instance.AllowTowerClick && _isRedy)
        {
            if (!_isActive)
                SetActiveTower();
            else
                TowersController.Instance.DeactiveCurrenTower();
        }
        else
            TowersController.Instance.DeactiveCurrenTower();
    }
}

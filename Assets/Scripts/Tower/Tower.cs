using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider), typeof(Outline))]
public class Tower : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Tower _nextTower;
    public Tower NextTower => _nextTower;
    [SerializeField] private int _coast;
    public int Coast => _coast;
    private int _x;
    private int _y;

    private Outline _outline;
    private Renderer[] _mainRender;
    private Color[] _defaultColor;
    private float _bildTime = 0.1f;
    private bool _isRedy = false;
    private bool _isActive = false;
    private bool _isBilding = false;
    private bool _isDestroying = false;
    private float _timer;
    public bool IsRedy => _isRedy;
    public UnityAction ActiveTower;
    public UnityAction DeactiveTower;
    public UnityAction OnStartBildingTower;
    public UnityAction<float, float> OnBildingTower;
    public UnityAction OnEndBildingTower;
    public UnityAction OnStartDestroying;
    public UnityAction<float, float> OnDestroying;
    public UnityAction OnDestroyTower;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _mainRender = GetComponentsInChildren<Renderer>();
        _defaultColor = _mainRender.Select(render => render.material.color).ToArray();
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
                newTower.StartBildTower(_x, _y);
                TowersController.Instance.DeactiveCurrenTower();
                this.DestroyTower();
                TowersGrid.Instance.BildTower(_x, _y);
            }
        }
    }

    private void ResetTower()
    {
        _isRedy = false;
        _isBilding = false;
        _isActive = false;
        _isDestroying = false;
        _timer = 0;
    }

    private void Update()
    {
        if (_isBilding)
        {
            _timer += Time.deltaTime;
            BildingTower(_timer, _bildTime);
        }
        if (_isDestroying)
        {
            _timer -= Time.deltaTime;
            DestroyingTower(_timer, _bildTime);
        }
    }

    public void StartBildTower(int x, int y)
    {
        OnStartBildingTower?.Invoke();
        _isBilding = true;
        _x = x;
        _y = y;
    }

    private void BildingTower(float current, float max)
    {
        OnBildingTower?.Invoke(current, max);
        if (current >= max)
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

    public void StartDestroyingTower()
    {
        _timer = _bildTime;
        Stats.Instance.ReciveMoney(_coast / 2);
        OnStartDestroying?.Invoke();
        _isDestroying = true;
        _isRedy = false;
        TowersController.Instance.DeactiveCurrenTower();
    }

    private void DestroyingTower(float current, float max)
    {
        OnDestroying?.Invoke(current, max);
        if (current <= 0)
            DestroyTower();
    }

    private void DestroyTower()
    {
        _isBilding = false;
        _isDestroying = false;
        _timer = 0;
        OnDestroyTower?.Invoke();
        TowersGrid.Instance.DestroyTower(_x, _y);
        gameObject.SetActive(false);
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
    }
}

using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _allow;
    [SerializeField] private Material _ban;
    [SerializeField] private int _x;
    [SerializeField] private int _y;
    [SerializeField] private bool _isPlasingCell;
    public bool AllowPlasing;
    public bool IsPlasingCell => _isPlasingCell;
    public int X => _x;
    public int Y => _y;

    public void CreateCell(int x, int y)
    {
        _x = x;
        _y = y;
    }

    [ContextMenu("UpdateCell")]
    public void UpdateCell()
    {
        _renderer.enabled = _isPlasingCell;
        if (_isPlasingCell)
        {
            _renderer.material = AllowPlasing ? _allow : _ban;
        }
        else AllowPlasing = false;
    }

    public void PlaceTower()
    {
        AllowPlasing = false;
        _renderer.material = _ban;
    }
}

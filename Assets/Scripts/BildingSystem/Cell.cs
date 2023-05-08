using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Color _allow;
    [SerializeField] private Color _ban;
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
        gameObject.SetActive(_isPlasingCell);
        if (_isPlasingCell)
        {
            _image.color = AllowPlasing ? _allow : _ban;
        }
        else AllowPlasing = false;
    }

    public void PlaceTower()
    {
        AllowPlasing = false;
        _image.color = _ban;
    }

    public void DestroyTower()
    {
        AllowPlasing = true;
        _image.color = _allow;
    }
}

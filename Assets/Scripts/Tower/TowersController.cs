using UnityEngine;

public class TowersController : MonoBehaviour
{
    public static TowersController Instance;
    private Tower _currentActive;

    private void Awake()
    {
        Instance = this;
    }

    //private void OnEnable()
    //{
    //    CameraMovement.Instance.OnCameraMoove += DeactiveCurrenTower;    
    //}

    //private void OnDisable()
    //{
    //    CameraMovement.Instance.OnCameraMoove -= DeactiveCurrenTower;
    //}

    public void SwitschActiveTower(Tower tower)
    {
        if (_currentActive != null)
            _currentActive.SetDeactiveTower();

        _currentActive = tower;
    }

    public void DeactiveCurrenTower()
    {
        if (_currentActive != null)
        {
            _currentActive.SetDeactiveTower();
            _currentActive = null;
        }
    }
}

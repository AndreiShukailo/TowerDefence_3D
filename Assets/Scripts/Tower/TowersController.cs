using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersController : MonoBehaviour
{
    public static TowersController Instance;
    private Tower _currentActive;

    private void Awake()
    {
        Instance = this;
    }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorController : MonoBehaviour
{
    [SerializeField] private LevelSelector[] _levels;

    private void OnEnable()
    {
        UpdateLevels();
    }

    private void UpdateLevels()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            if (i <= LevelController.instance.LastLevelId)
                _levels[i].SetUnlocked(i == LevelController.instance.LastLevelId);
            else
                _levels[i].SetLocked();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private void OnEnable()
    {
        LevelUI.Instance.OnOpenScreen += SetPause;
        LevelUI.Instance.OnCloseScreen += ContinueGame;
        LevelController.instance.OnLoadLevel += ContinueGame;
    }

    private void OnDisable()
    {
        LevelUI.Instance.OnOpenScreen -= SetPause;
        LevelUI.Instance.OnCloseScreen -= ContinueGame;
        LevelController.instance.OnLoadLevel -= ContinueGame;
    }

    private void SetPause()
    {
        Time.timeScale = 0;
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
    }
}

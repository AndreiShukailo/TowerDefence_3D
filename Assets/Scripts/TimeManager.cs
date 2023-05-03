using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool Pause { get; private set; }
    public static TimeManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
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
        Pause = true;
    }

    private void ContinueGame()
    {
        Time.timeScale = 1;
        Pause = false;
    }
}

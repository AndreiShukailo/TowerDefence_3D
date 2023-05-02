using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void StartLevel(int LevelId)
    {
        LevelController.instance.LoadLevel(LevelId);
    }

    public void Exit()
    {
        LevelController.instance.Exit();
    }
}

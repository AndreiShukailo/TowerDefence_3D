using UnityEngine;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour, IPointerClickHandler
{
    private TimeManager _timeManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        TowersController.Instance.DeactiveCurrenTower();
        if(TimeManager.Instance.Pause == true)
            LevelUI.Instance.ContinueGame();
    }
}

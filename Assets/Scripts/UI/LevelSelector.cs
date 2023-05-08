using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _locked;
    [SerializeField] private Image _selector;

    public void SetUnlocked(bool isLast)
    {
        _locked.gameObject.SetActive(false);
        _button.interactable = true;
        EnableSelector(isLast);
    }

    public void SetLocked()
    {
        _locked.gameObject.SetActive(true);
        _button.interactable = false;
        EnableSelector(false);
    }

    private void EnableSelector(bool isEnable)
    {
        _selector.gameObject.SetActive(isEnable);
    }
}

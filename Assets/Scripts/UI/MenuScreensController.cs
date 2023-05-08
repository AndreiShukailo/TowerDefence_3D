using UnityEngine;

public class MenuScreensController : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    private GameObject _currentScreen;

    private void Start()
    {
        _currentScreen = _menu;
        OpenScreen(_menu);
    }

    public void OpenScreen(GameObject screen)
    {
        if (_currentScreen != null)
        {
            _currentScreen.SetActive(false);
            _currentScreen = screen;
            _currentScreen.SetActive(true);
        }
    }
}

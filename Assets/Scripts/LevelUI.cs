using UnityEngine;
using UnityEngine.Events;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _victoryScreen;
    public static LevelUI Instance { get; private set; }
    public UnityAction OnOpenScreen;
    public UnityAction OnCloseScreen;

    private void OnEnable()
    {
        Home.Instance.OnLoose += ActiveGameOverScreen;
        Spawner.Instance.OnVictory += ActiveVictoryScreen;
    }

    private void OnDisable()
    {
        Home.Instance.OnLoose -= ActiveGameOverScreen;
        Spawner.Instance.OnVictory -= ActiveVictoryScreen;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _pauseUI.SetActive(false);
        _gameOverScreen.SetActive(false);
        _victoryScreen.SetActive(false);
    }

    public void SetPause()
    {
        OnOpenScreen?.Invoke();
        _pauseUI.SetActive(true);
    }

    public void ContinueGame()
    {
        _pauseUI.SetActive(false);
        OnCloseScreen?.Invoke();
    }

    public void ExitToMenu()
    {
        LevelController.instance.LoadLevel(0);
    }

    public void RestartLevel(int LevelId)
    {
        LevelController.instance.LoadLevel(LevelId);
    }

    private void ActiveGameOverScreen()
    {
        OnOpenScreen?.Invoke();
        _gameOverScreen.SetActive(true);
    }

    private void ActiveVictoryScreen()
    {
        OnOpenScreen?.Invoke();
        _victoryScreen.SetActive(true);
    }
}

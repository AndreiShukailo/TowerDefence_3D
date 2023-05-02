using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public UnityAction OnLoadLevel;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (FindObjectOfType<LevelController>() == null)
        {
            gameObject.SetActive(true);
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void LoadLevel(int level)
    {
        OnLoadLevel?.Invoke();
        SceneManager.LoadScene(level);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

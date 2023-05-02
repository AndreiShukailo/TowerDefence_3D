using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Stats _stats;
    [SerializeField] private Home _home;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private TMP_Text _moneyCount;
    [SerializeField] private Image _waveProgress;
    [SerializeField] private TMP_Text _currentWaveNumber;
    [SerializeField] private TMP_Text _amountWaves;
    [SerializeField] private GameObject _timer;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private TMP_Text _moneyBack;
    [SerializeField] private Image _homeHealthBar;
    [SerializeField] private TMP_Text _currentHealth;
    [SerializeField] private TMP_Text _maxHealth;

    private void OnEnable()
    {
        _stats.OnMoneyChange += ChangeMoneyCount;
        _spawner.OnWaveUpdate += UpdateWaveInfo;
        _spawner.OnNextWave += EnableTimer;
        _spawner.OnTimerUpdate += UpdateTimer;
        _spawner.OnMoneyBackUpdate += UpdateMoneyBack;
        _spawner.OnUpdateEnemyCount += UpdateEnemyProgress;
        _home.OnUpdateHealth += UpdateHomeHealth;
    }

    private void OnDisable()
    {
        _stats.OnMoneyChange -= ChangeMoneyCount;
        _spawner.OnWaveUpdate -= UpdateWaveInfo;
        _spawner.OnNextWave -= EnableTimer;
        _spawner.OnTimerUpdate -= UpdateTimer;
        _spawner.OnMoneyBackUpdate -= UpdateMoneyBack;
        _spawner.OnUpdateEnemyCount -= UpdateEnemyProgress;
        _home.OnUpdateHealth -= UpdateHomeHealth;
    }

    private void ChangeMoneyCount(int count)
    {
        _moneyCount.text = count.ToString();
    }

    private void UpdateWaveInfo(int curentWavenumber, int amountWaves)
    {
        _currentWaveNumber.text = curentWavenumber.ToString();
        _amountWaves.text = amountWaves.ToString();
        DisableTimer();
    }

    private void EnableTimer()
    {
        _timer.SetActive(true);
    }

    private void DisableTimer()
    {
        _timer.SetActive(false);
    }

    private void UpdateTimer(float time)
    {
        _time.text = Mathf.CeilToInt(time).ToString();
    }

    private void UpdateMoneyBack(int moneyBack)
    {
        _moneyBack.text = moneyBack.ToString();
    }

    private void UpdateEnemyProgress(int enemyCount, int amountEnemy)
    {
        _waveProgress.fillAmount = (float)(enemyCount) / (float)amountEnemy;
    }

    private void UpdateHomeHealth(float currentHealth, float maxHealth)
    {
        _homeHealthBar.fillAmount = currentHealth / maxHealth;
        _maxHealth.text = maxHealth.ToString();
        _currentHealth.text = currentHealth.ToString();
    }
}

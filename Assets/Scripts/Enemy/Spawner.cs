using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawn;
    [SerializeField] private Transform _target;
    [SerializeField] private List<Wave> _waves;
    public static Spawner Instance { get; private set; }

    private Vector3[] _spawnPoints;
    private Vector3[] _targetPoins;
    private int _enemyCount = 0;
    private float _waveTimer;
    private float _enemyTimer;
    private Queue<Wave> _leftWaves = new Queue<Wave>();
    private Queue<Enemy> _leftEnemies = new Queue<Enemy>();
    private Wave _currentWave;
    private Enemy _currentEnemy;
    private int _amountEnemy = 0;
    private int _swawnedEnemy = 0;
    public UnityAction<int, int> OnWaveUpdate;
    public UnityAction OnNextWave;
    public UnityAction<float> OnTimerUpdate;
    public UnityAction<int> OnMoneyBackUpdate;
    public UnityAction<int, int> OnUpdateEnemyCount;
    public UnityAction OnVictory;

    private void Awake()
    {
        Instance = this;
        _spawnPoints = _spawn.GetComponentsInChildren<Transform>().Skip(1).Select(transform => transform.position).ToArray();
        _targetPoins = _target.GetComponentsInChildren<Transform>().Skip(1).Select(transform => transform.position).ToArray();
    }

    private void Start()
    {
        foreach (var wave in _waves)
        {
            _leftWaves.Enqueue(wave);
            _amountEnemy += wave.Enemies.Count;
        }
        OnWaveUpdate?.Invoke(_waves.Count - _leftWaves.Count, _waves.Count);
        OnUpdateEnemyCount?.Invoke(_swawnedEnemy, _amountEnemy);
        _currentWave = GetNextWave();
    }

    private void Update()
    {
        if (_currentWave != null)
        {
            if (_waveTimer >= _currentWave.WaveDeley)
            {
                if (_currentEnemy == null)
                {
                    OnWaveUpdate?.Invoke(_waves.Count - _leftWaves.Count, _waves.Count);
                    _currentEnemy = GetNextEnemy();
                }
                else if (_enemyTimer >= _currentWave.EnemyDeley)
                {
                    SpawnEnemy();
                    _enemyTimer = 0;
                    _currentEnemy = GetNextEnemy();
                }
                else
                {
                    _enemyTimer += Time.deltaTime;
                }
            }
            else
            {
                _waveTimer += Time.deltaTime;
                OnTimerUpdate?.Invoke(_currentWave.WaveDeley - _waveTimer);
                OnMoneyBackUpdate?.Invoke(Mathf.CeilToInt((_currentWave.WaveDeley - _waveTimer) / 2));
            }
        }
    }

    private void SpawnEnemy()
    {
        var enemy = GlobalObjectPoller.instance.Get(_currentEnemy);
        var target = _targetPoins[Random.Range(0, _targetPoins.Length)];
        var position = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        enemy.Create(target, position);
        enemy.OnDeath += EnemyKill;
        _swawnedEnemy++;
        OnUpdateEnemyCount?.Invoke(_swawnedEnemy, _amountEnemy);
    }

    public void StartSpawning()
    {
        if (_currentWave != null)
        {
            Stats.Instance.ReciveMoney(Mathf.CeilToInt((_currentWave.WaveDeley - _waveTimer)/2));
            _waveTimer = _currentWave.WaveDeley;
        }
    }

    private Enemy GetNextEnemy()
    {
        if (_leftEnemies.Count > 0)
            return _leftEnemies.Dequeue();
        else return null;
    }

    private Wave GetNextWave()
    {
        if (_leftWaves.Count > 0)
        {
            _leftEnemies.Clear();
            var wave = _leftWaves.Dequeue();
            _enemyCount = wave.Enemies.Count;
            _waveTimer = 0;
            OnNextWave?.Invoke();
            foreach (var enemy in wave.Enemies)
                _leftEnemies.Enqueue(enemy);
            return wave;
        }
        else
        {
            OnVictory?.Invoke();
            return null;
        }
    }

    private void EnemyKill(Enemy enemy)
    {
        enemy.OnDeath -= EnemyKill;
        _enemyCount--;
        if (_enemyCount <= 0)
            GetNextWave();
    }
}

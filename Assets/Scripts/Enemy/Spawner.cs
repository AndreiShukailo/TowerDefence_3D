using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawn;
    [SerializeField] private Transform _target;
    [SerializeField] private List<Wave> _waves;

    private Vector3[] _spawnPoints;
    private Vector3[] _targetPoins;
    private float _waveTimer;
    private float _enemyTimer;
    private Queue<Wave> _leftWaves = new Queue<Wave>();
    private Queue<Enemy> _leftEnemies = new Queue<Enemy>();
    private Wave _currentWave;
    private Enemy _currentEnemy;

    private void Awake()
    {
        _spawnPoints = _spawn.GetComponentsInChildren<Transform>().Skip(1).Select(transform => transform.position).ToArray();
        _targetPoins = _target.GetComponentsInChildren<Transform>().Skip(1).Select(transform => transform.position).ToArray();
        foreach (var wave in _waves)
            _leftWaves.Enqueue(wave);
    }

    private void Update()
    {
        if (_currentWave == null)
        {
            _currentWave = GetNextWave();
        }
        else if (_waveTimer >= _currentWave.WaveDeley)
        {
            if (_currentEnemy == null)
            {
                _currentEnemy = GetNextEnemy();

                if (_currentEnemy == null)
                {
                    _currentWave = GetNextWave();
                    _waveTimer = 0;
                }
            }
            else if (_enemyTimer >= _currentWave.EnemyDeley)
            {
                SpawnEnemy();
                _enemyTimer = 0;
                _currentEnemy = GetNextEnemy();
            }
        }

        _waveTimer += Time.deltaTime;
        _enemyTimer += Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        var gameObject = GlobalObjectPoller.instance.Get(_currentEnemy.gameObject);
        var target = _targetPoins[Random.Range(0, _targetPoins.Length)];
        var position = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        gameObject.GetComponent<Enemy>().Create(target, position);
    }

    public void StartSpawning()
    {
        if (_currentWave != null)
            _waveTimer = _currentWave.WaveDeley;
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
            foreach (var enemy in wave.Enemies)
                _leftEnemies.Enqueue(enemy);
            return wave;
        }
        else return null;
    }
}

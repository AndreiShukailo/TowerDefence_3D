using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Create new Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private float _enemyDeley;
    [SerializeField] private float _waveDeley;
    public List<Enemy> Enemies => _enemies;
    public float WaveDeley => _waveDeley;
    public float EnemyDeley => _enemyDeley;
}

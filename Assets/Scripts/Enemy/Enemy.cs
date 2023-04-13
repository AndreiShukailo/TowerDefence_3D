using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _maxHealth;
    private EnemyMovement _movement;
    public EnemyMovement Movement => _movement;
    private float _currentHealth;

    public void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
    }

    public void Create(Vector3 target, Vector3 startPosition)
    {
        transform.position = startPosition;
        _movement.SetTarget(target);
    }
}

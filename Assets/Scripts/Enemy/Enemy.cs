using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _maxHealth;
    private EnemyMovement _movement;
    public EnemyMovement Movement => _movement;
    private float _currentHealth;
    public UnityAction OnDeth;

    void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
    }

    public void Create(Vector3 target, Vector3 startPosition)
    {
        transform.position = startPosition;
        _movement.SetTarget(target);
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        OnDeth?.Invoke();
    }
}

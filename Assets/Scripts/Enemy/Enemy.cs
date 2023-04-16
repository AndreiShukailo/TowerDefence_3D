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
    public float MaxHealth => _maxHealth;
    private EnemyMovement _movement;

    public UnityAction<float> OnHealthChanged;
    private float _currentHealth;
    public UnityAction OnDeth;

    void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
    }

    public void Create(Vector3 target, Vector3 startPosition)
    {
        _movement.SetTarget(target, startPosition);
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        OnHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth <= 0)
            Death();
    }

    private void Death()
    {
        _movement.StopMove();
        OnDeth?.Invoke();
        gameObject.SetActive(false);
    }
}

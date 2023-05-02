using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _reward;
    [SerializeField] private float _damage;
    public float Damage => _damage;
    [SerializeField] private float _maxHealth;
    public float MaxHealth => _maxHealth;
    private EnemyMovement _movement;

    public UnityAction<float> OnHealthChanged;
    private float _currentHealth;
    public UnityAction<Enemy> OnDeath;

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
            Death(true);
    }

    public void Death(bool isKilled)
    {
        if(isKilled)
            Stats.Instance.ReciveMoney(_reward);
        _movement.StopMove();
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }
}

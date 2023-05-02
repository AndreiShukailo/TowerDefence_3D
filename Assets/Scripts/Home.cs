using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Home : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 10;
    private float _currentHealth;
    public UnityAction OnLoose;
    public UnityAction<float, float> OnUpdateHealth;
    public static Home Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        OnUpdateHealth?.Invoke(_currentHealth, _maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _currentHealth = Mathf.Clamp(_currentHealth - enemy.Damage, 0, _maxHealth);
            OnUpdateHealth?.Invoke(_currentHealth, _maxHealth);
            if (_currentHealth <= 0)
            {
                enemy.Death(false);
                OnLoose?.Invoke();
            }
            else
            {
                enemy.Death(false);
            }
        }
    }
}

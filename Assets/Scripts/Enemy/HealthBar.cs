using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Image _fill;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _enemy.OnHealthChanged += ChangeHealth;
    }

    private void ChangeHealth(float currentHealt)
    {
        _fill.fillAmount = currentHealt / _enemy.MaxHealth;
    }

    private void LateUpdate()
    {
        transform.LookAt(new Vector3(_camera.transform.position.x, _camera.transform.position.y, transform.position.z));
    }
}

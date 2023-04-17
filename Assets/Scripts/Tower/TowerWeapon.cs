using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CapsuleCollider))]
public class TowerWeapon : MonoBehaviour
{
    [SerializeField] private Transform _damageArea;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _radius;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _shootingSpeed;
    [SerializeField] private Tower _tower;
    private CapsuleCollider _collider;
    private List<Enemy> _targets = new List<Enemy>(0);
    private float _timer = 0;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _collider.radius = _radius;
        _damageArea.transform.localScale = new Vector3(_radius * 2, _damageArea.transform.localScale.y, _radius * 2);
        HidDamageArea();
        _tower.ActiveTower += ShowDamageArea;
        _tower.DeactiveTower += HidDamageArea;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _targets.Add(enemy);
            enemy.OnDeth += () => _targets.Remove(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _targets.Remove(enemy);
        }
    }

    private void Update()
    {
        if (_tower.IsRedy)
        {
            if (_targets.Count > 0)
            {
                RotateWeapon(_targets[0]);
                if (_timer >= 1 / _shootingSpeed)
                    Shoot(_targets[0]);
                else
                    _timer += Time.deltaTime;
            }
        }
            
    }

    private void RotateWeapon(Enemy target)
    {
            var dir = (target.transform.position - transform.position).normalized;
            var loockRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, loockRotation, Time.deltaTime * _rotationSpeed);
    }

    private void Shoot(Enemy target)
    {
        var projectile = GlobalObjectPoller.instance.Get(_projectile);
        projectile.Create(_projectileSpeed, _damage, target, _shootPoint.position);
        _timer = 0;
    }

    private void ShowDamageArea()
    {
        _damageArea.gameObject.SetActive(true);
    }

    private void HidDamageArea()
    {
        _damageArea.gameObject.SetActive(false);
    }
}

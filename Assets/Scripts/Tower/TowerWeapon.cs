using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Tower), typeof(CapsuleCollider))]
public class TowerWeapon : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private Transform _weapon;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _radius;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _shootingSpeed;
    private Tower _tower;
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
        _tower = GetComponent<Tower>();
        _collider.radius = _radius;
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
            var dir = (target.transform.position - _weapon.position).normalized;
            var loockRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            _weapon.transform.rotation = Quaternion.LerpUnclamped(_weapon.transform.rotation, loockRotation, Time.deltaTime * _rotationSpeed);
    }

    private void Shoot(Enemy target)
    {
        var gameObject = GlobalObjectPoller.instance.Get(_projectile.gameObject);
        gameObject.GetComponent<Projectile>().Create(_projectileSpeed, _damage, target, _shootPoint.position);
        _timer = 0;
    }
}

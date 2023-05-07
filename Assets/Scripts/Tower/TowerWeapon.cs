using System.Collections.Generic;
using UnityEngine;

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
    private Quaternion _defaulRotation;
    private Vector3[] _weaponFreeDirections = new Vector3[4];
    private float _freeDirectionTimer = 0;
    private Vector3 _freeDirection = Vector3.zero;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _collider.radius = _radius;
        _damageArea.localScale = Vector3.one * _radius * 2;
        HidDamageArea();
        _tower.ActiveTower += ShowDamageArea;
        _tower.DeactiveTower += HidDamageArea;
        _tower.OnStartBildingTower += HidDamageArea;
        _defaulRotation = transform.rotation;

        _weaponFreeDirections[0] = new Vector3(0.5f, 0, 0.5f);
        _weaponFreeDirections[1] = new Vector3(-0.5f, 0, 0.5f);
        _weaponFreeDirections[2] = new Vector3(0.5f, 0, -0.5f);
        _weaponFreeDirections[3] = new Vector3(-0.5f, 0, -0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _targets.Add(enemy);
            enemy.OnDeath += RemooveFromTargets;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            _targets.Remove(enemy);
            enemy.OnDeath -= RemooveFromTargets;
        }
    }

    private void OnDisable()
    {
        ResetWeapon();
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
            else if (_freeDirectionTimer >= 5 / _shootingSpeed)
            {
                if (_freeDirection == Vector3.zero)
                    _freeDirection = GetRandomFreeDirection();

                if (FreeRotateWeapon(_freeDirection))
                {
                    _freeDirection = Vector3.zero;
                    _freeDirectionTimer = 0;
                }
            }
            else
                _freeDirectionTimer += Time.deltaTime;
        }
            
    }

    private void ResetWeapon()
    {
        _targets.Clear();
        transform.rotation = _defaulRotation;
    }

    private void RotateWeapon(Enemy target)
    {
            var dir = (target.transform.position - transform.position).normalized;
            var loockRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.LerpUnclamped(transform.rotation, loockRotation, Time.deltaTime * _rotationSpeed);
    }

    private bool FreeRotateWeapon(Vector3 direction)
    {
        var loockRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, loockRotation, Time.deltaTime * _rotationSpeed * 0.5f);
        return Quaternion.Angle(transform.rotation, loockRotation) == 0;
    }

    private Vector3 GetRandomFreeDirection()
    {
        while (true)
        {
            var direction = _weaponFreeDirections[Random.Range(0, _weaponFreeDirections.Length)];
            var loockRotation = Quaternion.LookRotation(direction);
            if (Quaternion.Angle(transform.rotation, loockRotation) != 0)
                return direction;
        }
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

    private void RemooveFromTargets(Enemy enemy)
    {
        _targets.Remove(enemy);
    }
}

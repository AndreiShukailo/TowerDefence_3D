using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private Enemy _target;

    public void Create(float speed, float damage, Enemy target, Vector3 startPosition)
    {
        _speed = speed;
        _damage = damage;
        _target = target;
        transform.position = startPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            gameObject.SetActive(false);
            enemy.TakeDamage(_damage);
        }
    }

    private void Update()
    {
        if (_target.gameObject.activeSelf)
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
        else
            gameObject.SetActive(false);
    }
}

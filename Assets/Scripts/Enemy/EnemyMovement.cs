using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    private NavMeshAgent _agent;
    private float _extraRotationSpeed = 360f;


    void Update()
    {
        ExtraRotation();
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _agent.updateRotation = false;
    }

    private void OnEnable()
    {
        _animator.SetFloat("Speed", _speed);
    }

    public void SetTarget(Vector3 target, Vector3 position)
    {
        _agent.Warp(position);
        _agent.destination = target;
    }

    public void StopMove()
    {
        _agent.CompleteOffMeshLink();
    }

    private void ExtraRotation()
    {
        Vector3 lookrotation = _agent.steeringTarget - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), _extraRotationSpeed * Time.deltaTime);
    }
}

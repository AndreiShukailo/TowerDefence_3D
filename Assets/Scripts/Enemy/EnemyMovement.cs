using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
    }

    public void SetTarget(Vector3 target, Vector3 position)
    {
        _agent.Warp(position);
        _agent.SetDestination(target);
    }

    public void StopMove()
    {
        _agent.CompleteOffMeshLink();
    }
}

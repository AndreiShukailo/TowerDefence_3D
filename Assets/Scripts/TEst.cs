using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TEst : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Awake()
    {
        Debug.Log(GetComponent<NavMeshAgent>().SetDestination(target.position));
    }

}

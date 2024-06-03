using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _targetTrm;
    [SerializeField] private NavMeshAgent _navAgent;

    private void Update()
    {
        _navAgent.SetDestination(_targetTrm.position);
    }
}

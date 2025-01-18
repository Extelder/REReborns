using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyState
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyPatrolState _patrolState;
    [SerializeField] private float _maxCooldown;
    [SerializeField] private EnemyStateMachine _stateMachine;

    public override void Enter()
    {
        Debug.Log("stoy");
        _agent.isStopped = true;
        CanChangeState = false;
        StartCoroutine(GoToPatrol());
    }

    private IEnumerator GoToPatrol()
    {
        float cooldown;
        if (_patrolState.CurrentPoint.StayRandomTime)
            cooldown = Random.Range(0, _maxCooldown);
        else
            cooldown = _patrolState.CurrentPoint.StayLenght;
        yield return new WaitForSeconds(cooldown);
        CanChangeState = true;
        _stateMachine.Patrol();
    }

    public override void Exit()
    {
        _agent.isStopped = false;
        StopAllCoroutines();
    }
}
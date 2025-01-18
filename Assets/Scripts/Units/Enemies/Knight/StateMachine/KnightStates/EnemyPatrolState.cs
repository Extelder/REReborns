using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyPatrolState : EnemyState
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] List<EnemyPoint> _points;
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyStateMachine _stateMachine;

    public EnemyPoint CurrentPoint { get; private set; }

    public override void Enter()
    {
        PlayPatrolAnim(true);
        Debug.Log("ШКЕБЕДЕ ПАТРУЛЬ");
        CurrentPoint = _points[GetRandomPointID()];
        _agent.SetDestination(CurrentPoint.transform.position);
        StartCoroutine(Patrol());
    }

    public int GetRandomPointID()
    {
        int pointID = Random.Range(0, _points.Count);
        if (_points[pointID] == CurrentPoint)
        {
            return GetRandomPointID();
        }

        return pointID;
    }

    public override void Exit()
    {
        PlayPatrolAnim(false);
        StopAllCoroutines();
    }

    public IEnumerator Patrol()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            if (_agent.remainingDistance <= 1)
            {
                _stateMachine.Idle();
            }
        }
    }

    public void PlayPatrolAnim(bool isWalking)
    {
        _animator.SetBool("IsWalking", isWalking);
    }
}
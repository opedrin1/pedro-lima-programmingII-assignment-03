using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    
    private EnemyState _currentState;
    private Transform _currentTarget;
    private bool _isWaiting = false;

    void Start()
    {
        _currentState = EnemyState.Idle;
    }

    void FixedUpdate()
    {
        if (_currentState == EnemyState.Idle)
        {
            if (!_isWaiting)
            {
                StartCoroutine(WaitAndChooseRandomPointAndMove(3));
            }
        }
        
        else if (_currentState == EnemyState.Patrol)
        {
            if(agent.remainingDistance <= .2f)
            {
                _currentState = EnemyState.Idle;
            }
        }
    }

    private IEnumerator WaitAndChooseRandomPointAndMove(float timeToWait)
    {
        Debug.Log("looking for random point");
        _isWaiting = true;
        yield return new WaitForSeconds (timeToWait);
        _currentState = EnemyState.Patrol;
        ChooseRandomPointAndMove();
        _isWaiting = false;

    }

    private void ChooseRandomPointAndMove()
    {
        if (patrolPoints.Length <= 0) return;
        _currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];

        agent.SetDestination(_currentTarget.position);
    }
}

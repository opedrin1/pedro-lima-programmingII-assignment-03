using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float giveUpDistance;
    [SerializeField] private float chaseCheckAngle;
    
    private EnemyState _currentState;
    private Transform _currentTarget;
    private bool _isWaiting = false;

    void Start()
    {
        _currentState = EnemyState.Idle;
    }

    void FixedUpdate()
    {
        if(_currentState == EnemyState.Idle)
        {
            enemyAnim.SetBool("Idle", true);
            
            if(!_isWaiting)
                StartCoroutine(WaitAndChooseARandomPointAndMove(5));

            //check for the player to chase
            if(IsPlayerInRange() && IsInFOV())
            {
                _currentState = EnemyState.Chase;
                enemyAnim.SetBool("Idle", false);
            }
        }
        else if(_currentState == EnemyState.Patrol)
        {
            enemyAnim.SetBool("Walk", true);
            
            if(agent.remainingDistance <= .2f)
            {
                _currentState = EnemyState.Idle;
                enemyAnim.SetBool("Walk", false);
            }

            // check for the player to chase
            if(IsPlayerInRange() && IsInFOV())
            {
                _currentState = EnemyState.Chase;
                enemyAnim.SetBool("Walk", false);
            }

        }
        else if(_currentState == EnemyState.Chase)
        {
            enemyAnim.SetBool("Chase", true);
            
            agent.SetDestination(playerTransform.position);

            // give up
            if(HasPlayerGoneAwayFromMe())
            {
                _currentState = EnemyState.Idle;
                enemyAnim.SetBool("Chase", false);
            }
        }
    }

    private IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
    {
        _isWaiting = true;
        yield return new WaitForSeconds(timeToWait);
        _currentState = EnemyState.Patrol;
        enemyAnim.SetBool("Idle", false);
        ChooseARandomPointAndMove();
        _isWaiting = false;
    }


    private void ChooseARandomPointAndMove()
    {
        if(patrolPoints.Length <=0) return;
        _currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];

        agent.SetDestination(_currentTarget.position);

    }

    private bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance;
    }

    private bool HasPlayerGoneAwayFromMe()
    {
        return Vector3.Distance(transform.position, playerTransform.position) >= giveUpDistance;
    }

    Vector3 _directionToPlayer;
    private bool IsInFOV()
    {
        _directionToPlayer = (playerTransform.position - transform.position).normalized;
        return Vector3.Angle(transform.forward, _directionToPlayer) <= chaseCheckAngle;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float giveUpDistance;
    [SerializeField] private float chaseCheckAngle;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackCooldown = 1.5f;
    
    protected PlayerHealth _playerHealth;
    protected float _attackTimer;

    protected EnemyState _currentState;
    protected Transform _currentTarget;
    protected bool _isWaiting = false;

    protected virtual void Start()
    {
        _currentState = EnemyState.Idle;
        _playerHealth = playerTransform.GetComponent<PlayerHealth>();
        _attackTimer = attackCooldown;
    }

    void FixedUpdate()
    {
        if(_currentState == EnemyState.Idle)
        {
            enemyAnim.SetBool("Idle", true);

            if(!_isWaiting)
            {
                _isWaiting = true;
                StartCoroutine(WaitAndChooseARandomPointAndMove(5));
            }

            if(IsPlayerInRange() && IsInFOV())
            {
                _currentState = EnemyState.Chase;
                enemyAnim.SetBool("Idle", false);
                agent.isStopped = false;
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

            if(IsPlayerInRange() && IsInFOV())
            {
                _currentState = EnemyState.Chase;
                enemyAnim.SetBool("Walk", false);
                agent.isStopped = false;
            }
        }
        else if(_currentState == EnemyState.Chase)
        {
            enemyAnim.SetBool("Chase", true);
            agent.SetDestination(playerTransform.position);

            if(Vector3.Distance(transform.position, playerTransform.position) <= attackDistance)
            {
                _currentState = EnemyState.Attack;
                enemyAnim.SetBool("Chase", false);
                agent.isStopped = true;
                _attackTimer = attackCooldown;
            }

            if(HasPlayerGoneAwayFromMe())
            {
                _currentState = EnemyState.Idle;
                enemyAnim.SetBool("Chase", false);
            }
        }
        else if(_currentState == EnemyState.Attack)
        {
            enemyAnim.SetBool("Attack", true);

            transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

            _attackTimer -= Time.fixedDeltaTime;
            if(_attackTimer <= 0f)
            {
                DealDamage();
                _attackTimer = attackCooldown;
            }

            if(Vector3.Distance(transform.position, playerTransform.position) > attackDistance)
            {
                _currentState = EnemyState.Chase;
                enemyAnim.SetBool("Attack", false);
                agent.isStopped = false;
            }
        }
    }

    private IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        _currentState = EnemyState.Patrol;
        enemyAnim.SetBool("Idle", false);
        agent.isStopped = false;
        ChooseARandomPointAndMove();
        _isWaiting = false;
    }

    private void ChooseARandomPointAndMove()
    {
        if(patrolPoints.Length <= 0) return;
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

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void DealDamage()
    {
        _playerHealth.TakeDamage(1);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Arrow"))
        {
            Destroy(other.gameObject); // destroy arrow
            Die(); // destroy enemy
        }
    }
}
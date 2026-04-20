using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Settings")]
    [SerializeField] private int hitsToKill = 3;
    
    private int _currentHits = 0;
    protected override void Die()
    {
        _currentHits++;
        if(_currentHits >= hitsToKill)
            Destroy(gameObject);
    }

    protected override void DealDamage()
    {
        _playerHealth.TakeDamage(3);
    }
}
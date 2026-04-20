using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float invincibilityDuration = 1f;
    
    public event Action<int> OnLivesChanged;
    public event Action OnPlayerDeath;
    
    private int _currentLives;
    private bool _isInvincible;
    private float _invincibilityTimer;

    public int CurrentLives => _currentLives;

    void Start()
    {
        _currentLives = maxLives;
    }

    void Update()
    {
        if (_isInvincible)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
                _isInvincible = false;
        }
    }

    public void TakeDamage()
    {
        if (_isInvincible) return;

        _currentLives--;
        OnLivesChanged?.Invoke(_currentLives);

        if (_currentLives <= 0)
            OnPlayerDeath?.Invoke();
        else
        {
            _isInvincible = true;
            _invincibilityTimer = invincibilityDuration;
        }
    }
}

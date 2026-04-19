using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    Vector3 _playerVelocity;
    
    private void Update()
    {
        animator.SetBool("IsGrounded", playerController.IsGrounded());
        
        animator.SetFloat("Velocity", playerController.GetPlayerVelocity().sqrMagnitude);
    }

    private void OnEnable()
    {
        playerController.OnJumpEvent += OnJump;
    }

    private void OnDisable()
    {
        playerController.OnJumpEvent -= OnJump;
    }

    private void OnJump()
    {
        animator.SetTrigger("Jump");
    }
}

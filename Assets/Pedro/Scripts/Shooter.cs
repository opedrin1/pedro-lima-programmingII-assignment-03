using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private InputAction shootInput;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform aimTrack;
    [SerializeField] private GameObject shootObject;

    [SerializeField] private float shootForce;

    private GameObject _arrow;

    private Vector3 _shootDirection;
    private PlayerState _currentState;
    private PlayerController _playerController;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;
        
        _playerController.OnStateUpdated += StateUpdate;
    }

    void StateUpdate(PlayerState state)
    {
        _currentState = state;
    }

    void OnDisable()
    {
        shootInput.performed -= Shoot;
        _playerController.OnStateUpdated -= StateUpdate;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.AIM) return;
        
        // calculate the direction
        _shootDirection = aimTrack.position - shootPoint.position;
        _shootDirection.Normalize();
        
        //create a new arrow
        _arrow = Instantiate(shootObject, shootPoint.position, Quaternion.LookRotation(_shootDirection));

        // apply a force
        _arrow.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);
    }

    
}
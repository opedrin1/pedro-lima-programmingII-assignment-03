using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("No rigidbody attached to Arrow");
        }
        
        Invoke(nameof(DestroyAfter), 5f);
    }
    
    void FixedUpdate()
    {
        _rb.rotation = Quaternion.LookRotation(_rb.linearVelocity);
    }

    void DestroyAfter()
    {
        Destroy(gameObject);
    }
}
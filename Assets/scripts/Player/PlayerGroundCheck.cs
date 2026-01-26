using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    Transform _groundChecker;
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    [SerializeField] LayerMask _groundLayer;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position, 0.45f, _groundLayer);
    }
}

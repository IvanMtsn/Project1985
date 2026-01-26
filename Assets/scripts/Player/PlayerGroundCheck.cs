using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField]Transform _groundChecker;
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    [SerializeField] LayerMask _groundLayer;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, 0.45f, _groundLayer);
    }
}

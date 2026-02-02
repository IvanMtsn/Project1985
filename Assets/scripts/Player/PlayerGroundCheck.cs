using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    [SerializeField] LayerMask _groundLayer;
    List<Collider> _colliders = new List<Collider>();
    PlayerMovementV2 _pMov;
    void Start()
    {
        _pMov = GetComponentInParent<PlayerMovementV2>();
    }
    void FixedUpdate()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(CollisionUtils.IsLayerInMask(other.gameObject.layer, _groundLayer) && !_colliders.Contains(other))
        {
            _colliders.Add(other);
            _isGrounded = true;
            SnapToGround();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (CollisionUtils.IsLayerInMask(other.gameObject.layer, _groundLayer) && _colliders.Contains(other))
        {
            _colliders.Remove(other);
            _isGrounded = _colliders.Count > 0;
        }
    }
    void SnapToGround()
    {
        if(Physics.Raycast(transform.position, Vector3.down,out RaycastHit hit,0.3f))
        {
            _pMov.Rb.position = hit.point + new Vector3(0,1,0);
        }
    }
}
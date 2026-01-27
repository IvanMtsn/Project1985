using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [SerializeField]Transform _groundChecker;
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    [SerializeField] LayerMask _groundLayer;
    List<Collider> _colliders = new List<Collider>();
    void Start()
    {
        
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
}

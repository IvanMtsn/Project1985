using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class PlayerGroundCheck : MonoBehaviour
{
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;
    public LayerMask GroundLayer;
    List<Collider> _colliders = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if(CollisionUtils.IsLayerInMask(other.gameObject.layer, GroundLayer) && !_colliders.Contains(other))
        {
            _colliders.Add(other);
            _isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (CollisionUtils.IsLayerInMask(other.gameObject.layer, GroundLayer) && _colliders.Contains(other))
        {
            _colliders.Remove(other);
            _isGrounded = _colliders.Count > 0;
        }
    }
}
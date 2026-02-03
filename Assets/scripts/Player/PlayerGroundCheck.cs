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
    PlayerMovementV2 _pMov;
    void Start()
    {
        _pMov = GetComponentInParent<PlayerMovementV2>();
    }
    void Update()
    {
        //if (IsGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.25f, GroundLayer, QueryTriggerInteraction.Ignore))
        //{
        //    Vector3 targetPos = new Vector3(_pMov.transform.position.x, _pMov.transform.position.y - hit.distance, _pMov.transform.position.z);
        //    _pMov.transform.position = Vector3.MoveTowards(_pMov.transform.position, targetPos, hit.distance * Time.deltaTime * 10);
        //    if (_pMov.Rb.linearVelocity.y > 0f)
        //    {
        //        _pMov.Rb.linearVelocity = new Vector3(_pMov.Rb.linearVelocity.x, 0f, _pMov.Rb.linearVelocity.z);
        //    }
        //}
    }
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
    //public Vector3 AlignToSurface(Vector3 movementDir)
    //{
    //    if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 0.25f, GroundLayer))
    //    {
    //        Vector3 slopeDir = Vector3.ProjectOnPlane(movementDir, hit.normal).normalized;
    //        return slopeDir * movementDir.magnitude;
    //    }
    //    return movementDir;
    //}
}
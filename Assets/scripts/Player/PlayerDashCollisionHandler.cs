using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashCollisionHandler : MonoBehaviour
{
    PlayerMovement _player;
    [SerializeField] LayerMask _pushableObjectsMask;
    [SerializeField] LayerMask _projectileMask;
    float _pushForce = 2f;
    List<GameObject> _contactedObjects;
    void Start()
    {
        _player = GetComponent<PlayerMovement>();
        _contactedObjects = new List<GameObject>();
        _player.OnDashEnd += ClearContactedObjects;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_player.IsDashing) { return; }
        if (CollisionUtils.IsLayerInMask(collision.gameObject.layer, _pushableObjectsMask) && !_contactedObjects.Contains(collision.gameObject))
        {
            _contactedObjects.Add(collision.gameObject);
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
            rb.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
        }
    }
    void ClearContactedObjects() => _contactedObjects.Clear();
    private void OnDisable()
    {
        _player.OnDashEnd -= ClearContactedObjects;
    }
}

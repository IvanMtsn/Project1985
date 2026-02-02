using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashCollisionHandler : MonoBehaviour
{
    PlayerMovementV2 _player;
    [SerializeField] LayerMask _pushableObjectsMask;
    List<GameObject> _contactedObjects;
    void Start()
    {
        _player = GetComponent<PlayerMovementV2>();
        _contactedObjects = new List<GameObject>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_player.IsDashing) { return; }
        if (CollisionUtils.IsLayerInMask(collision.gameObject.layer, _pushableObjectsMask) && !_contactedObjects.Contains(collision.gameObject))
        {
            _contactedObjects.Add(collision.gameObject);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().HeavyHit(collision.transform.position-transform.position, true);
                _player.EndDash();
            }
        }
    }
    void ClearContactedObjects() => _contactedObjects.Clear();
}

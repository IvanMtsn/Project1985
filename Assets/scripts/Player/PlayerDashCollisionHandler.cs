using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashCollisionHandler : MonoBehaviour
{
    PlayerMovementV2 _pMov;
    [SerializeField] LayerMask _pushableObjectsMask;
    List<GameObject> _contactedObjects;
    [SerializeField] AudioClip _stunClip;
    [SerializeField] AudioClip _dashStopClip;
    [SerializeField] AudioClip _dashCollideClip;
    void Start()
    {
        _pMov = GetComponent<PlayerMovementV2>();
        _contactedObjects = new List<GameObject>();
    }
    private void Update()
    {
        if (_contactedObjects.Count > 0) { _contactedObjects.Clear(); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_pMov.IsDashing)
        {
            return;
        }
        if (CollisionUtils.IsLayerInMask(collision.gameObject.layer, _pushableObjectsMask) && !_contactedObjects.Contains(collision.gameObject))
        {
            _contactedObjects.Add(collision.gameObject);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().HeavyHit(collision.transform.position-transform.position, true);
                _pMov.EndDash();
                if (_pMov.GroundCheck.IsGrounded) { _pMov.Rb.AddForce((transform.position - collision.transform.position) * 75, ForceMode.Impulse); }
                _pMov.LastTimeSinceDash += 0.5f;
                AudioManager.Instance.PlaySound(_dashCollideClip, 1.4f);
                AudioManager.Instance.PlaySound(_dashStopClip, 0.8f);
            }
        }
    }
}

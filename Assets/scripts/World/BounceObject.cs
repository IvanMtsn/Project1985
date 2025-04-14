using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    float _pushStrength = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
            pushDirection.y = 0;
            rb.AddForce(pushDirection * _pushStrength, ForceMode.Impulse);
        }
    }
}

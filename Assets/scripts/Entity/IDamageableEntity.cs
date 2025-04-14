using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageableEntity
{
    void TakeDamage(float damage);
    void Die();
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, IDamageableEntity
{
    public float MaxHealth { get; } = 100;
    public float Health { get; private set; }
    public event Action<float> onDamageTaken;
    float _timerUntilHeal = 5;
    float _currentHealTimer;
    bool _isHealing = false;
    void Start()
    {
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if((_currentHealTimer < _timerUntilHeal) && (Health < MaxHealth) && !_isHealing)
        {
            _currentHealTimer += Time.deltaTime;
            if(_currentHealTimer >= _timerUntilHeal)
            {
                StartCoroutine(Healing());
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (GetComponent<PlayerMovement>().IsInvincible) { return; }

        onDamageTaken?.Invoke(damage);
        Health -=damage;
        _currentHealTimer = 0;
        if (Health <= 0)
        {
            Die();
        }
    }
    public void Heal(float val)
    {
        float valueToHeal;
        if(Health + val > MaxHealth)
        {
            valueToHeal = MaxHealth - Health;
        }
        else { valueToHeal = val; }
        Health += valueToHeal;
    }
    IEnumerator Healing()
    {
        _isHealing = true;
        while(Health < MaxHealth)
        {
            Heal(7);
            yield return new WaitForSeconds(0.15f);
        }
        _isHealing = false;
    }
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

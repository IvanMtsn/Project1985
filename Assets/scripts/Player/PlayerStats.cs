using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, IDamageableEntity
{
    public float MaxHealth { get; } = 100;
    public float Health { get; private set; }
    float _timerUntilHeal = 5;
    float _currentHealTimer;
    bool _isHealing = false;
    bool _isInvincible = false;
    float _hitInvincibleTimer = 0.3f; 
    [SerializeField] CameraController _cameraController;
    Coroutine _healingCoroutine;
    void Start()
    {
        Health = MaxHealth;
    }
    void Update()
    {
        if((_currentHealTimer < _timerUntilHeal) && (Health < MaxHealth) && !_isHealing)
        {
            _currentHealTimer += Time.deltaTime;
            if(_currentHealTimer >= _timerUntilHeal)
            {
                _healingCoroutine = StartCoroutine(Healing());
                _currentHealTimer = 0;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (_isInvincible) { return; }
        if (_isHealing)
        {
            StopCoroutine(_healingCoroutine);
            _isHealing = false;
        }
        Invicibility(_hitInvincibleTimer);
        _cameraController.ShakeCamera(damage/3);
        Health -= damage;
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
    public void Invicibility(float time)
    {
        if(_isInvincible) { return; }
        StartCoroutine(InvicibilityCoroutine(time));
    }
    IEnumerator InvicibilityCoroutine(float time)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(time);
        _isInvincible = false;
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

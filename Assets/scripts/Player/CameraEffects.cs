using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    PlayerStats _player;
    // Start is called before the first frame update
    void Awake()
    {

    }
    void Update()
    {
       
    }
    public void ShakeCamera(float shakeStrength)
    {
        StartCoroutine(Shake(shakeStrength));
    }
    private IEnumerator Shake(float shakeStrength)
    {
        float timeElapsed = 0;
        Vector3 originalRotation = transform.localEulerAngles;

        while (timeElapsed < 0.2f)
        {
            float randomX = (Random.value - 0.5f) * shakeStrength;
            float randomY = (Random.value - 0.5f) * shakeStrength;
            float randomZ = (Random.value - 0.5f) * shakeStrength;

            transform.localEulerAngles = originalRotation + new Vector3(randomX, randomY, randomZ);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset to original rotation after shake ends
        transform.localEulerAngles = originalRotation;
    }
    private void OnEnable()
    {
        _player = FindObjectOfType<PlayerStats>();
        _player.onDamageTaken += ShakeCamera;
    }
    private void OnDisable()
    {
        _player.onDamageTaken -= ShakeCamera;
    }
}

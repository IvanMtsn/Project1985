using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 _lookInput;
    float _rotationY = 0;
    float _rotationX = 0;
    float _mouseSensitivity = 0.25f;
    [SerializeField] Transform _camPos;
    [SerializeField] Transform _player;
    Vector3 _originalPos;
    float _camShake = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = _camPos.position;
        _originalPos = transform.localPosition;

    }
    void Update()
    {
        _lookInput = InputManager.Instance.Look;

        _rotationY += -_lookInput.y * _mouseSensitivity;
        _rotationX += _lookInput.x * _mouseSensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -89, 89);
        if (_camShake > 0.1f)
        {
            float randomX = (Random.value - 0.5f) * _camShake/5;
            float randomY = (Random.value - 0.5f) * _camShake/5;
            float randomZ = (Random.value - 0.5f) * _camShake/5;

            transform.localPosition = _originalPos + new Vector3(randomX, randomY, randomZ);
            _camShake -= Time.deltaTime * 12;
        }
        else
        {
            transform.localPosition = _originalPos;
        }
    }
    private void LateUpdate()
    {
        _player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, _rotationX, 0));
        Quaternion targetRot = Quaternion.Euler(_rotationY, _rotationX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.5f);
    }
    public void ShakeCamera(float shakeStrength)
    {
       if(_camShake < shakeStrength)
        {
            _camShake = shakeStrength;
        }
    }
}

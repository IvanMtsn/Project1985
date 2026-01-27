using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 _lookInput;
    float _rotationY = 0;
    float _rotationX = 0;
    float _mouseSensitivity = 0.25f;
    [SerializeField] Transform _camPos;
    [SerializeField] Transform _player;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.position = _camPos.position;
    }
    void Update()
    {
        _lookInput = InputManager.Instance.Look;

        _rotationY += -_lookInput.y * _mouseSensitivity;
        _rotationX += _lookInput.x * _mouseSensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -89, 89);
    }
    private void LateUpdate()
    {
        _player.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(0, _rotationX, 0));
        Quaternion targetRot = Quaternion.Euler(_rotationY, _rotationX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.5f);
    }
}

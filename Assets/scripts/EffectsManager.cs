using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    Transform _playerCam;
    float _slowDownDuration = 0.4f;
    bool _isTimeSlowedDown = false;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if(UnityEngine.GameObject.Find("Main Camera") != null)
        {
            _playerCam = UnityEngine.GameObject.Find("Main Camera").transform;
        }
    }
    void Update()
    {
        if(_playerCam == null) { FindPlayerCam();}
    }
    public void SlowDownTime()
    {
        if(!_isTimeSlowedDown)
        {
            StartCoroutine(SlowDownTimeCoroutine());
        }
    }
    IEnumerator SlowDownTimeCoroutine()
    {
        _isTimeSlowedDown = true;
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(_slowDownDuration);
        Time.timeScale = 1;
        _isTimeSlowedDown = false;
    }
    void FindPlayerCam()
    {
        if (UnityEngine.GameObject.Find("Main Camera") != null)
        {
            _playerCam = UnityEngine.GameObject.Find("Main Camera").transform;
        }
    }
}

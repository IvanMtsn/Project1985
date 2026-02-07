using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeManager : MonoBehaviour
{
    //Yes this may be a god object
    //no i dont care
    void Start()
    {
        Application.targetFrameRate = 120;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            UnityEngine.GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementV2>().EndDash();
        }
    }
}

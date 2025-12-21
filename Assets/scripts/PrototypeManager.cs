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

    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

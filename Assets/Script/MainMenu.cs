using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameManager");
        Destroy(gameController);
    }
    public void OpenCreditScene()
    {
        SceneManager.LoadScene("Credit");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SceneManager.LoadScene("AtelierScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool gameIsPaused = false;
    public GameObject pauseMenu;

    public static PauseManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void MainMenu()
    {
        if (GameOverManager.instance.CheckVictoryCondition())
        {
            StateVariableController.score += 20;
            PlayerPrefs.SetInt("score", StateVariableController.score);
            PlayerPrefs.Save();
        }
        else
        {
            StateVariableController.score -= 20;
            PlayerPrefs.SetInt("score", StateVariableController.score);
            PlayerPrefs.Save();

        }

        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadWorkShop()
    {

        StateVariableController.score = GameOverManager.instance.score;
        PlayerPrefs.SetInt("coin", GameOverManager.instance.score);
        PlayerPrefs.Save();
        ResumeGame();
        SceneManager.LoadScene("AtelierScene");
    }

    public void PauseGame()
    {

        gameIsPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

    }
}

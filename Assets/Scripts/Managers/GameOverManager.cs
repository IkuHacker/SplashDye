using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameOverManager : MonoBehaviour
{
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public Text coinWinText;
    public Text coinLooseText;
    public int score;

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la sc�ne");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        score = PlayerPrefs.GetInt("score", 200);
       
    }

   


    
    public void GameOver() 
    {
        score -= 35;
        coinLooseText.text = "-35";
        defeatPanel.SetActive(true);
        victoryPanel.SetActive(false);
    }
    public void LoadWorkShop()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
        SceneManager.LoadScene("AtelierScene");
    }

    public void LoadMainMenu()
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

   

}

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
    public EnemySpawn enemySpawn;
    public int score;

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        score = PlayerPrefs.GetInt("score", 200);
       
    }

   

    public void GameFinish()
    {
        PlayerMovement.instance.MovePlayer(0f, 0f);
        PlayerMovement.instance.enabled = false;

        if (CheckVictoryCondition())
        {

            score += 50;
            coinWinText.text = "+50";
            victoryPanel.SetActive(true);
            defeatPanel.SetActive(false);

        }
        else
        {
            score -= 0;
            coinLooseText.text = "+0";
            defeatPanel.SetActive(true);
            victoryPanel.SetActive(false);
        }
    }

    
    public void GameOver() 
    {
        PlayerMovement.instance.MovePlayer(0f, 0f);
        PlayerMovement.instance.enabled = false;
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

    public  bool CheckVictoryCondition()
    {
        Inventory inventory = Inventory.instance;

        foreach (ItemData requiredItem in enemySpawn.requiredItems)
        {
            ItemInInventory itemInInventory = inventory.content.Find(item => item.itemData == requiredItem);
            if (itemInInventory == null || itemInInventory.count < 1)
            {
                return false;
            }
        }
        return true;
    }



}

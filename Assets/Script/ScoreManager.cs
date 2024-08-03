using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public int score;

    private void Start()
    {
        StateVariableController.score = PlayerPrefs.GetInt("score", 0);
    }
    private void Update()
    {

        scoreText.text = StateVariableController.score.ToString();
    }
}

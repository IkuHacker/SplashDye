using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public float duration;
    public Text timerText;
    public static Timer instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Timer dans la scene");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        StartTimer();
    }
    public void StartTimer()
    {
        duration = StateVariableController.duration;
        UpdateTimerDisplay();
        InvokeRepeating(nameof(UpdateTimer), 1f, 1f);
    }

    private void UpdateTimer()
    {
        duration -= 1f;
        if (duration <= 0f)
        {
            duration = 0f;
            CancelInvoke(nameof(UpdateTimer));
            GameOverManager.instance.GameFinish();
        }
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(duration / 60f);
        int seconds = Mathf.FloorToInt(duration % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void AddTimer(float durationAdd) 
    {
        duration += durationAdd;
    }
}

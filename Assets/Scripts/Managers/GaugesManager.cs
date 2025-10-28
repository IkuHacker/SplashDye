using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;


public class GaugesManager : MonoBehaviour
{
    public int Score = 0;
    private float Timer;
    [Header("Références")]
    [SerializeField] public Slider cultSlider;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] public Slider foodSlider;
    [SerializeField] public Slider determinationSlider;
    [SerializeField] private RectTransform DeterminationAnimation;
    [SerializeField] private RectTransform CultAnimation;
    [SerializeField] private RectTransform FoodAnimation;
    [SerializeField] private Image GameOverBackground;
    [SerializeField] private Image GameOver1;
    [SerializeField] private Image GameOver2;
    [SerializeField] private Image GameOver3;
    [SerializeField] private Image GameOver4;
    private bool gameIsOver = false;

    [SerializeField] private Image ButtonRagequit;
    [SerializeField] private Image ButtonReplay;

    [SerializeField] private GameObject TextRagequit;
    [SerializeField] private GameObject TextReplay;
    public float Speed = 2.5f;
    private bool Active = false;

    [Header("PNJ")]
    private Dictionary<string, int> jobCounts = new Dictionary<string, int>();
    public List<GameObject> pnjList = new List<GameObject>();
    private List<GameObject> hungryPNJ = new List<GameObject>();

    [Header("Vitesse de descente (par seconde)")]
    [SerializeField] private float baseFoodDecayRate = 0.5f; // pts/s par PNJ
    [SerializeField] private float energyDecayRate = 0.02f;
    [SerializeField] private float determinationDecayRate = 0.005f;
    [SerializeField] private float maxLossRate = 0.5f; // limite descente max

    [Header("Métiers - Réglages")]
    [SerializeField] private float cultGaugeSpeed = 0.02f;
    [SerializeField] private float determinationGaugeSpeed = 0.02f;

    private void Start()
    {
        TextRagequit.SetActive(false);
        TextReplay.SetActive(false);
        RefreshPNJList();

        SetAlphaSafe(GameOverBackground, 0f);
        SetAlphaSafe(GameOver1, 0f);
        SetAlphaSafe(GameOver2, 0f);
        SetAlphaSafe(GameOver3, 0f);
        SetAlphaSafe(GameOver4, 0f);
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= 1f)
        {
            Score += 1000;
            Timer = 0;
            scoreText.text = $"Score: {Score}";
        }
        RefreshPNJList();
        float delta = Time.deltaTime;

        UpdateSliderAnimations();
        UpdatePNJHunger();

        UpdateFood(delta);
        UpdateGaugeByPercentage(cultSlider, "Cult", delta, cultGaugeSpeed);
        UpdateGaugeByPercentage(determinationSlider, "Determination", delta, determinationGaugeSpeed);

        ClampSliders();
        CheckGameOver(delta);
    }

    // ──────────────────────────────────────────────
    // PNJ MANAGEMENT
    // ──────────────────────────────────────────────
    private void RefreshPNJList()
    {
        pnjList.RemoveAll(p => p == null);
        jobCounts.Clear();

        foreach (var pnj in pnjList)
        {
            if (pnj == null) continue;
            PNJMovements mov = pnj.GetComponent<PNJMovements>();
            string job = mov != null ? mov.Job : "None";

            if (!jobCounts.ContainsKey(job)) jobCounts[job] = 0;
            jobCounts[job]++;
        }
    }

    private int GetJobCount(string job)
    {
        return jobCounts.ContainsKey(job) ? jobCounts[job] : 0;
    }

    private float GetJobPercentage(string job)
    {
        if (pnjList.Count == 0) return 0f;
        return (float)GetJobCount(job) / pnjList.Count;
    }

    public int GetTotalPNJs()
    {
        return pnjList.Count;
    }

    // ──────────────────────────────────────────────
    // JAUJES DYNAMIQUES
    // ──────────────────────────────────────────────
    private void UpdateFood(float delta)
    {
        float decay = baseFoodDecayRate * pnjList.Count;
        if (decay > maxLossRate) decay = maxLossRate;
        foodSlider.value -= decay * delta;
    }

    private void UpdateGaugeByPercentage(Slider slider, string job, float delta, float gaugeSpeed)
    {
        float pct = GetJobPercentage(job);

        // change = 0 si 50%, positif si >50%, négatif si <50%
        float change = (pct - 0.5f) * 2f * gaugeSpeed;

        // limiter la descente
        if (change < -maxLossRate) change = -maxLossRate;

        slider.value += change * delta;
    }

    private void ClampSliders()
    {
        foodSlider.value = Mathf.Clamp01(foodSlider.value);
        cultSlider.value = Mathf.Clamp01(cultSlider.value);
        determinationSlider.value = Mathf.Clamp01(determinationSlider.value);
    }

    private void UpdateSliderAnimations()
    {
        if (DeterminationAnimation)
            DeterminationAnimation.anchoredPosition = new Vector3(DeterminationAnimation.anchoredPosition.x, 337 * determinationSlider.value - 1289, 0);
        if (CultAnimation)
            CultAnimation.anchoredPosition = new Vector3(CultAnimation.anchoredPosition.x, 337 * cultSlider.value - 1289, 0);
        if (FoodAnimation)
            FoodAnimation.anchoredPosition = new Vector3(FoodAnimation.anchoredPosition.x, 337 * foodSlider.value - 1289, 0);
    }

    // ──────────────────────────────────────────────
    // FAIM
    // ──────────────────────────────────────────────
    private void UpdatePNJHunger()
    {
        if (pnjList.Count == 0) return;

        hungryPNJ.RemoveAll(p => p == null);

        float targetHungry = pnjList.Count * (1f - foodSlider.value);

        if (hungryPNJ.Count >= targetHungry) return;

        int toAdd = Mathf.RoundToInt(targetHungry - hungryPNJ.Count);

        var candidates = pnjList.Except(hungryPNJ)
                                .OrderBy(x => Random.value)
                                .Take(toAdd);

        foreach (var pnj in candidates)
        {
            var hunger = pnj.GetComponent<PNJHunger>();
            if (hunger != null)
            {
                hunger.ActiveHunger();
                hungryPNJ.Add(pnj);
            }
        }
    }

    public void ModifyFood(float amount)
    {
        foodSlider.value = Mathf.Clamp01(foodSlider.value + amount);
    }

    public void RemoveFromHungryList(GameObject pnj)
    {
        hungryPNJ.Remove(pnj);
    }

    // ──────────────────────────────────────────────
    // GAME OVER
    // ──────────────────────────────────────────────
    private void CheckGameOver(float delta)
    {
        if (gameIsOver) return;

        if (GetTotalPNJs() == 0 && Active ||
            foodSlider.value <= 0f ||
            determinationSlider.value <= 0f ||
            cultSlider.value <= 0f)
        {
            gameIsOver = true;
            Time.timeScale = 0f; // ✅ ON BLOQUE ICI UNE SEULE FOIS
            TextRagequit.SetActive(true);
            TextReplay.SetActive(true);
        }

        if (gameIsOver)
        {
            FadeInImage(GameOverBackground, delta);
            FadeInImage(ButtonRagequit, delta);
            FadeInImage(ButtonReplay, delta);
            FadeInImage(GameOver1, delta);
            FadeInImage(GameOver2, delta);
            FadeInImage(GameOver3, delta);
            FadeInImage(GameOver4, delta);
        }
    }

    private void FadeInImage(Image img, float delta)
    {
        if (!img) return;

        Color c = img.color;

        // Ajout de l'opacity
        c.a += Speed * delta;
        c.a = Mathf.Clamp01(c.a + Speed * Time.unscaledDeltaTime);
        img.color = c;

        // Si l'opacité est arrivée à 1 -> on freeze le jeu
        if (c.a >= 1f)
        {
            TextRagequit.SetActive(true);
            TextReplay.SetActive(true);
        }
    }


    private void SetAlphaSafe(Image img, float alpha)
    {
        if (!img) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    public void Ragequit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");

    }
    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TestScene");
    }
}

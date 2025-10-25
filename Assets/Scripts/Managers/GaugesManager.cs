using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GaugesManager : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Slider cultSlider;
    [SerializeField] private Slider foodSlider;
    [SerializeField] private Slider determinationSlider;
    [SerializeField] private RectTransform DeterminationAnimation;
    [SerializeField] private RectTransform CultAnimation;
    [SerializeField] private RectTransform FoodAnimation;
    [SerializeField] private Image GameOverBackground;
    [SerializeField] private Image GameOver1;
    [SerializeField] private Image GameOver2;
    [SerializeField] private Image GameOver3;
    [SerializeField] private Image GameOver4;
    public float Speed = 2.5f;

    [Header("PNJ")]
    private PNJMovements[] allPNJs;
    private Dictionary<string, int> jobCounts = new Dictionary<string, int>();
    [SerializeField] public List<GameObject> pnjList = new List<GameObject>();
    private List<GameObject> hungryPNJ = new List<GameObject>();

    [Header("Vitesse de descente (par seconde)")]
    [SerializeField] private float baseFoodDecayRate = 0.005f;
    [SerializeField] private float energyDecayRate = 0.02f;
    [SerializeField] private float determinationDecayRate = 0.005f;

    [Header("Multiplicateurs")]
    [SerializeField] private float noFoodEnergyMultiplier = 2f;
    [SerializeField] private float noFoodDeterminationMultiplier = 1.3f;
    [SerializeField] private float noEnergyDeterminationMultiplier = 1.5f;

    private void Start()
    {
        allPNJs = FindObjectsOfType<PNJMovements>();
        UpdateJobCounts();
        // Optionnel : initialiser les alphas des GameOver à 0 si besoin
        SetAlphaSafe(GameOverBackground, 0f);
        SetAlphaSafe(GameOver1, 0f);
        SetAlphaSafe(GameOver2, 0f);
        SetAlphaSafe(GameOver3, 0f);
        SetAlphaSafe(GameOver4, 0f);
    }

    private void Update()
    {
        UpdateJobCounts(); // met à jour le comptage en live (peut être optimisé si nécessaire)

        float delta = Time.deltaTime;

        if (DeterminationAnimation != null)
            DeterminationAnimation.anchoredPosition = new Vector3(DeterminationAnimation.anchoredPosition.x, 337 * determinationSlider.value - 1289, 0);
        if (CultAnimation != null)
            CultAnimation.anchoredPosition = new Vector3(CultAnimation.anchoredPosition.x, 337 * cultSlider.value - 1289, 0);
        if (FoodAnimation != null)
            FoodAnimation.anchoredPosition = new Vector3(FoodAnimation.anchoredPosition.x, 337 * foodSlider.value - 1289, 0);

        UpdatePNJHunger();

        float hungryPercent = (pnjList.Count > 0) ? (float)hungryPNJ.Count / pnjList.Count : 0f;

        float currentFoodDecay = baseFoodDecayRate * (0.3f + hungryPercent * 2f);
        if (foodSlider != null) foodSlider.value -= currentFoodDecay * delta;

        float currentEnergyDecay = energyDecayRate;
        if (foodSlider != null && foodSlider.value <= 0.5f) currentEnergyDecay *= noFoodEnergyMultiplier;
        if (cultSlider != null) cultSlider.value -= currentEnergyDecay * delta;

        float currentDeterminationDecay = determinationDecayRate;
        if (foodSlider != null && foodSlider.value <= 0.2f) currentDeterminationDecay *= noFoodDeterminationMultiplier;
        if (cultSlider != null && cultSlider.value <= 0.2f) currentDeterminationDecay *= noEnergyDeterminationMultiplier;
        if (determinationSlider != null) determinationSlider.value -= currentDeterminationDecay * delta;

        // Clamp safely
        if (foodSlider != null) foodSlider.value = Mathf.Clamp01(foodSlider.value);
        if (cultSlider != null) cultSlider.value = Mathf.Clamp01(cultSlider.value);
        if (determinationSlider != null) determinationSlider.value = Mathf.Clamp01(determinationSlider.value);

        if (Time.frameCount % 60 == 0)
            Debug.Log($"🧪 Métiers: {GetTotalPNJs()} PNJs | Jobs = {string.Join(", ", jobCounts.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");

        // --- Game Over checks : fade les images correspondantes + background ---
        // Cult (GameOver4)
        if (cultSlider != null && cultSlider.value <= 0f)
        {
            FadeInImage(GameOverBackground, delta);
            FadeInImage(GameOver4, delta);
        }
        // Food (GameOver2)
        if (foodSlider != null && foodSlider.value <= 0f)
        {
            FadeInImage(GameOverBackground, delta);
            FadeInImage(GameOver2, delta);
        }
        // Determination (GameOver3)
        if (determinationSlider != null && determinationSlider.value <= 0f)
        {
            FadeInImage(GameOverBackground, delta);
            FadeInImage(GameOver3, delta);
        }
        // No PNJ (GameOver1)
        if (GetTotalPNJs() == 0)
        {
            FadeInImage(GameOverBackground, delta);
            FadeInImage(GameOver1, delta);
        }
    }

    private void UpdateJobCounts()
    {
        // refresh list of PNJs (si tu veux l'optimiser, gère via un manager de spawn)
        allPNJs = FindObjectsOfType<PNJMovements>();

        jobCounts.Clear();
        foreach (var pnj in allPNJs)
        {
            if (pnj == null) continue;
            string job = pnj.Job ?? "None";
            if (!jobCounts.ContainsKey(job)) jobCounts[job] = 0;
            jobCounts[job]++;
        }
    }

    private void FadeInImage(Image img, float delta)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = Mathf.Clamp01(c.a + Speed * delta);
        img.color = c;
    }

    // sécurité pour initialiser alpha
    private void SetAlphaSafe(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = Mathf.Clamp01(alpha);
        img.color = c;
    }

    public int GetTotalPNJs()
    {
        return jobCounts.Values.Sum();
    }

    void UpdatePNJHunger()
    {
        if (pnjList == null || pnjList.Count == 0)
            return;

        pnjList.RemoveAll(p => p == null);
        hungryPNJ.RemoveAll(p => p == null);

        float foodPercent = (foodSlider != null) ? foodSlider.value : 1f;
        int targetHungryCount = Mathf.RoundToInt(pnjList.Count * (1f - foodPercent));

        if (hungryPNJ.Count >= targetHungryCount)
            return;

        int toAdd = targetHungryCount - hungryPNJ.Count;
        var candidates = pnjList.Except(hungryPNJ)
                                .OrderBy(x => Random.value)
                                .Take(toAdd);

        foreach (var pnj in candidates)
        {
            PNJHunger hunger = pnj.GetComponent<PNJHunger>();
            if (hunger != null)
            {
                hunger.ActiveHunger();
                hungryPNJ.Add(pnj);
            }
        }
    }

    public void ModifyFood(float amount)
    {
        if (foodSlider == null) return;
        foodSlider.value = Mathf.Clamp01(foodSlider.value + amount);
    }

    public void RemoveFromHungryList(GameObject pnj)
    {
        if (hungryPNJ.Contains(pnj))
            hungryPNJ.Remove(pnj);
    }
}

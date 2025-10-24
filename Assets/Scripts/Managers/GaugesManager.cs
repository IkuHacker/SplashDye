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

    [Header("PNJ")]
    [SerializeField] public List<GameObject> pnjList = new List<GameObject>();
    private List<GameObject> hungryPNJ = new List<GameObject>(); // PNJ déjà affamés

    [Header("Vitesse de descente (par seconde)")]
    [SerializeField] private float baseFoodDecayRate = 0.005f;
    [SerializeField] private float energyDecayRate = 0.02f;
    [SerializeField] private float determinationDecayRate = 0.005f;

    [Header("Multiplicateurs")]
    [SerializeField] private float noFoodEnergyMultiplier = 2f;
    [SerializeField] private float noFoodDeterminationMultiplier = 1.3f;
    [SerializeField] private float noEnergyDeterminationMultiplier = 1.5f;

    void Update()
    {

        float delta = Time.deltaTime;

        DeterminationAnimation.anchoredPosition = new Vector3(DeterminationAnimation.anchoredPosition.x, 337 * determinationSlider.value -1289, 0);
        CultAnimation.anchoredPosition = new Vector3(CultAnimation.anchoredPosition.x, 337 * cultSlider.value -1289, 0);
        FoodAnimation.anchoredPosition = new Vector3(FoodAnimation.anchoredPosition.x, 337 * foodSlider.value -1289, 0);

        // Mise à jour dynamique des PNJ affamés
        UpdatePNJHunger();

        // --- Calcul du pourcentage de PNJ affamés ---
        float hungryPercent = (pnjList.Count > 0) ? (float)hungryPNJ.Count / pnjList.Count : 0f;

        // --- Descente de la jauge de nourriture en fonction du pourcentage de faim ---
        float currentFoodDecay = baseFoodDecayRate * (0.3f + hungryPercent * 2f);
        foodSlider.value -= currentFoodDecay * delta;

        // --- Énergie ---
        float currentEnergyDecay = energyDecayRate;
        if (foodSlider.value <= 0.5f)
            currentEnergyDecay *= noFoodEnergyMultiplier;
        cultSlider.value -= currentEnergyDecay * delta;

        // --- Détermination ---
        float currentDeterminationDecay = determinationDecayRate;
        if (foodSlider.value <= 0.2f)
            currentDeterminationDecay *= noFoodDeterminationMultiplier;
        if (cultSlider.value <= 0.2f)
            currentDeterminationDecay *= noEnergyDeterminationMultiplier;
        determinationSlider.value -= currentDeterminationDecay * delta;

        // --- Clamp ---
        foodSlider.value = Mathf.Clamp01(foodSlider.value);
        cultSlider.value = Mathf.Clamp01(cultSlider.value);
        determinationSlider.value = Mathf.Clamp01(determinationSlider.value);

        // Debug global
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"🍖 Nourriture: {foodSlider.value * 100:F0}% | PNJ affamés: {hungryPNJ.Count}/{pnjList.Count}");
        }

        // --- Game Over ---
        if (cultSlider.value <= 0 && foodSlider.value <= 0 && determinationSlider.value <= 0)
        {
            Debug.Log("🔥 GAME OVER : Invocation de Baudan le Sâton, malédiction éternelle !");
        }
    }

    void UpdatePNJHunger()
    {
        if (pnjList == null || pnjList.Count == 0)
            return;

        // Nettoyer les listes
        pnjList.RemoveAll(p => p == null);
        hungryPNJ.RemoveAll(p => p == null);

        float foodPercent = foodSlider.value;
        int targetHungryCount = Mathf.RoundToInt(pnjList.Count * (1f - foodPercent));

        // Si on a déjà plus ou autant de PNJ affamés, on ne touche pas
        if (hungryPNJ.Count >= targetHungryCount)
            return;

        // Ajouter des PNJ à la faim
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
                Debug.Log($"🍗 Nouveau PNJ affamé : {pnj.name}");
            }
        }
    }

    public void ModifyFood(float amount)
    {
        foodSlider.value = Mathf.Clamp01(foodSlider.value + amount);
        Debug.Log($"🍔 Nourriture +{amount * 100:F0}% → {foodSlider.value * 100:F0}%");
    }

    public void RemoveFromHungryList(GameObject pnj)
    {
        if (hungryPNJ.Contains(pnj))
        {
            hungryPNJ.Remove(pnj);
            Debug.Log($"😋 {pnj.name} retiré de la liste des affamés !");
        }
    }
}

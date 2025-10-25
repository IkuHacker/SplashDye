using UnityEngine;
using System.Collections;

public class PNJHunger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hungerIcon;
    [SerializeField] public SpriteRenderer CultIcon;
    [SerializeField] public SpriteRenderer DeterminationIcon;
    private bool isHunger;
    private bool hungerBlocked;

    private GaugesManager gaugesManager;

    void Start()
    {
        gaugesManager = FindFirstObjectByType<GaugesManager>();
    }

    public void ActiveHunger()
    {
        if (hungerBlocked) return;
        if (isHunger) return;

        isHunger = true;
        hungerIcon.enabled = true;
        CultIcon.enabled = false;
        DeterminationIcon.enabled = false;
        Debug.Log($"😩 {name} a faim !");
    }

    public void DesactiveHunger()
    {
        if (!isHunger) return;

        isHunger = false;
        hungerIcon.enabled = false;
        Debug.Log($"😌 {name} est rassasié.");

        if (gaugesManager != null)
        {
            gaugesManager.RemoveFromHungryList(gameObject);

            // Augmente la jauge de nourriture de 10%
            StartCoroutine(ApplyFoodBonus(0.1f));
        }
    }

    private IEnumerator ApplyFoodBonus(float amount)
    {
        // On peut ajouter un petit délai si besoin, ici c'est instantané
        yield return null;
        if (gaugesManager != null)
        {
            gaugesManager.ModifyFood(amount);
            // S'assure que la barre ne dépasse pas 100%
            gaugesManager.foodSlider.value = Mathf.Clamp01(gaugesManager.foodSlider.value);
        }
    }

    public void DesactiveHungerTemporarily(float duration)
    {
        DesactiveHunger();
        StartCoroutine(BlockHunger(duration));
    }

    private IEnumerator BlockHunger(float duration)
    {
        hungerBlocked = true;
        Debug.Log($"⏳ {name} ne pourra pas avoir faim pendant {duration} sec.");
        yield return new WaitForSeconds(duration);
        hungerBlocked = false;
        Debug.Log($"⌛ {name} peut à nouveau avoir faim si la jauge est basse.");
    }
}

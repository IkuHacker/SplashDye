using UnityEngine;
using System.Collections;

public class PNJHunger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer hungerIcon;
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
        Debug.Log($"😩 {name} a faim !");
    }

    public void DesactiveHunger()
    {
        if (!isHunger) return;

        isHunger = false;
        hungerIcon.enabled = false;
        Debug.Log($"😌 {name} est rassasié.");

        if (gaugesManager != null)
            gaugesManager.RemoveFromHungryList(gameObject);
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

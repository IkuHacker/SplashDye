using UnityEngine;
using System.Collections;

public class RandomFlash : MonoBehaviour
{
    [Header("Réglages")]
    public GameObject objectToFlash;
    public float baseInterval = 30f;
    public float intervalRandomRange = 10f;
    public float flashDuration = 0.001f; // 1 ms

    private void Start()
    {
        if (objectToFlash != null)
            objectToFlash.SetActive(false);

        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            float waitTime = baseInterval + Random.Range(-intervalRandomRange, intervalRandomRange);
            waitTime = Mathf.Max(1f, waitTime);

            yield return new WaitForSeconds(waitTime);

            objectToFlash.SetActive(true);
            yield return new WaitForSeconds(flashDuration);
            objectToFlash.SetActive(false);
        }
    }
}

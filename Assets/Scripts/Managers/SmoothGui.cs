using UnityEngine;

public class SmoothGui : MonoBehaviour
{
    public float amplitude = 20f;
    public float speed = 2f;

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);
    }
}

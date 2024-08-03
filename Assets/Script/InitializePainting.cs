using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitializePainting : MonoBehaviour
{
    public SpriteRenderer[] paints;
    public PaintingData[] paintsToInitialize;
    public List<ItemData> requiredsItems = new List<ItemData>();
    public float duration;

    public GameObject paintInformationsPanel;
    public Text paintsName;
    public Text paintDescrption;
    public Text paintDuration;
    public Transform contentDye;
    public GameObject dyeObject;
    void Start()
    {
        InitializePaint();
        GameObject gameController = GameObject.FindGameObjectWithTag("GameManager");
        Destroy(gameController);
    }

    public void InitializePaint()
    {
        // Shuffle the paintsToInitialize array
        ShuffleArray(paintsToInitialize);

        // Initialize only the first 6 paints
        for (int i = 0; i < Mathf.Min(paints.Length, 6); i++)
        {
            Paint paint = paints[i].gameObject.GetComponent<Paint>();
            paint.currentPaints = paintsToInitialize[i];
            paints[i].sprite = paintsToInitialize[i].paintVisual;
        }
    }

    public void ClosePaintPanel()
    {
        paintInformationsPanel.SetActive(false);
    }

    public void GoToGame() 
    {
        StateVariableController.requiredsItems = requiredsItems;
        StateVariableController.duration = duration;
        SceneManager.LoadScene("Map0ne");
    }
    void ShuffleArray(PaintingData[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            PaintingData temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}

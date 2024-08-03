using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Paint : MonoBehaviour
{
    public PaintingData currentPaints;
    private bool isInRange;
    public InitializePainting paintManager;

    private void Update()
    {
        if(isInRange && Input.GetKeyDown(KeyCode.E)) 
        {
            UpdatePainInformationsPanels();
        }

        if (isInRange && Input.GetKeyDown(KeyCode.Backspace))
        {
            ClosePaintPnel();
        }
    }

    public void ClosePaintPnel() 
    {
        paintManager.paintInformationsPanel.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    public void UpdatePainInformationsPanels() 
    {
        paintManager.paintInformationsPanel.SetActive(true);
        paintManager.paintsName.text = currentPaints.paintName;
        paintManager.paintDescrption.text = currentPaints.paintDescrption;
        float duration = currentPaints.duration;
        paintManager.duration = duration;
        int minutes = Mathf.FloorToInt(duration / 60f);
        int seconds = Mathf.FloorToInt(duration % 60f);
        paintManager.paintDuration.text = string.Format("Maximum duration : " + minutes+"min" + " et " +seconds + "s");
        for (int i = 0; i < paintManager.contentDye.childCount; i++)
        {
            Destroy(paintManager.contentDye.GetChild(i).gameObject);
           
        }

        paintManager.requiredsItems.Clear();
        foreach (ItemInInventory color in currentPaints.colorToPaint)
        {
            GameObject itemDyObject = Instantiate(paintManager.dyeObject, paintManager.contentDye);
            itemDyObject.GetComponent<Image>().sprite = color.itemData.visual;
            itemDyObject.GetComponentInChildren<Text>().text = color.count.ToString();
            for (int i = 0; i < color.count; i++)
            {
                paintManager.requiredsItems.Add(color.itemData);
            }
           

        }
    }
}

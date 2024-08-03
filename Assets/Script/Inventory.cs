using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public List<ItemInInventory> content = new List<ItemInInventory>();

  

    [SerializeField]
    private Transform inventorySlotsParent;
 



    [SerializeField]
    private GameObject itemIndicationPanel;
    public Text itemNameInPanel;
    public Text itemDescrptionInPanel;
    public Image itemVisualInPanel;



    public bool inventoryPanelActive;
    const int InventorySize = 7;





    public Sprite emptySlotVisual;

    public static Inventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la sc�ne");
            return;
        }

        instance = this;
    }


    private void Start()
    {
        RefreshContent();
    }


    public void AddItem(ItemData item)
    {
        itemIndicationPanel.SetActive(true);
        itemNameInPanel.text = item.itemName;
        itemDescrptionInPanel.text = item.itemDescription;
        itemVisualInPanel.sprite = item.visual;

        ItemInInventory[] itemInInventory = content.Where(elem => elem.itemData == item).ToArray();

        bool itemAdded = false;

        if (!isFull() && item != null)
        {
            if (itemInInventory.Length > 0 && item.stackable)
            {
                for (int i = 0; i < itemInInventory.Length; i++)
                {
                    if (itemInInventory[i].count < item.maxStack)
                    {
                        itemAdded = true;
                        itemInInventory[i].count++;

                        break;
                    }
                }

                if (!itemAdded)
                {
                    content.Add(
                        new ItemInInventory
                        {
                            itemData = item,
                            count = 1
                        }
                        );




                }
                else
                {

                }
            }
            else
            {
                content.Add(
                   new ItemInInventory
                   {
                       itemData = item,
                       count = 1
                   }
                       );





            }


        }
        else
        {
            return;
        }



        RefreshContent();
    }

    public void CloseItemIndicationPanel()
    {
        itemIndicationPanel.SetActive(false);
    }
    public void RemoveItem(ItemData item)
    {
        ItemInInventory itemInInventory = content.Where(elem => elem.itemData == item).FirstOrDefault();

        if (itemInInventory != null && itemInInventory.count > 1)
        {
            itemInInventory.count--;
        }
        else
        {
            content.Remove(itemInInventory);
        }

        RefreshContent();

    }

  

    public void RefreshContent()
    {
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
            currentSlot.countText.enabled = false;
        }

        // On peuple le visuel des slots selon le contenu réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.item = content[i].itemData;
            currentSlot.itemVisual.sprite = content[i].itemData.visual;

            if (currentSlot.item.stackable)
            {
                currentSlot.countText.enabled = true;
                currentSlot.countText.text = content[i].count.ToString();
            }
        }


    }






    public void LoadData(List<ItemInInventory> savedData)
    {
        content = savedData;
        for (int i = 0; i < content.Count; i++)
        {
            Debug.Log(savedData[i].itemData.name);
        }
        RefreshContent();
    }

    public void ClearContent()
    {
        content.Clear();
    }

    public List<ItemInInventory> GetContent()
    {
        return content;
    }

    public bool isFull()
    {
        return InventorySize == content.Count;
    }

}

[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
}



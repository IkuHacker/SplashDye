using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemActionPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject useItemButton;
    public Text itemNameText;

    [SerializeField]
    private GameObject actionPanel;
    public ItemEffect itemEffect;
    public static ItemActionPanel instance;

    



    private ItemData itemCurrentlySelected;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scene");
            return;
        }

        instance = this;
    }
    public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        itemCurrentlySelected = item;


        if (item == null)
        {
            actionPanel.SetActive(false);
            return;
        }
        itemNameText.text = item.itemName;

        switch (item.type)
        {
            case ItemData.Type.Resource:
                useItemButton.SetActive(false);
                break;
            case ItemData.Type.Consumable:
                useItemButton.SetActive(true);
                break;
        }

        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }




    public void UseActionButton()
    {
        itemEffect.AddSpeed(itemCurrentlySelected.speedAdd, itemCurrentlySelected.duration);
        itemEffect.AddDamageShoot(itemCurrentlySelected.damageToEnemyAdd, itemCurrentlySelected.duration);
        itemEffect.AddDoubleShoot(itemCurrentlySelected.canDoubleChot, itemCurrentlySelected.duration);
        itemEffect.AddTimeCounte(itemCurrentlySelected.timeOfCounterAdd);
        PlayerHealth.instance.Heal(itemCurrentlySelected.healthAdd);
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }





    public void DestroyActionButton()
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        Inventory.instance.RefreshContent();
        CloseActionPanel();
    }

    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }

  





}

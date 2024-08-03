using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemData item;
    public Image itemVisual;
    public Text countText;
    public void ClickOnSlot()
    {
        ItemActionPanel.instance.OpenActionPanel(item, transform.position);
    }

}

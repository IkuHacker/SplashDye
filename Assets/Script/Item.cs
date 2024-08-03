using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData item;
    public AudioClip takeItem;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            AudioManager.instance.PlayClipAt(takeItem, transform.position);
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }

    }
}

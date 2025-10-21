using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [HideInInspector] public bool hasABurger;
    public GameObject burgerGO;

    
    
    public void ActiverBurger(bool active)
    {
        hasABurger = active;
        burgerGO.SetActive(active);

    }
}


using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private Animator animator;
    public ItemData itemToDrop;// Intervalle entre les attaques (en secondes)

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    

    public void TakeDamage(int damage) 
    {
        animator.SetTrigger("Hurt");
        currentHealth -= damage;
        if(currentHealth <= 0) 
        {
            animator.SetTrigger("Die");

        }
    }


    public void Die() 
    {
        if(itemToDrop != null && itemToDrop.itemPrefab != null) 
        {
            Instantiate(itemToDrop.itemPrefab, transform.position, Quaternion.identity);
        }
        currentHealth = 0;
        Destroy(gameObject);
    }
}

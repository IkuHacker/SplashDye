using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn instance; // Singleton instance

    public GameObject enemyPrefab; // Prefab de l'ennemi à spawner
    public Transform player; // Référence au joueur
    public Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f); // Taille de la zone de spawn
    public float minDistanceFromPlayer = 5f; // Distance minimale à laquelle un ennemi peut apparaître du joueur
    public float maxDistanceFromPlayer = 20f; // Distance maximale à laquelle un ennemi peut apparaître du joueur
    public int numberOfEnemiesToSpawn = 5;
    public List<GameObject> enemys = new List<GameObject>();
    public List<ItemData> requiredItems = new List<ItemData>();
    public List<ItemData> itemToDrop = new List<ItemData>();
    public float strongEnemyProbability = 0.1f;
    public Color enemyStrongColor;// Probabilité d'apparition d'un ennemi renforcé

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        requiredItems = StateVariableController.requiredsItems;
        if (player == null)
        {
            Debug.LogError("Player reference is not assigned.");
            return;
        }
        SpawnEnemies();
        StartCoroutine(SpawnOnContinueEnemie());
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                20f,
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            ) + transform.position;

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();
            if (enemyBehavior != null)
            {
                enemyBehavior.Initialize();
            }
            else
            {
                Debug.LogError("EnemyBehavior component not found on enemyPrefab.");
            }

            // Vérifier la probabilité pour un ennemi renforcé
            if (Random.value < strongEnemyProbability)
            {
                // Augmenter la taille de l'ennemi
                enemy.transform.localScale *= 1.5f;

                enemyBehavior.damage *= 2;
                enemyBehavior.agent.speed *= 1.5f;
                SpriteRenderer enemySpriteRendere = enemy.GetComponent<SpriteRenderer>();
                enemySpriteRendere.color = enemyStrongColor;
                // Augmenter la vie de l'ennemi
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.maxHealth = 5;
                    enemyHealth.currentHealth = enemyHealth.maxHealth;
                }
                else
                {
                    Debug.LogError("EnemyHealth component not found on enemyPrefab.");
                }
            }

            enemys.Add(enemy);
            EnemyHealth enemyHealthComponent = enemy.GetComponent<EnemyHealth>();
            if (enemyHealthComponent != null)
            {
                int randomIndex = Random.Range(0, itemToDrop.Count);
                enemyHealthComponent.itemToDrop = itemToDrop[randomIndex];
            }
            else
            {
                Debug.LogError("EnemyHealth component not found on enemyPrefab.");
            }
        }

        foreach (ItemData item in requiredItems)
        {
            if (enemys.Count == 0)
            {
                Debug.LogWarning("No enemies available to assign required items.");
                break;
            }

            int randomIndex = Random.Range(0, enemys.Count);
            EnemyHealth enemyHealth = enemys[randomIndex].GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.itemToDrop = item;
                enemys.RemoveAt(randomIndex);
            }
            else
            {
                Debug.LogError("EnemyHealth component not found on enemy in the list.");
            }
        }
    }

    IEnumerator SpawnOnContinueEnemie()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);
            numberOfEnemiesToSpawn = 5;
            SpawnEnemies();
        }
    }
}

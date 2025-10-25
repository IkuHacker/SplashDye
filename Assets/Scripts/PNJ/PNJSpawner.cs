using UnityEngine;
using System.Collections;

public class PNJSpawner : MonoBehaviour
{
    [Header("Paramètres de spawn")]
    public GameObject agentPrefab;        // le prefab de ton agent
    public GameObject FirePrefab;  
    public float spawnInterval = 5f;      // temps entre chaque spawn
    public int maxAgents = 10;            // nombre maximum d'agents
    public Vector3 spawnAreaSize = new Vector3(20, 0, 20); // zone dans laquelle ils apparaissent

    [SerializeField] private GaugesManager gaugesManager;
    private int currentAgents = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentAgents < maxAgents)
            {
                SpawnAgent();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnAgent()
    {
        Vector3 randomPos = transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );
        if (UnityEngine.Random.Range(0, 5) == 2)
        {
            GameObject agent = Instantiate(FirePrefab, randomPos, Quaternion.identity);
        }
        else
        {
            GameObject agent = Instantiate(agentPrefab, randomPos, Quaternion.identity);
            currentAgents++;
            gaugesManager.pnjList.Add(agent);

            // Pour réduire le compteur quand un agent meurt ou est détruit
            agent.GetComponent<PNJMovements>().onAgentDestroyed += () => currentAgents--;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}

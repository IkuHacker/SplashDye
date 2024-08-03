using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform playerTransform;
    public Transform[] spawnPoint;
    void Start()
    {
        InitializeSpawn();
    }
    public void InitializeSpawn()
    {
        int spawnIndex = Random.Range(0, spawnPoint.Length);
        playerTransform.position = spawnPoint[spawnIndex].position;

    }
}

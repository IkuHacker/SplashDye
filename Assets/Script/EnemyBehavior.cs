using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float followDistance = 30f;
    public float attackDistance = 2f;
    public float attackRate = 2f;

    public NavMeshAgent agent;
    private Animator animator;
    public int damage;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (agent == null)
        {
            Debug.LogError($"{gameObject.name} is missing NavMeshAgent component.");
        }
        if (animator == null)
        {
            Debug.LogError($"{gameObject.name} is missing Animator component.");
        }
    }

    public void Initialize()
    {
        if (EnemySpawn.instance == null)
        {
            Debug.LogError("EnemySpawn instance is not assigned.");
            return;
        }

        if (EnemySpawn.instance.player == null)
        {
            Debug.LogError("Player reference in EnemySpawn is not assigned.");
            return;
        }

        // Check if the transform component is null
        if (transform == null)
        {
            Debug.LogError("Transform component is null.");
            return;
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is not assigned.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, EnemySpawn.instance.player.position);
        if (distanceToPlayer < followDistance)
        {
            agent.SetDestination(EnemySpawn.instance.player.position);
        }
    }

    void Update()
    {
        if (EnemySpawn.instance == null || EnemySpawn.instance.player == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, EnemySpawn.instance.player.position);
        if (distanceToPlayer < followDistance)
        {
            agent.SetDestination(EnemySpawn.instance.player.position);
        }

        if (distanceToPlayer < attackDistance && !isAttacking && !PlayerCombat.instance.isSpecialAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackPlayer());
        }
        else if (distanceToPlayer >= attackDistance && isAttacking && PlayerCombat.instance.isSpecialAttacking)
        {
            StopCoroutine(AttackPlayer());
            isAttacking = false;
        }

        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    private IEnumerator AttackPlayer()
    {
        animator.SetTrigger("Attack");
        PlayerHealth.instance.TakeDamage(damage);
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }
}

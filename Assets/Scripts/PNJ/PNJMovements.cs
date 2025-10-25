using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PNJMovements : MonoBehaviour
{
[Header("Paramètres de déplacement")]
    public float moveSpeed = 3f;
    public float wanderRadius = 10f;
    public float changeTargetInterval = 3f;
    public float stopDistance = 0.5f;
    public float rotationSpeed = 5f;
    public bool flipSprite = true; // active/désactive le flip automatique
    public string Job = "None";
    private Vector3[] CultPositions = new Vector3[]
    {
    new Vector3(-4.3f, 1f, -0.7f),
    new Vector3(-4f, 1f, -2.7f),
    new Vector3(-1.9f, 1f, -3.7f),
    new Vector3(0.3f, 1f, -3f),
    new Vector3(-1f, 1f, -1.1f),
    new Vector3(0.3f, 1f, 0.6f),
    new Vector3(-1.7f, 1f, 1.5f),
    new Vector3(-3.4f, 1f, 1f)
    };

    private Vector3[] DeterminationPositions = new Vector3[]
    {
    new Vector3(7.8f, 1f, -4.5f),
    new Vector3(6.3f, 1f, -7.3f),
    new Vector3(4.5f, 1f, -10.3f)
    };

    public Animator animBack;
    private Animator anim;
    private Vector3 targetPosition;
    private Vector3 moveDir;
    private Vector3 velocity;

    public event Action onAgentDestroyed;

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(ChangeTargetRoutine());
    }

    private void Update()
    {
        MoveTowardsTarget();
        UpdateAnimatorParameters();
        HandleFlip();

    }

    private IEnumerator ChangeTargetRoutine()
    {
        while (true)
        {
            if (Job == "Cult")
            {
                targetPosition = CultPositions[UnityEngine.Random.Range(0, CultPositions.Length)];
            }
            else if (Job == "Determination")
            {
                targetPosition = DeterminationPositions[UnityEngine.Random.Range(0, DeterminationPositions.Length)];
            }
            else
            {
                // Nouvelle destination aléatoire
                Vector3 randomDirection = new Vector3(
                UnityEngine.Random.Range(-wanderRadius, wanderRadius),
                0,
                UnityEngine.Random.Range(-wanderRadius, wanderRadius));
                targetPosition = transform.position + randomDirection;
            }
            yield return new WaitForSeconds(changeTargetInterval);
        }
    }
    private void HandleFlip()
    {
        if (!flipSprite || moveDir == Vector3.zero) return;

        // Si tu utilises un SpriteRenderer
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            if (moveDir.x < -0.1f) sr.flipX = true;
            else if (moveDir.x > 0.1f) sr.flipX = false;
        }

        // Sinon, si c’est un perso 3D, tu peux faire :
        // if (moveDir.x > 0) transform.localScale = new Vector3(1, 1, 1);
        // else if (moveDir.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0;

        if (direction.magnitude > stopDistance)
        {
            moveDir = direction.normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
        }
        else
        {
            moveDir = Vector3.zero;
        }
    }

    private void UpdateAnimatorParameters()
    {
        // Convertit le vecteur global en local
        Vector3 localVelocity = transform.InverseTransformDirection(moveDir * moveSpeed);

        // Calcul des valeurs à envoyer à l'Animator
        float horizontal = localVelocity.x;
        float vertical = localVelocity.z;
        float speed = new Vector2(horizontal, vertical).magnitude;


        anim.SetFloat("Speed", speed);
        animBack.SetFloat("Speed", speed);

    }

    private void OnDestroy()
    {
        onAgentDestroyed?.Invoke();
    }
}

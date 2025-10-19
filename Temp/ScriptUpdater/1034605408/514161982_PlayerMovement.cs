using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;


    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène !");
            Destroy(gameObject);
            return;
        }
        instance = this;

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Vérifie si le joueur est au sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        Vector3 velocity = direction * moveSpeed;
        Vector3 currentVelocity = rb.linearVelocity;

        // Conserve la vitesse verticale (chute ou saut)
        rb.linearVelocity = new Vector3(velocity.x, currentVelocity.y, velocity.z);
    }


    // --- NOUVEAU SYSTÈME D’INPUT UNITY ---
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }



    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}

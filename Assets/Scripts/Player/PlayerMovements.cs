using UnityEngine;
using UnityEngine.InputSystem; 


public class PlayerMovements : MonoBehaviour
{
[Header("Movement")]
    public float moveSpeed = 5f;
    [HideInInspector] public float walkSpeed;

    public Transform orientation;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    private Rigidbody rb;
    private PlayerInputActions inputActions; // référence vers ton Input Action Asset

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Initialisation des actions d'input
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // On récupère le Vector2 de l’action "Move"
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}

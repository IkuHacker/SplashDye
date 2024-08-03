using System.Collections;
using UnityEngine;

public class AtelierPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 velocity = Vector3.zero;

    [Header("Flip")]
    public float flipDuration = 0.2f;
    private bool isFacingRight = true;
    private bool isFlipping = false;

    [Header("Jump")]
    private bool isGrounded;
    public bool isJumping;
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public float jumpForce = 350f;

    [Header("Verificator")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistance = 0.2f;




    [Header("Other")]
    public Rigidbody rb;
    public Animator animator;
    public static AtelierPlayerMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;
        verticalMovement = Input.GetAxis("Vertical") * moveSpeed;

        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * raycastDistance, Color.red);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if ((jumpBufferCounter > 0f && coyoteTimeCounter > 0f))
        {
            isJumping = true;
            jumpBufferCounter = 0f;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (!isFlipping && (isFacingRight && horizontalMovement < 0f || !isFacingRight && horizontalMovement > 0f))
        {
            StartCoroutine(Flip());
        }

        float characterVelocity = Mathf.Abs(rb.velocity.sqrMagnitude);
        float horizontalVelocity = Input.GetAxis("Horizontal");
        float verticalVelocity = Input.GetAxis("Vertical");
        animator.SetFloat("speed", characterVelocity);
        animator.SetFloat("Horizontal", horizontalVelocity);
        animator.SetFloat("Vertical", verticalVelocity);

    }




    private void FixedUpdate()
    {
       
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, raycastDistance, groundLayer);
        MovePlayer(horizontalMovement, verticalMovement);
    }

    public void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        Vector3 targetVelocity = new Vector3(_horizontalMovement, rb.velocity.y, _verticalMovement);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

        if (isJumping)
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f));
            isJumping = false;
        }
    }



    private IEnumerator Flip()
    {
        isFlipping = true;

        Vector3 localScale = transform.localScale;
        float startScaleX = localScale.x;
        float targetScaleX = -startScaleX;
        float elapsedTime = 0f;

        isFacingRight = !isFacingRight;

        while (elapsedTime < flipDuration)
        {
            float newScaleX = Mathf.Lerp(startScaleX, targetScaleX, elapsedTime / flipDuration);
            localScale.x = newScaleX;
            transform.localScale = localScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        localScale.x = targetScaleX;
        transform.localScale = localScale;
        isFlipping = false;
    }
}

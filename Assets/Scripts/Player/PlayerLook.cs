using UnityEngine;
using UnityEngine.InputSystem; // nouveau système d'input

public class PlayerLook : MonoBehaviour
{
    [Header("Références")]
    public Transform orientation;  // sert à aligner la direction du joueur
    public Transform playerCamera; // ta caméra principale

    [Header("Sensibilité")]
    public float sensX = 1f;
    public float sensY = 1f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    private float xRotation;
    private float yRotation;

    public GameObject MetierMenu;

    private void Awake()
    {
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
        if (MetierMenu.activeSelf == false)
        {
            // Lis la valeur de l'action "Look" (Vector2)
            lookInput = inputActions.Player.Look.ReadValue<Vector2>();

            // Calcul de la rotation
            float mouseX = lookInput.x * sensX;
            float mouseY = lookInput.y * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            // Applique à la caméra
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }

    }
}

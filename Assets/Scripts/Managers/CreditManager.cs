using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // indispensable pour le nouveau système d'input



public class CreditManager : MonoBehaviour
{
    private InputAction escapeAction;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private string sceneName;


    void OnDestroy()
    {
        // Toujours se désabonner proprement
        if (escapeAction != null)
            escapeAction.performed -= ctx => EscapeToCreditScene();
    }

    void Awake()
    {
        // Récupère l’action nommée “Interact” (ou celle que tu veux)
        escapeAction = playerInput.actions["Escape"];

        // Abonnement à l’événement de clic
        escapeAction.performed += ctx => EscapeToCreditScene();
    }
    void EscapeToCreditScene()
    {
        SceneManager.LoadScene(sceneName);
    }

  
}

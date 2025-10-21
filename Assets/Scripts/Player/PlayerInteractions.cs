using UnityEngine;
using UnityEngine.InputSystem; // indispensable pour le nouveau système d'input

public class PlayerInteractions : MonoBehaviour
{
    [Header("Paramètres du tir")]
    [SerializeField] private float rayDistance = 10f; // distance max du raycast
    [SerializeField] private LayerMask interactionLayers; // couche PNJ + Cooker

    private Camera mainCam;
    private PlayerInput playerInput;

    private PlayerInventory playerInventory;
    private InputAction interactAction;

    public GaugesManager gaugesManager;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            Debug.LogError("⚠️ Aucun PlayerInput trouvé sur le joueur !");

        playerInventory = GetComponent<PlayerInventory>();

        // Récupère l’action nommée “Interact” (ou celle que tu veux)
        interactAction = playerInput.actions["Interact"];

        // Abonnement à l’événement de clic
        interactAction.performed += ctx => FireRaycast();
    }

    void OnDestroy()
    {
        // Toujours se désabonner proprement
        if (interactAction != null)
            interactAction.performed -= ctx => FireRaycast();
    }

    void Start()
    {
        mainCam = Camera.main;
        if (mainCam == null)
            Debug.LogError("⚠️ Aucune caméra principale trouvée dans la scène !");
    }

    void FireRaycast()
    {
        if (mainCam == null) return;

        Debug.Log("🧍 Le joueur a intéragis!");

        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactionLayers))
        {
            Debug.Log("🧍 Le joueur a tiré un raycast !");
            GameObject hitObject = hit.collider.gameObject;
            string layerName = LayerMask.LayerToName(hitObject.layer);

            if (layerName == "PNJ")
            {
                if (playerInventory.hasABurger)
                {
                    playerInventory.ActiverBurger(false);

                    if (gaugesManager != null)
                    {
                        gaugesManager.ModifyFood(0.1f); // +10% par burger
                    }

                    PNJHunger hunger = hitObject.GetComponent<PNJHunger>();
                    if (hunger != null)
                    {
                        hunger.DesactiveHungerTemporarily(10f);
                    }

                }

                playerInventory.ActiverBurger(false);
                hitObject.transform.GetComponent<PNJHunger>().DesactiveHunger();
            }
            else if (layerName == "Cooker")
            {
                playerInventory.ActiverBurger(true);
            }
        }
        else
        {
            Debug.Log("❌ Aucun objet interactif touché.");
        }

        // Pour visualiser le rayon dans la scène
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow, 1f);
    }
}

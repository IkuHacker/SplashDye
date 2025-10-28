using UnityEngine;
using UnityEngine.InputSystem;

public class FunctionManager : MonoBehaviour
{
    
    public GameObject Cancel;
    public RectTransform FunctionBurger;
    public RectTransform FunctionDetermination;
    public RectTransform FunctionCult;
    public GameObject Knot;
    public GameObject Hit;

    void Update()
    {
        if (Hit.transform.parent != null && Hit.transform.parent.gameObject.name == "PNJ(Clone)")
        {
            Hit = Hit.transform.parent.gameObject;
        }
        print(Hit.GetComponent<PNJMovements>().Job);

        // fonction 1 : Bouton Croix
        print(Mouse.current.position.ReadValue());
        if (Mouse.current.position.ReadValue().x > 874 && Mouse.current.position.ReadValue().x < 1049 &&
            Mouse.current.position.ReadValue().y > 401 && Mouse.current.position.ReadValue().y < 720 &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Knot.SetActive(true);
            Hit.GetComponent<PNJMovements>().Job = "None";
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }


        // fonction 2 : Bouton Diable
        if (Mouse.current.position.ReadValue().x > 560 && Mouse.current.position.ReadValue().x < 875 &&
            Mouse.current.position.ReadValue().y > 600 && Mouse.current.position.ReadValue().y < 900 &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("Cult");
            Hit.GetComponent<PNJMovements>().Job = "Cult";
            Hit.GetComponent<PNJHunger>().DeterminationIcon.enabled = false;
            Hit.GetComponent<PNJHunger>().CultIcon.enabled = true;
            Knot.SetActive(true);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }


        // fonction 3 : Bouton tuer
        if (Mouse.current.position.ReadValue().x > 1050 && Mouse.current.position.ReadValue().x < 1375 &&
            Mouse.current.position.ReadValue().y > 550 && Mouse.current.position.ReadValue().y < 900 &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("Food");
            Destroy(Hit);
            Knot.SetActive(true);
            Hit.GetComponent<PNJMovements>().Job = "None";
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        // fonction 4 : Bouton Flamme
        if (Mouse.current.position.ReadValue().x > 675 && Mouse.current.position.ReadValue().x < 1300 &&
            Mouse.current.position.ReadValue().y > 75 && Mouse.current.position.ReadValue().y < 400 &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("Determination");
            Hit.GetComponent<PNJMovements>().Job = "Determination";
            Hit.GetComponent<PNJHunger>().DeterminationIcon.enabled = true;
            Hit.GetComponent<PNJHunger>().CultIcon.enabled = false;
            Knot.SetActive(true);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}

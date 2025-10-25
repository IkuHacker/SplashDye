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

        if (Mouse.current.position.ReadValue().x > 390 && Mouse.current.position.ReadValue().x < 480 &&
            Mouse.current.position.ReadValue().y > 170 && Mouse.current.position.ReadValue().y < 250 &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Knot.SetActive(true);
            Hit.GetComponent<PNJMovements>().Job = "None";
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        if (Mouse.current.position.ReadValue().x > 300 && Mouse.current.position.ReadValue().x < 390 &&
            Mouse.current.position.ReadValue().y > 200 && Mouse.current.position.ReadValue().y < 315 &&
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
        if (Mouse.current.position.ReadValue().x > 481 && Mouse.current.position.ReadValue().x < 545 &&
            Mouse.current.position.ReadValue().y > 200 && Mouse.current.position.ReadValue().y < 315 &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            print("Food");
            Destroy(Hit);
            Knot.SetActive(true);
            Hit.GetComponent<PNJMovements>().Job = "None";
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
        if (Mouse.current.position.ReadValue().x > 375 && Mouse.current.position.ReadValue().x < 490 &&
            Mouse.current.position.ReadValue().y > 66 && Mouse.current.position.ReadValue().y < 169 &&
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

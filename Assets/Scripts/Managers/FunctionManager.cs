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
        HandlePNJParent();
        print(Hit.GetComponent<PNJMovements>().Job);

    }

    // --- FONCTIONS SÉPARÉES ---

    void HandlePNJParent()
    {
        if (Hit.transform.parent != null && Hit.transform.parent.gameObject.name == "PNJ(Clone)")
        {
            Hit = Hit.transform.parent.gameObject;
        }
    }
    public void ClickCancel()
    {
        Knot.SetActive(true);
        Hit.GetComponent<PNJMovements>().Job = "None";
        ResumeGame();
    }

    public void ClickCult()
    {
        print("Cult");
        var pnjMove = Hit.GetComponent<PNJMovements>();
        var pnjHunger = Hit.GetComponent<PNJHunger>();

        pnjMove.Job = "Cult";
        pnjHunger.DeterminationIcon.enabled = false;
        pnjHunger.CultIcon.enabled = true;

        Knot.SetActive(true);
        ResumeGame();
    }

    public void ClickFood()
    {
        print("Food");
        Hit.GetComponent<PNJMovements>().Job = "None";
        Destroy(Hit);
        Knot.SetActive(true);
        ResumeGame();
    }

    public void ClickDetermination()
    {
        print("Determination");
        var pnjMove = Hit.GetComponent<PNJMovements>();
        var pnjHunger = Hit.GetComponent<PNJHunger>();

        pnjMove.Job = "Determination";
        pnjHunger.DeterminationIcon.enabled = true;
        pnjHunger.CultIcon.enabled = false;

        Knot.SetActive(true);
        ResumeGame();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}

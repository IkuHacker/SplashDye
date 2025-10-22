using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string creditSceneName;
    [SerializeField] private Animator buttonPlayAnimator;
    [SerializeField] private Animator buttonCreditAnimator;

    [SerializeField] private TransitionManager transitionManager;





    public void PlayGame()
    {
        buttonPlayAnimator.CrossFade("Pressed", 0.1f);
        buttonPlayAnimator.CrossFade("Normal", 0f);
        transitionManager.LoadTransition(sceneName);

    }

    public void CreditScene()
    {
        buttonCreditAnimator.CrossFade("Pressed", 0.1f);
        buttonCreditAnimator.CrossFade("Normal", 0f);
        transitionManager.LoadTransition(creditSceneName);


    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

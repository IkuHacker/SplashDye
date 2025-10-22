using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;  


public class TransitionManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator;
    
    public void LoadTransition(string sceneName)
    {
        StartCoroutine(PlayTransition(sceneName));
    }

    private IEnumerator PlayTransition(string sceneName)
    {
        // Lance l'animation de transition
        transitionAnimator.CrossFade("TransitionIn", 0f);

        // Récupère la durée de l’animation
        AnimatorStateInfo stateInfo = transitionAnimator.GetCurrentAnimatorStateInfo(0);
        float transitionDuration = stateInfo.length;

        // Attends la fin de l’animation
        yield return new WaitForSeconds(transitionDuration);

        // Charge la nouvelle scène
        SceneManager.LoadScene(sceneName);
    }
}

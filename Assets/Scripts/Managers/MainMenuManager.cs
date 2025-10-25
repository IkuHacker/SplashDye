using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string creditSceneName;
    [SerializeField] private string tutorialSceneName;


    public void PlayGame()
    {

        SceneManager.LoadScene(sceneName);


    }

    public void CreditScene()
    {

        SceneManager.LoadScene(creditSceneName);


    }

    
    public void TutorialScene()
    {

        SceneManager.LoadScene(tutorialSceneName);


    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuManager1 : MonoBehaviour
{
    public Image PlayButton;
    public Image QuitButton;
    public float Speed = 5f;

    void Update()
    {
        if (Mouse.current.position.ReadValue().x > 140 && Mouse.current.position.ReadValue().x < 720 && Mouse.current.position.ReadValue().y > 430 && Mouse.current.position.ReadValue().y < 640)
        {
            if (PlayButton.color.a < 1){PlayButton.color = new Color(1, 1, 1, PlayButton.color.a + Speed * Time.deltaTime);}
        }
        else
        {
            if (PlayButton.color.a > 0){PlayButton.color = new Color(1, 1, 1, PlayButton.color.a - Speed * 2 * Time.deltaTime);}
        }
        if (Mouse.current.position.ReadValue().x > 1220 && Mouse.current.position.ReadValue().x < 1750 && Mouse.current.position.ReadValue().y > 430 && Mouse.current.position.ReadValue().y < 640)
        {
            if (QuitButton.color.a < 1){QuitButton.color = new Color(1, 1, 1, QuitButton.color.a + Speed * Time.deltaTime);}
        }
        else
        {
            if (QuitButton.color.a > 0){QuitButton.color = new Color(1, 1, 1, QuitButton.color.a - Speed * 2 * Time.deltaTime);}
        }
    }
}

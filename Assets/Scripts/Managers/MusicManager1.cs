using UnityEngine;
using UnityEngine.InputSystem;

public class MusicManager1 : MonoBehaviour
{
    public AudioSource Source;
    public AudioSource SFXSource;

    public AudioClip MenuBackground;  
    public AudioClip SFX;

    void Start()
    {
        Source.clip = MenuBackground;
        Source.Play();
    }

    void Update()
    {
        if (SFXSource != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            SFXSource.clip = SFX;
            SFXSource.Play();
        }
    }
}

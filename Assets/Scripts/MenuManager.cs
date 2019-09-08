using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    string hoverOverSound = "ButtonHover";

    [SerializeField]
    string pressButtonSound = "ButtonPress";

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No Audiomanager Found");
        }
    }

    public void Play()
    {
        audioManager.PlaySound(pressButtonSound);
        SceneManager.LoadScene("MainLevel");
    }

    public void Quit()
    {
        audioManager.PlaySound(pressButtonSound);
        Debug.Log("APPLICATION QUIT");
        Application.Quit();
    }
    public void OnMouseOver()
    {
        audioManager.PlaySound(hoverOverSound);
    }
}

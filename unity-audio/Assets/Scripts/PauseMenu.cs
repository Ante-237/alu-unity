using StarterAssets;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Timer UITimer;
    public GameObject PauseCanvas;
    public SettingsSO settings;
    public GameObject Player;
    public StarterAssetsInputs starter;



    private void Start()
    {
        if(settings.optionsActivated)
        {
            UITimer.seconds = settings.CurrentTime;
            Player.transform.position = settings.PlayerPosition;
            //settings.optionsActivated = false;
            UITimer.isStarted = false;
            PauseCanvas.SetActive(true);
            Debug.Log("Options Activation Working");
        }
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        UITimer.isStarted = false;   
        PauseCanvas.SetActive(true);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UITimer.isStarted = true;
        PauseCanvas.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UITimer.seconds = 0.0f;
        settings.optionsActivated = false;
    }

    public void MainMenu()
    {
        settings.optionsActivated = false;
        SceneManager.LoadScene(4);

    }

    public void Options()
    {
        UITimer.isStarted = false;
        settings.CurrentTime = UITimer.seconds;
        settings.PlayerPosition = Player.transform.position;
        settings.optionsActivated = true;
        settings.PreviousScene = SceneManager.GetActiveScene().buildIndex;

        UITimer.isStarted = false;
        SceneManager.LoadScene(3);
        PauseCanvas.SetActive(false);
    }

}

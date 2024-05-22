using StarterAssets;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Timer UITimer;
    public GameObject PauseCanvas;
    public SettingsSO settings;
    public GameObject Player;
    public StarterAssetsInputs starter;
    public AudioMixer mixer;
    public AudioMixerSnapshot[] snapshots;
    private float[] snapshotRatio = { 0, 1 };



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
        mixer.TransitionToSnapshots(snapshots, snapshotRatio, 0.1f); 
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UITimer.isStarted = true;
        DefaultMixer();
        PauseCanvas.SetActive(false);
    }

    public void Restart()
    {
        DefaultMixer();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        UITimer.seconds = 0.0f;
        settings.optionsActivated = false;
    }

    public void MainMenu()
    {
        DefaultMixer();
        settings.optionsActivated = false;
        SceneManager.LoadScene(0);

    }

    private void DefaultMixer()
    {
        mixer.TransitionToSnapshots(snapshots, new float[] { 1, 0 }, 0.1f); 
    }

    public void Options()
    {
        UITimer.isStarted = false;
        settings.CurrentTime = UITimer.seconds;
        settings.PlayerPosition = Player.transform.position;
        settings.optionsActivated = true;
        settings.PreviousScene = SceneManager.GetActiveScene().buildIndex;

        UITimer.isStarted = false;
        SceneManager.LoadScene(1);
        PauseCanvas.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 40f;
    public int health = 4;

    /// <summary>
    /// keeps reference to text element.
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// keeps reference to health text element
    /// </summary>
    public TextMeshProUGUI healthText;

    /// <summary>
    /// reference to win use canvas panle
    /// </summary>
    public GameObject WinLooseUI;
    
    private Rigidbody rb;

    private int score = 0;

    private float horizontal;
    private float vertical;



    void SetScoreText()
    {
        scoreText.text = "Score : " + score;
    }

    void SetHealth()
    {
        if (health < 0)
        {
            health = 0;
        }
        healthText.text = "Health : " + health;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
       LoadMainMenu();
    }

    /// <summary>
    /// loads the main menu scene if escape key is pressed.
    /// </summary>
    private void LoadMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
        }
    }

    
    
    /// <summary>
    /// reloads current scene while reseting various settings
    /// </summary>
    /// <param name="seconds"> the amount of time to wait before scene is loaded </param>
    /// <returns></returns>
    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        health = 5;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
    
    

    public void FixedUpdate()
    {
        PlayerMovements();
    }

    /// <summary>
    /// player movements mapping
    /// </summary>
    public void PlayerMovements()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        rb.AddForce(new Vector3(horizontal, 0, vertical) * (speed * Time.deltaTime), ForceMode.Acceleration);
    }

    
    /// <summary>
    /// handles collision with objects
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            score++;
            SetScoreText();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            
            health--;
            if (health <= 0)
            {
                SetGameOverUI();
                StartCoroutine(LoadScene(3.0f));
            }
            SetHealth();
        }

        if (other.CompareTag("Goal"))
        {
            SetWinUI();
            StartCoroutine(LoadScene(3.0f));
        }
    }

    
    /// <summary>
    /// enables the win canvas menu with right settings
    /// </summary>
    private void SetWinUI()
    {
        WinLooseUI.SetActive(true);
        Image currentImage = WinLooseUI.GetComponent<Image>();
        TextMeshProUGUI uiText = WinLooseUI.GetComponentInChildren<TextMeshProUGUI>();
        uiText.text = "You Win!";
        currentImage.color = new Color(0, 1, 0, 1);
        uiText.color = new Color(0, 0, 0, 1);
        
    }

    
    /// <summary>
    /// enables the gameover canvas menu with right settings
    /// </summary>
    private void SetGameOverUI()
    {
        WinLooseUI.SetActive(true);
        Image currentImage = WinLooseUI.GetComponent<Image>();
        TextMeshProUGUI uiText = WinLooseUI.GetComponentInChildren<TextMeshProUGUI>();
        uiText.text = "Game Over!";
        currentImage.color = new Color(1, 0, 0, 1);
        uiText.color = new Color(1, 1, 1, 1);
        
        
        health = 5;
        score = 0;
    }
}

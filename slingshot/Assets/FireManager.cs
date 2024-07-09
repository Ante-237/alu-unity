using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class FireManager : MonoBehaviour
{
    public List<GameObject> sprites = new List<GameObject>();
    public SettingSO settings;
    public TextMeshProUGUI DebugText;
    public TextMeshProUGUI ScoreText;
    public LineRenderer lineRenderer;

    public GameObject AmmoBall;
    public GameObject Obstacles;
    public Transform spawnPoint;
    public GameObject StartBtn;

    private TouchControls touchControls;
    private Vector2 touchPositions;
    private Vector3 worldCordinates;
    private Vector3 someCoordinates;

    // public InputAction TouchGesture;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    private GameObject bulletObject;
    Camera _camera;

    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
        TouchSimulation.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
        TouchSimulation.Disable();
    }

    private void SubStart()
    {
        touchControls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    List<ARRaycastHit> castHits = new List<ARRaycastHit>();
    NativeArray<Vector2> currentPlanePoints = new NativeArray<Vector2>();

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Started :" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        touchPositions = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
        Vector3 screenCoordinates = new Vector3(touchPositions.x, touchPositions.y, _camera.nearClipPlane);  
        worldCordinates = _camera.ScreenToWorldPoint(screenCoordinates);
        worldCordinates.z = 0;
        DebugText.text = worldCordinates.ToString();

        raycastManager.Raycast(touchPositions, castHits, trackableTypes: UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        currentPlanePoints = planeManager.GetPlane(castHits[0].trackableId).boundary;

        if (settings.gameStarted)
        {
          bulletObject = Instantiate(AmmoBall, worldCordinates, Quaternion.identity);
        }
        else
        {
            Instantiate(Obstacles, castHits[Random.Range(0, castHits.Count)].trackable.transform.position, Quaternion.identity);
        }
 
        // DebugText.text = castHits.Count.ToString();
       
        // StartSpawnSequence();

       // bulletObject = Instantiate(AmmoBall, worldCordinates, Quaternion.identity);
        bulletObject.GetComponent<Rigidbody>().useGravity = false;
        bulletObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private int runNumber = 5;
    private bool runOnce = false;

    private void StartSpawnSequence()
    {
        if(!runOnce)
        {
            for (int i = 0; i < runNumber; i++)
            {
                var tempObj = Instantiate(Obstacles, new Vector3(currentPlanePoints[i].x, 0, currentPlanePoints[i].y), Quaternion.identity);
                tempObj.GetComponent<randomMove>().SetMotion(currentPlanePoints);
                // Debug.Log("X :" + planeLocations[i].x + " Y : " + planeLocations[i].y);
            }

            runOnce = true;
        }
       
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("Touch Ended :" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
        // bulletObject.GetComponent<Ammo>().FireBall((touchControls.Touch.TouchPosition.ReadValue<Vector3>() - worldCordinates).normalized);
        touchPositions = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
        Vector3 screenCoordinates = new Vector3(touchPositions.x, touchPositions.y, _camera.nearClipPlane);
        someCoordinates = _camera.ScreenToWorldPoint(screenCoordinates);
        someCoordinates.z = 0;
        DebugText.text = someCoordinates.ToString();

        // bulletObject.GetComponent<Ammo>().FireBall(Vector3.forward);
        bulletObject.GetComponent<Rigidbody>().useGravity = true;
        bulletObject.GetComponent<Rigidbody>().isKinematic = false;
        settings.MagnitudeFactor = Vector3.Distance(someCoordinates, worldCordinates) * 100;
        Ray ray = new Ray();
        bulletObject.GetComponent<Ammo>().FireBall(_camera.transform.forward);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(worldCordinates, someCoordinates);
    }

    private void Start()
    {
        Assert.IsNotNull(raycastManager, "RayCast Manager has not been assigned");
        Assert.IsNotNull(planeManager, "Plane Manager has not been assigned");
        updateAmmo();
        SubStart();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;   
        _camera = Camera.main; 

        lineRenderer = GetComponent<LineRenderer>();
    }

 

    private void FingerDown(Finger finger)
    {
       // DebugText.text = "Finger Down";
        lineRenderer.positionCount = 3;
        
        Vector3 temp = new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0);
       // DebugText.text = "Finger Down : " + finger.screenPosition;

       temp = _camera.ScreenToWorldPoint(finger.screenPosition);
       // temp.z = 0;
       
       lineRenderer.SetPosition(0, temp);
    }

    private void FingerMove(Finger finger)
    {
       // DebugText.text = "Finger Moved";
        Vector3 temp = new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0);

       // DebugText.text = "Finger Moved :" + finger.screenPosition;
        temp = _camera.ScreenToWorldPoint(finger.screenPosition);
        //  temp.z = 0;
        lineRenderer.SetPosition(1, temp);
    }

    private void FingerUp(Finger finger)
    {
       // DebugText.text = "Finger Up ";
        Vector3 temp = new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0);
       // DebugText.text = "Finger Up :" + finger.screenPosition;
        temp = _camera.ScreenToWorldPoint(finger.screenPosition);
        lineRenderer.SetPosition(2, temp);
    }

    public void updateAmmo()
    {
        for (int i = 0; i < settings.ammo; i++)
        {
            sprites[i].SetActive(true);
        }
    }

    public void Update()
    {

    }

    public void StartActualGame()
    {
        settings.gameStarted = true;
        StartBtn.SetActive(false);
    }
    public void HalfRestart()
    {
        // reset score points
        // reset fire balls number

        settings.Score = 0;
        settings.ammo = 5;

        // call triggers to ensure updates run through

        ScoreText.text = settings.Score.ToString();
        updateAmmo();
    }
    public void RestartApplication() => SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    public void ExitApplication() => Application.Quit();


    /*
    private void OldInputSystemControlsTEst()
    {
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                DebugText.text = "Touch Began";
            }

            if (touch.phase == TouchPhase.Moved)
            {
                DebugText.text = "Touch Moved";
            }

            if (touch.phase == TouchPhase.Ended)
            {
                DebugText.text = "Touch Ended";
            }
        }
    } 
    */
} 

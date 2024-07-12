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
    public GameObject RestartBtn;

    private TouchControls touchControls;
    private Vector2 touchPositions;
    private Vector3 worldCordinates;
    private Vector3 someCoordinates;

    // audio source things
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip touchClip;
    [SerializeField] private AudioClip fireClip;

    // public InputAction TouchGesture;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    private GameObject bulletObject;
    Camera _camera;

    private bool project = false;

    private Scene extraScene;
    [SerializeField] private Transform FireDirection;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private int lq = 50;
    [SerializeField] private GameObject Projectile;

    private Rigidbody rb;
    private PhysicsScene PhysicsScene;

    private void Awake()
    {
        touchControls = new TouchControls();
        settings.gameStarted = false;
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
    private int count = 0;
    private Vector3 DirectionFire = Vector3.forward;

    private void StartTouch(InputAction.CallbackContext context)
    {
        if(settings.ammo > 0)
        {
            Debug.Log("Touch Started :" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
            touchPositions = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            Vector3 screenCoordinates = new Vector3(touchPositions.x, touchPositions.y, _camera.nearClipPlane);
            worldCordinates = _camera.ScreenToWorldPoint(screenCoordinates);
            // worldCordinates.z = 0;
            DebugText.text = worldCordinates.ToString();

            raycastManager.Raycast(touchPositions, castHits, trackableTypes: UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
            currentPlanePoints = planeManager.GetPlane(castHits[0].trackableId).boundary;

            if (settings.gameStarted)
            {
                bulletObject = Instantiate(AmmoBall, worldCordinates, Quaternion.identity);
            }
            else
            {
                foreach (var temp in castHits)
                {
                    if (count > 5) break;
                   //Instantiate(Obstacles, temp.pose.position, Quaternion.identity);
                    count++;
                }
                 Instantiate(Obstacles, castHits[Random.Range(0, castHits.Count)].pose.position, Quaternion.identity);
            }

            // DebugText.text = castHits.Count.ToString();
            // StartSpawnSequence();
            // bulletObject = Instantiate(AmmoBall, worldCordinates, Quaternion.identity);
            bulletObject.GetComponent<Rigidbody>().useGravity = false;
            bulletObject.GetComponent<Rigidbody>().isKinematic = true;
            project = true;
        }

        updateAmmo();

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
        if(settings.ammo > 0)
        {
            Debug.Log("Touch Ended :" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());
            // bulletObject.GetComponent<Ammo>().FireBall((touchControls.Touch.TouchPosition.ReadValue<Vector3>() - worldCordinates).normalized);
            touchPositions = touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            Vector3 screenCoordinates = new Vector3(touchPositions.x, touchPositions.y, _camera.nearClipPlane);
            someCoordinates = _camera.ScreenToWorldPoint(screenCoordinates);
            //someCoordinates.z = 0;
            DebugText.text = someCoordinates.ToString();

            // bulletObject.GetComponent<Ammo>().FireBall(Vector3.forward);
            bulletObject.GetComponent<Rigidbody>().useGravity = true;
            bulletObject.GetComponent<Rigidbody>().isKinematic = false;
            settings.MagnitudeFactor = Vector3.Distance(someCoordinates, worldCordinates) * 100;

            DirectionFire = (someCoordinates - worldCordinates).normalized;
            //DirectionFire.z = 1;

            bulletObject.GetComponent<Ammo>().FireBall(DirectionFire);
            settings.ammo -= 1;
            project = false;
            updateAmmo();
            audioSource.PlayOneShot(fireClip);
        }


        StartCoroutine(CheckScore());
      
    }

    IEnumerator CheckScore()
    {
        ScoreText.text = settings.Score.ToString();
        yield return new WaitForSeconds(2.0f);
    }



    // trajectory here
   

    public void SimulateTrajectory()
    {
        lr.positionCount = lq;
        GameObject obj = Instantiate(Projectile, FireDirection.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(obj, extraScene);
        rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(FireDirection.forward * settings.MagnitudeFactor * settings.FireForce, ForceMode.Impulse);

        for (int i = 0; i < lq; i++)
        {
            PhysicsScene.Simulate(Time.fixedDeltaTime);
            lr.SetPosition(i, obj.transform.position);
        }

        Destroy(obj);

    }

    public void FixedUpdate()
    {
        // All of the non-default physics scenes need to be simulated manually
        var physicsScene = extraScene.GetPhysicsScene();
        {
            var autoSimulation = Physics.autoSimulation;
            Physics.autoSimulation = false;
            Physics.autoSimulation = autoSimulation;
        }
        PhysicsScene = physicsScene;
    }




    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(worldCordinates, someCoordinates);
    }

    private void Start()
    {
        extraScene = SceneManager.CreateScene("Scene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        Assert.IsNotNull(raycastManager, "RayCast Manager has not been assigned");
        Assert.IsNotNull(planeManager, "Plane Manager has not been assigned");

        settings.Score = 0;
        settings.ammo = 5;
   
        updateAmmo();
        SubStart();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += FingerMove;
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += FingerUp;   
        _camera = Camera.main; 
    
    }


    //private void FingerDown(Finger finger)
    //{
    //   // DebugText.text = "Finger Down";
    //    lineRenderer.positionCount = 3;
        
    //    Vector3 temp = new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0);
    //   // DebugText.text = "Finger Down : " + finger.screenPosition;

    //   temp = _camera.ScreenToWorldPoint(finger.screenPosition);
    //   // temp.z = 0;
       
    //   lineRenderer.SetPosition(0, temp);
    //}

    //private void FingerMove(Finger finger)
    //{
    //   // DebugText.text = "Finger Moved";
    //    Vector3 temp = new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0);

    //   // DebugText.text = "Finger Moved :" + finger.screenPosition;
    //    temp = _camera.ScreenToWorldPoint(finger.screenPosition);
    //    //  temp.z = 0;
    //    lineRenderer.SetPosition(1, temp);
    //}

    //private void FingerUp(Finger finger)
    //{
    //   // DebugText.text = "Finger Up ";
    //    Vector3 temp = new Vector3(finger.screenPosition.x, finger.screenPosition.y, 0);
    //   // DebugText.text = "Finger Up :" + finger.screenPosition;
    //    temp = _camera.ScreenToWorldPoint(finger.screenPosition);
    //    lineRenderer.SetPosition(2, temp);
    //}

    public void updateAmmo()
    {
        for (int i = 0; i < settings.ammo; i++)
        {
            sprites[i].SetActive(true);
        }

        for(int i = 5 - settings.ammo; i > 0; i--)
        {
            sprites[i].SetActive(false);
        }
    }

    public void Update()
    {

        //if (project)
        //{
        //    SimulateTrajectory();
        //}

        SimulateTrajectory();

        if(settings.Score >= 5)
        {
            RestartBtn.SetActive(true);
            StartBtn.SetActive(false);
            settings.Score = 0;
        }
    }

    public void StartActualGame()
    {
        settings.gameStarted = true;
       // StartBtn.SetActive(false);
    }
    public void HalfRestart()
    {
        // reset score points
        // reset fire balls number

        settings.Score = 0;
        settings.ammo = 5;
        RestartBtn.SetActive(false) ;
        settings.gameStarted = false;

        // call triggers to ensure updates run through

        ScoreText.text = settings.Score.ToString();
        updateAmmo();
    }

    public void RestartApplication() {
        settings.ammo = 5;
        settings.gameStarted = false;
        settings.Score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
       

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

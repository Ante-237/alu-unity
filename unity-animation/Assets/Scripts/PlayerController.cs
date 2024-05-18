using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
   // [SerializeField] private float JumpForce = 300f;
   // [SerializeField] private float MovementSpeed = 20f;
    [SerializeField] private Transform cameraPosition;
    private Vector3 vertical;
    private Vector3 horizontal;
    private float directionX;
    private float directionZ;
    private Vector3 InitialPosition;


   // public CharacterController controller;
    private void Awake()
    {
        InitialPosition = transform.position;
    }

    private void Start()
    {
      //  rb = GetComponent<Rigidbody>();
    //    controller = GetComponent<CharacterController>();

    }

    void Update()
    {
       // Movements();
       // Jumping();
        CheckFalling();
    }

/// <summary>
/// if player is falling get change the position to initial Position
/// </summary>
    void CheckFalling()
    {
        Vector3 spaceGap = new Vector3(0, 3, 0);
        if (transform.position.y < -15f)
        {
            transform.position = InitialPosition +  spaceGap;
        }
    }

    /*
    
/// <summary>
/// player jumps when space is pressed
/// </summary>
    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * JumpForce);    
        }
       
    }

/// <summary>
/// hanles vertical and horizontal movements of player
/// </summary>
    void Movements()
    {
        directionX = Input.GetAxisRaw("Horizontal");
        directionZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraPosition.forward;
        Vector3 right = cameraPosition.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        //controller.Move(new Vector3(directionX, 0, directionZ));

        Vector3 movements = new Vector3(directionX, 0, directionZ);//* (Time.deltaTime * MovementSpeed);
        //transform.position += movements;
        rb.AddForce( movements * (Time.deltaTime * MovementSpeed));
    }
    */
}

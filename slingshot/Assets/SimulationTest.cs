using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationTest : MonoBehaviour
{
    private Scene extraScene;
    [SerializeField] private Transform FireDirection;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private int lq = 50;
    [SerializeField] private GameObject Projectile;


    private Rigidbody rb;
    private PhysicsScene PhysicsScene;

    public void Start()
    {
        // First create an extra scene with local physics
        extraScene = SceneManager.CreateScene("Scene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));

        // Mark the scene active, so that all the new GameObjects end up in the newly created scene
        //SceneManager.SetActiveScene(extraScene);

      //  PopulateExtraSceneWithObjects();
    }

    private void Update()
    {
        SimulateTrajectory();
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

    public void SimulateTrajectory()
    {

        lr.positionCount = lq;
        GameObject obj = Instantiate(Projectile, FireDirection.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(obj, extraScene);
        rb = obj.GetComponent<Rigidbody>();
        rb.AddForce(FireDirection.forward * 5, ForceMode.Impulse);



        for (int i = 0; i < lq; i++)
        {
            PhysicsScene.Simulate(Time.fixedDeltaTime);
            lr.SetPosition(i, obj.transform.position);
        }

        Destroy(obj);

    }

    //public void PopulateExtraSceneWithObjects()
    //{
    //    // Create GameObjects for physics simulation
    //    var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    //    SceneManager.MoveGameObjectToScene(sphere, extraScene);
    //    sphere.AddComponent<Rigidbody>();

    //    Rigidbody rb = sphere.GetComponent<Rigidbody>();
       
    //    sphere.transform.position = Vector3.up * 4;
    //}
}

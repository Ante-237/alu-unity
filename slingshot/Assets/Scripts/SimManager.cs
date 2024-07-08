using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SimManager : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 100;
    [SerializeField] private Transform _obstaclesParent;
    [SerializeField] private GameObject Ball;
    [SerializeField] private Transform SpawnPoint;

    
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();
    InputAction fireAction;
    PlayerInput _playerInput;

    private void Start()
    {
        CreatePhysicsScene();
        _playerInput = GetComponent<PlayerInput>();
      
        
    }

    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        //foreach (Transform obj in _obstaclesParent)
        //{
        //    var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
        //    ghostObj.GetComponent<Renderer>().enabled = false;
        //    SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
        //    if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        //}
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        SimulateTrajectory(Ball.GetComponent<Ball>(), SpawnPoint.position);
    }
   

    private void Update()
    {
        foreach (var item in _spawnedObjects)
        {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }

    }

    public void SimulateTrajectory(Ball ballPrefab, Vector3 pos)
    {
        var ghostObj = Instantiate(ballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);
        
        

        _line.positionCount = _maxPhysicsFrameIterations;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }
}

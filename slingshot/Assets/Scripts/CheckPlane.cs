using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CheckPlane : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentStateText;
    [SerializeField] private GameObject Target;
    [SerializeField] private int runNumber = 4;
    [SerializeField] private Button StartBtn;

    Vector3 firstVector = new Vector3(-1, 0, 1);
    Vector3 secondVector = new Vector3(0, 1, 0);

    private bool ObjectCreated = false;
    private List<Vector2> randomPositions = new List<Vector2>();

    private ARPlane mainPlane;
    ARPlaneManager planeManager;
    ARRaycastManager raycastManager;


    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
    NativeArray<Vector2> planeLocations = new NativeArray<Vector2>();

    private void Start()
    {
        float dotProduct = Vector3.Dot(firstVector, secondVector);
        Debug.Log(dotProduct);
        SubscribeToPlanesChanged();
        raycastManager = GetComponent<ARRaycastManager>();
        Invoke(nameof(updateCount), 30);
        StartBtn.onClick.AddListener(StartSpawnSequence);
    }

    private void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }
    }

    void SubscribeToPlanesChanged()
    {
        // This is inefficient. You should re-use a saved reference instead.
        var manager = Object.FindObjectOfType<ARPlaneManager>();
        manager.planesChanged += OnPlanesChanged;   
        planeManager = manager;  
    }

    void HandleRaycast(ARRaycastHit hit)
    {
        if(hit.trackable is ARPlane plane){
            // Do something with 'plane';       
        }
    }

    public void updateCount()
    {
      CurrentStateText.text = planeLocations.Length.ToString();
    }

    public void OnPlanesChanged(ARPlanesChangedEventArgs changes)
    {
        foreach (var plane in changes.added)
        {
            // handle added planes
            if (!ObjectCreated)
            {
                mainPlane = planeManager.GetPlane(plane.trackableId);
                planeLocations = mainPlane.boundary;
                //  CurrentStateText.text = plane.name + ": Added";
                StartSpawnSequence();
                CurrentStateText.text = planeLocations.Length.ToString();
                ObjectCreated = true;
                break;
            }
            // CurrentStateText.text = plane.boundary.Length.ToString();

           plane.gameObject.SetActive(false);
        }

        /*
        //foreach (var plane in changes.updated)
        //{
        //    // handle updated planes
        //    CurrentStateText.text = plane.name + ": Updated";
        //}

        //foreach (var plane in changes.removed)
        //{
        //    // handle removed planes
        //    CurrentStateText.text = plane.name + ": Removed";
        //}
        */
    }

    private void StartSpawnSequence()
    {
        for (int i = 0; i < runNumber; i++)
        {
            var tempObj = Instantiate(Target, new Vector3(planeLocations[i].x, 0, planeLocations[i].y), Quaternion.identity);
            tempObj.GetComponent<randomMove>().SetMotion(planeLocations);
            // Debug.Log("X :" + planeLocations[i].x + " Y : " + planeLocations[i].y);
        }
    }

}

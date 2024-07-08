using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

public class randomMove : MonoBehaviour
{
    public float timeDifference = 5.0f;
    public float speedRun = 0.5f;
    private float currentTime = 0;

    List<Vector3> currentData = new List<Vector3>();
    // = new NativeArray<Vector2>();

    private bool startMovements = false;

    public void SetMotion(NativeArray<Vector2> locations)
    {
        foreach(Vector2 location in locations)
        {
            currentData.Add(location);
        }
        
        startMovements = true;
    }

    private int currentIndex = 0;

    private void Update()
    {
        if (startMovements)
        {
            // transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentData[currentIndex].x, 0f, currentData[currentIndex].y), speedRun * Time.deltaTime);
            transform.position = Vector3.Slerp(transform.position, new Vector3(currentData[currentIndex].x * 2, 0f, currentData[currentIndex].y * 2), speedRun * Time.deltaTime);
        }

        if(currentTime > timeDifference)
        {
            if(currentIndex < currentData.Count)
            {
                currentIndex = Random.Range(0, currentData.Count);
            }
            else
            {
                currentIndex = 0;
            }
            currentTime = 0;
        }
        currentTime += Time.deltaTime; 
    }
}

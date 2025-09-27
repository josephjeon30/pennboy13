using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDraw : MonoBehaviour
{
    Coroutine drawing;
    LineRenderer line;
    Vector3 start;
    Vector3 end;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            StartLine();
        }
        if(Input.GetMouseButtonUp(0)) {
            FinishLine();
        }
    }

    void StartLine()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
        start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void FinishLine()
    {
        StopCoroutine(drawing);
        bool isStraight = true;
        Vector3[] linePoints = new Vector3[line.positionCount];
        line.GetPositions(linePoints);

        // float avgSlope = -1;
        // if (linePoints.Length > 1)
        // {
        //     avgSlope = (linePoints[1].y - linePoints[0].y) / (linePoints[1].x - linePoints[0].x);
        // }
        for (int i = 0; i < linePoints.Length - 7; i+=6)
        {

            // float newSlope = (linePoints[i + 1].y - linePoints[i].y) / (linePoints[i + 1].x - linePoints[i].x);
            // if (Mathf.Abs((avgSlope - newSlope)/avgSlope) > .2)
            // {
            //     Debug.Log(Mathf.Abs((avgSlope - newSlope)/avgSlope));
            //     isStraight = false; 
            // }
            Vector3 p1 = linePoints[i];
            Vector3 p2 = linePoints[i + 3];
            Vector3 p3 = linePoints[i + 6];

            // Calculate vectors
            Vector3 v1 = p2 - p1;
            Vector3 v2 = p3 - p1;

            // Calculate the cross product magnitude
            // If points are collinear, the magnitude of the cross product will be zero
            float crossProductMagnitude = Vector3.Cross(v1, v2).magnitude;

            // Use a small tolerance for floating-point comparisons
            if (isStraight && crossProductMagnitude > 0.8f) // Adjust tolerance as needed
            {
                Debug.Log(crossProductMagnitude);
                isStraight = false; // The line is not straight
            }
        }
        if (isStraight)
        {
            Debug.Log("STRAIGHT!! ");
        }

        end = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float slope = (end.y - start.y) / (end.x - start.x);
        if (slope < 0.5 && slope > -0.5)
        {
            // PUT EVENT CALLER HERE FOR HORIZONTAL
            Debug.Log("HORIZONTAL!! ");
        }
        Destroy(line);
    }

    IEnumerator DrawLine() {
        GameObject newGameObject = Instantiate(Resources.Load("Line") as GameObject, new Vector3(0,0,0), Quaternion.identity);
        //LineRenderer line = newGameObject.GetComponent<LineRenderer>();
        line = newGameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        //line.material.SetColor("_Color", Color.blue);

        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, position);
            yield return null;
        }
    }
}

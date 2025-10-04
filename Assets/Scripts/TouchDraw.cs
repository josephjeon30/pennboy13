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

    bool checkStraight()
    {
        bool isStraight = true;
        double averageSlope = (end.y - start.y) / (end.x - start.x);

        Vector3[] linePoints = new Vector3[line.positionCount];
        line.GetPositions(linePoints);
        for (int i = 0; i < linePoints.Length - 3; i += 3)
        {
            Vector3 p1 = linePoints[i];
            Vector3 p2 = linePoints[i + 3];
            double currSlope = (p2.y - p1.y) / (p2.x - p1.x);
            if (System.Math.Abs((averageSlope - currSlope) / averageSlope) > 100)
            {
                isStraight = false;
            }
        }
        return isStraight;
    }

    double getSlope(double x1, double y1, double x2, double y2)
    {
        if (end.x == start.x)
        {
            if (end.y - start.y >= 0)
            {
                return Double.PositiveInfinity;
            }
            else
            {
                return Double.NegativeInfinity;
            }
        }

        else
        {
            return (y2- y1) / (x2- x1);
        }
        //return (y2- y1) / (x2- x1);
    }


    string checkLineTypeSimplified()
    {
        string lineType = "INVALID";
        double averageSlope = getSlope(start.x, start.y, end.x, end.y);
        double upperSlopeBound = 0.0;
        double lowerSlopeBound = 0.0;

        // vertical line
        if (averageSlope >= 8.0 || averageSlope <= -8.0)
        {
            upperSlopeBound = 7.0;
            lowerSlopeBound = 7.0;
            lineType = "VERTICAL";
        }
        //diagonal up
        else if (averageSlope < 8.0 && averageSlope > 0.15)
        {
            upperSlopeBound = 10.0;
            lowerSlopeBound = .02;
            lineType = "DIAGONAL_UP";
        }
        // diagonal down
        else if (averageSlope > -8.0 && averageSlope < -0.15)
        {
            upperSlopeBound = -.02;
            lowerSlopeBound = -10.0;
            lineType = "DIAGONAL_DOWN";
        }
        //horizontal
        else
        {
            upperSlopeBound = .3;
            lowerSlopeBound = -.3;
            lineType = "HORIZONTAL";
        }

        Vector3[] linePoints = new Vector3[line.positionCount];
        line.GetPositions(linePoints);
        int sampleSize = linePoints.Length / 7;
        Debug.Log("AVG SLOPE");
        Debug.Log(averageSlope);
        for (int i = 0; i < linePoints.Length - sampleSize; i += sampleSize)
        {
            Vector3 p1 = linePoints[i];
            Vector3 p2 = linePoints[i + sampleSize];
            double currSlope = getSlope(p1.x, p1.y, p2.x, p2.y);
            Debug.Log(currSlope);
            if (lineType == "VERTICAL" && System.Math.Abs(currSlope) < lowerSlopeBound)
            {
                Debug.Log("NOT straight ");
                Debug.Log(p1);
                Debug.Log(p2);
                Debug.Log(currSlope);
                lineType = "INVALID";
            }
            else if (lineType != "VERTICAL" && (currSlope > upperSlopeBound || currSlope < lowerSlopeBound))
            {
                Debug.Log("NOT straight ");
                Debug.Log(p1);
                Debug.Log(p2);
                Debug.Log(currSlope);
                lineType = "INVALID";
            }
        }
        return lineType;
    }
    void FinishLine()
    {
        StopCoroutine(drawing);
        end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        string lineType = checkLineTypeSimplified();
        // bool isStraight = true;
        // Vector3[] linePoints = new Vector3[line.positionCount];
        // line.GetPositions(linePoints);

        // // double avgSlope = -1;
        // // if (linePoints.Length > 1)
        // // {
        // //     avgSlope = (linePoints[1].y - linePoints[0].y) / (linePoints[1].x - linePoints[0].x);
        // // }
        // for (int i = 0; i < linePoints.Length - 7; i += 6)
        // {

        //     // double newSlope = (linePoints[i + 1].y - linePoints[i].y) / (linePoints[i + 1].x - linePoints[i].x);
        //     // if (Mathf.Abs((avgSlope - newSlope)/avgSlope) > .2)
        //     // {
        //     //     Debug.Log(Mathf.Abs((avgSlope - newSlope)/avgSlope));
        //     //     isStraight = false; 
        //     // }
        //     Vector3 p1 = linePoints[i];
        //     Vector3 p2 = linePoints[i + 3];
        //     Vector3 p3 = linePoints[i + 6];

        //     // Calculate vectors
        //     Vector3 v1 = p2 - p1;
        //     Vector3 v2 = p3 - p1;

        //     // Calculate the cross product magnitude
        //     // If points are collinear, the magnitude of the cross product will be zero
        //     double crossProductMagnitude = Vector3.Cross(v1, v2).magnitude;

        //     // Use a small tolerance for doubleing-point comparisons
        //     if (isStraight && crossProductMagnitude > 0.8f) // Adjust tolerance as needed
        //     {
        //         Debug.Log(crossProductMagnitude);
        //         isStraight = false; // The line is not straight
        //     }
        // }
        // if (isStraight)
        // {
        //     Debug.Log(lineType);
        // }
        Debug.Log(lineType);
        
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

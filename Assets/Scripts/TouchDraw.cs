using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchDraw : MonoBehaviour
{
    public static event UnityAction swipeForward;
    public static event UnityAction swipeBack;
    

    Coroutine drawing;
    LineRenderer line;
    Vector3 start;
    Vector3 end;
    bool debugMode = false;

    // Color c1 = new Color(0.5f, 0.9f, 1, 1);
    // Color c2 = new Color(0.5f, 0.9f, 1, 0);
    Color c1 = new Color(1, 0.54f, 0.12f, 1);
    Color c2 = new Color(1, 0.54f, 0.12f, 0.3f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLine()
    {
        if (drawing != null)
        {
            StopCoroutine(drawing);
        }
        drawing = StartCoroutine(DrawLine());
        start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            if (end.y > start.y)
            {
                lineType = "UP";
                swipeBack?.Invoke();
            }
            else
            {
                lineType = "DOWN";
                swipeForward?.Invoke();
            }

            
        }
        //diagonal up
        else if (averageSlope < 8.0 && averageSlope > 0.15)
        {
            upperSlopeBound = 10.0;
            lowerSlopeBound = .02;
            if (end.y > start.y)
            {
                lineType = "UPRIGHT";
                swipeBack?.Invoke();
            }
            else
            {
                lineType = "DOWNLEFT";
                swipeForward?.Invoke();
            }
        }
        // diagonal down
        else if (averageSlope > -8.0 && averageSlope < -0.15)
        {
            upperSlopeBound = -.02;
            lowerSlopeBound = -10.0;
            if (end.y > start.y)
            {
                lineType = "UPLEFT";
                swipeBack?.Invoke();
                
            }
            else
            {
                lineType = "DOWNRIGHT";
                swipeForward?.Invoke();
            }
        }
        //horizontal
        else
        {
            upperSlopeBound = .3;
            lowerSlopeBound = -.3;

            if (end.x > start.x)
            {
                lineType = "RIGHT";
                swipeForward?.Invoke();
            }
            else
            {
                lineType = "LEFT";
                swipeBack?.Invoke();
            }
           
        }

        Vector3[] linePoints = new Vector3[line.positionCount];
        line.GetPositions(linePoints);
        int sampleSize = linePoints.Length / 7;
        if (debugMode)
        {
            Debug.Log("AVG SLOPE");
            Debug.Log(averageSlope);
        }
        for (int i = 0; i < linePoints.Length - sampleSize; i += sampleSize)
        {
            Vector3 p1 = linePoints[i];
            Vector3 p2 = linePoints[i + sampleSize];
            double currSlope = getSlope(p1.x, p1.y, p2.x, p2.y);

            if (debugMode)
            {
                Debug.Log(currSlope);
            }

            if ((lineType == "UP" || lineType == "DOWN") && System.Math.Abs(currSlope) < lowerSlopeBound)
            {
                if (debugMode)
                {
                    Debug.Log("NOT straight ");
                    Debug.Log(p1);
                    Debug.Log(p2);
                    Debug.Log(currSlope);
                }
                lineType = "INVALID";
            }
            else if (lineType != "VERTICAL" && (currSlope > upperSlopeBound || currSlope < lowerSlopeBound))
            {
                if (debugMode)
                {
                    Debug.Log("NOT straight ");
                    Debug.Log(p1);
                    Debug.Log(p2);
                    Debug.Log(currSlope);
                }
                lineType = "INVALID";
            }
        }
        return lineType;
    }
    public string FinishLine()
    {
        StopCoroutine(drawing);
        end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        string lineType = checkLineTypeSimplified();

        if (debugMode)
        {
            Debug.Log(lineType);
        }

        Destroy(line.gameObject);
        return lineType;
    }

    IEnumerator DrawLine() {
        GameObject newGameObject = Instantiate(Resources.Load("Line") as GameObject, new Vector3(0,0,0), Quaternion.identity);
        //LineRenderer line = newGameObject.GetComponent<LineRenderer>();
        line = newGameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        //line.material.SetColor("_Color", Color.blue);
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.SetColors(c1, c2);
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

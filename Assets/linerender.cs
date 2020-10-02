using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linerender : MonoBehaviour
{
    public GameObject Line;

    public drawLine activeLine;
    public GameObject Mesh1;
    [Header("Polygon Settings")]
    public int vertexNumber;
    public float radius;
    public Vector3 centerPos;
    public float startWidth;
    public float endWidth;
    public bool polygon;

    void Start()
    {
        if (polygon)
        {
            GameObject newline = Instantiate(Line);
            activeLine = newline.GetComponent<drawLine>();
            activeLine.DrawPolygon(vertexNumber, radius, centerPos, startWidth, endWidth, Mesh1);
        }
        else
        {
            // activeLine.DrawGridShape(vertexNumber, radius, centerPos, startWidth, endWidth, Mesh1);
        }

    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //         GameObject newline = Instantiate(Line);
    //         activeLine = newline.GetComponent<drawLine>();
    //     }
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         Mesh1.GetComponent<Polygon>().setVertices(activeLine.points, activeLine.direction);
    //         activeLine = null;
    //     }
    //     if (activeLine != null)
    //     {
    //         Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         activeLine.UpdateLine(mousePos);
    //     }
    // }


}

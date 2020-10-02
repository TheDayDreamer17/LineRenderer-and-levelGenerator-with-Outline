using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GridGenerator;

public class drawLine : MonoBehaviour
{
    LineRenderer LineRenderer;
    public List<Vector2> points = new List<Vector2>();
    public int direction;
    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.positionCount = 0;

    }
    public void UpdateLine(Vector2 point)
    {
        if (points.Count == 0)
        {
            SetPoints(point);
            return;
        }
        /*if ((point.x - points.Last().x) > 0.1f)
        {
            SetPoints(point);
        }*/
        if (Vector2.Distance(point, points.Last()) > 0.1f)
        {
            SetPoints(point);
            //return;
        }
        if (points.Count == 1)
        {
            if (point.x - points[0].x > 0)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
        }
    }



    void SetPoints(Vector2 point)
    {
        points.Add(point);
        LineRenderer.positionCount = points.Count;
        LineRenderer.SetPosition(points.Count - 1, point);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector2 item in points)
        {
            Gizmos.DrawSphere(item, 0.1f);
        }
    }
    public void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, GameObject mesh)
    {
        LineRenderer.startWidth = startWidth;
        LineRenderer.endWidth = endWidth;
        LineRenderer.loop = true;
        float angle = 2 * Mathf.PI / vertexNumber;
        LineRenderer.positionCount = vertexNumber;

        for (int i = 0; i < vertexNumber; i++)
        {
            Matrix4x4 rotationMatrix = new Matrix4x4(new Vector4(Mathf.Cos(angle * i), Mathf.Sin(angle * i), 0, 0),
                                                     new Vector4(-1 * Mathf.Sin(angle * i), Mathf.Cos(angle * i), 0, 0),
                                       new Vector4(0, 0, 1, 0),
                                       new Vector4(0, 0, 0, 1));
            Vector3 initialRelativePosition = new Vector3(0, radius, 0);
            points.Add(centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));
            // Debug.Log(points.Count);
            LineRenderer.SetPosition(i, centerPos + rotationMatrix.MultiplyPoint(initialRelativePosition));

        }
        // Debug.Log(points.Count);
        mesh.GetComponent<Polygon>().setVertices(points, direction);
    }

    public IEnumerator DrawLineOutline(List<Vector3> linePoints)
    {
        // LineRenderer.positionCount = linePoints.Count;
        LineRenderer.positionCount = 0;
        int index = 0;
        foreach (var item in linePoints)
        {

            // Debug.Log(item);
            LineRenderer.positionCount++;
            LineRenderer.SetPosition(index, item);
            index++;
            yield return new WaitForSeconds(1);

        }
        yield return null;
    }

    internal void DrawOutline(Grid<GameObject> grid, float zPos)
    {

        int index = 0;
        LineRenderer.positionCount = 0;
        float cellSize = grid.GetCellSize();
        int Width = grid.GetWidth();
        int Height = grid.GetHeight();
        int x = 0, y = 0;
        bool custom = false;
        bool outlineNotCreated = true;
        Vector2 currentTileCoOrdinates;
        while (outlineNotCreated)
        {
            if (grid.getCreated(x, y))
            {

                Vector3 tilePosition = grid.GetWorldPosition(x, y);
                currentTileCoOrdinates = new Vector2(x, y);
                LineRenderer.positionCount++;
                //bottom left
                if (y > 0 && grid.getCreated(x, y - 1))
                {
                    Debug.Log("bottom left occurred again");
                }
                else
                {
                    LineRenderer.SetPosition(index, new Vector3(tilePosition.x - cellSize / 2, tilePosition.y - cellSize / 2, zPos));
                }
                //top left
                LineRenderer.SetPosition(index, new Vector3(tilePosition.x - cellSize / 2, tilePosition.y + cellSize / 2, zPos));



            }
            if (y == Height - 1)
            {
                x = x + 1;
                if (grid.getCreated(x, y))
                {
                    custom = true;
                    //do something
                    y = y - 1;
                }
            }
            if (!custom)
            {
                if (y < Height)
                {
                    y++;
                }
                else
                {
                    if (x < Width)
                    { x++; }
                    else
                    {
                        outlineNotCreated = false;
                    }
                }
            }

            // else
            // {
            //     if (y < Height)
            //     {
            //         y++;
            //     }
            //     else
            //     {
            //         if (x < Width)
            //         { x++; }
            //         else
            //         {
            //             x = 0;
            //             y++;
            //         }
            //     }
            // }
        }



    }

    internal void extendLine(Vector3 v)
    {
        // Debug.Log(item);
        LineRenderer.positionCount++;
        LineRenderer.SetPosition(LineRenderer.positionCount - 1, v);

    }
}

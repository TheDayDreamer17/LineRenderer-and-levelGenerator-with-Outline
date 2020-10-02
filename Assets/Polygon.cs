using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    List<Vector2> vertices2DList;
    float cameraHeight;
    float cameraWidth;
    Vector2 BottomLeft, BottomRight;
    public Material material;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = Camera.main.aspect * cameraHeight;
        BottomLeft = new Vector2(Camera.main.transform.position.x - cameraWidth, Camera.main.transform.position.y - cameraHeight);
        BottomRight = new Vector2(Camera.main.transform.position.x + cameraWidth, Camera.main.transform.position.y - cameraHeight);
        vertices2DList = new List<Vector2>();

    }

    // Update is called once per frame
    public void setVertices(List<Vector2> points, int direction)
    {

        /* vertices2DList.Add(BottomLeft);
         vertices2DList.AddRange(point);
         vertices2DList.Add(BottomRight);*/

        // vertices2DList.Add(BottomLeft);
        if (direction == -1)
            points.Reverse();
        //auto complete if needed
        // if (points.First().x >= BottomLeft.x)
        // {
        //     points.Insert(0, new Vector2(BottomLeft.x, points.First().y));
        // }
        // Debug.Log(points.Count);

        vertices2DList.AddRange(points);

        //auto complete if needed
        // if (points.Last().x <= BottomRight.x)
        // {
        //     vertices2DList.Add(new Vector2(BottomRight.x, points.Last().y));
        // }

        // vertices2DList.Add(BottomRight);

        GenerateMesh();

    }
    void GenerateMesh()
    {

        var vertices2d = vertices2DList.ToArray();
        var vertices3d = System.Array.ConvertAll<Vector2, Vector3>(vertices2d, v => v);

        var triangulator = new Triangulator(vertices2d);
        var indices = triangulator.Triangulate();

        var colors = Enumerable.Range(0, vertices3d.Length).Select(i => Random.ColorHSV()).ToArray();

        var Mesh = new Mesh
        {
            vertices = vertices3d,
            triangles = indices,
            colors = colors
        };

        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();

        var meshReneder = gameObject.AddComponent<MeshRenderer>();
        // meshReneder.material = new Material(Shader.Find("Sprites/Default"));
        meshReneder.material = material;
        var filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = Mesh;
    }

}

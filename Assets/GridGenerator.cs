using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EdgeHelpers;
using System.Linq;
public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private float cellSize;
    [SerializeField]
    private Vector3 originPosition;
    [SerializeField]
    private Vector3 gridOffset;
    private Grid<GameObject> grid;
    public GameObject GridSprite;
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public GameObject Line;

    private drawLine activeLine;
    private float offset = 0.04f;
    private List<Vector3> linePoints = new List<Vector3>();
    public float zPos;
    List<VerticeForGrid> verticesofGrid = new List<VerticeForGrid>();
    public GameObject CorrectPointSprite;
    public GameObject WrongPointSprite;
    private int finalCount = 0;
    List<Vector3> finalVertices = new List<Vector3>();
    public bool UnSorted = true;
    bool up = false, right = false, down = false, left = false;
    bool checkCreated = true;
    public SpriteRenderer sp;
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originPosition = new Vector3(-width / 2, -height / 2, 0);
        gridOffset = new Vector3(originPosition.x + 0.5f, originPosition.y + 0.5f, 0);

    }
    void Start()
    {

        grid = new Grid<GameObject>(width, height, cellSize, originPosition, (Grid<GameObject> g, int x, int y) => null, (Grid<GameObject> g, int x, int y) => null);
        //new Grid<GameObject>(width, height, cellSize, originPosition, (Grid<GameObject> g, int x, int y) => Instantiate(GridSprite, new Vector3(x + originPosition.x + cellSize / 2, y + originPosition.y + cellSize / 2, 0), Quaternion.identity), (Grid<GameObject> g, int x, int y) => Instantiate(GridSprite, new Vector3(x + originPosition.x + cellSize / 2, y + originPosition.y + cellSize / 2, 0), Quaternion.identity));
        gridGenerated();
        createTexture();
        printVertices();

        drawlinerenderer();
    }

    private void drawlinerenderer()
    {
        GameObject newline = Instantiate(Line);
        activeLine = newline.GetComponent<drawLine>();
        // activeLine.DrawOutline(grid, zPos);
        // DrawOutline();
        StartCoroutine(SortVertices());
        // SortVertices();
        // StartCoroutine(activeLine.DrawLineOutline(finalVertices));
    }

    private IEnumerator SortVertices()
    {
        // foreach (var item in verticesofGrid)
        // {
        //     if (!item.getUsed())
        //     {
        //         finalVertices.Add(item.vertice);
        //         Debug.Log(item.vertice + " " + item.getUsed());

        //     }
        // }
        // finalVertices.Sort();
        List<Vector3> temp = new List<Vector3>();
        // for (int x = 1; x < temp.Count; x++)
        // {
        //     for (int y = x; y < temp.Count; y++)
        //     {
        //         if (temp[x].x == 1)
        //     }
        // }
        Vector3 v = finalVertices[0];
        Vector3 prev = v;
        Vector3 first = v;
        temp.Add(v);
        int x = 0, y = 0;
        activeLine.extendLine(v);
        // activeLine.extendLine(new Vector3(v.x, v.y + 0.05f));
        float offset = cellSize / 2;

        while (UnSorted)
        {
            // Debug.Log(temp.Exists(e => e.x == v.x && e.y == v.y));
            if (!temp.Exists(e => e.y == v.y + 1 && e.x == v.x) && finalVertices.Exists(e => e.y == v.y + 1 && e.x == v.x))
            {
/*                Debug.Log("prev " + prev.x + " " + prev.y);*/
                up = true;
                // resetBool(1);
                prev = v;
                v = finalVertices.Find(e => e.y == v.y + 1 && e.x == v.x);
                if (temp.Contains(v))
                {
                    UnSorted = false;
                }
                // CheckAndGetIndex(new Vector3((v.x) + cellSize / 2, v.y + (cellSize / 2)), out x, out y);
                // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                /*Debug.Log(v.x + " " + v.y);*/
                temp.Add(v);
                activeLine.extendLine(v);
                if (temp.Count == finalVertices.Count)
                {
                    UnSorted = false;
                }
            }
            else if (grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y - offset), true) && !temp.Exists(e => e.x == v.x + 1 && e.y == v.y) && finalVertices.Exists(e => e.x == v.x + 1 && e.y == v.y))
            {
                // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                // right = true;
                // resetBool();
                Debug.Log("prev " + prev.x + " " + prev.y);
                prev = v;
                v = finalVertices.Find(e => e.x == v.x + 1 && e.y == v.y);

                // CheckAndGetIndex(new Vector3((v.x + cellSize) + cellSize / 2, (v.y) - (cellSize / 2)), out x, out y);
                // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                activeLine.extendLine(v);
                if (temp.Count == finalVertices.Count)
                {
                    UnSorted = false;
                }
            }
            else if (grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y - offset), true) && !grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y - offset), false) && !temp.Exists(e => e.y == v.y - 1 && e.x == v.x) && finalVertices.Exists(e => e.y == v.y - 1 && e.x == v.x))
            {
                // ((x < width && y < height) ? !grid.getCreated(x, y) : true)
                // down = true;
                // resetBool();
                Debug.Log("prev " + prev.x + " " + prev.y);
                prev = v;
                v = finalVertices.Find(e => e.y == v.y - 1 && e.x == v.x);
                if (temp.Contains(v))
                {
                    UnSorted = false;
                }
                // CheckAndGetIndex(v, out x, out y);
                grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                activeLine.extendLine(v);
                if (temp.Count == finalVertices.Count)
                {
                    UnSorted = false;
                }
            }

            else if (!grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y - offset), false) && (!temp.Exists(e => e.x == v.x - 1 && e.y == v.y)) && finalVertices.Exists(e => e.x == v.x - 1 && e.y == v.y))
            {
                // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                // left = true;
                // resetBool();
                Debug.Log("prev " + prev.x + " " + prev.y);
                prev = v;
                v = finalVertices.Find(e => e.x == v.x - 1 && e.y == v.y);
                if (temp.Contains(v))
                {
                    UnSorted = false;
                }
                // CheckAndGetIndex(v, out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                activeLine.extendLine(v);
                if (temp.Count == finalVertices.Count)
                {
                    UnSorted = false;
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
        activeLine.extendLine(first);
        // activeLine.extendLine(new Vector3(first.x, first.y + 0.05f));
        yield return null;
        // finalVertices.Sort(delegate (Vector3 x, Vector3 y)
        // {
        //     if ((x.y - y.y == 1 && x.x == y.x) || (x.x - y.x == 1 && x.y == y.y)) return 1;
        //     else if ((Mathf.Abs(x.x - y.x) + Mathf.Abs(x.y - y.y)) == 1)
        //         return 0;
        //     else
        //         return 0;
        //         // return x.x.CompareTo(y.x);
        //     }); ;
    }
    void createTexture()
    {
        Texture2D tex = new Texture2D(width, height);
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                tex.SetPixel(x, y, !grid.getCreated(x, y) ? Color.clear : Color.black);
            }
        }

        tex.Apply();
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;

        sp.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), cellSize);
    }
    // void CheckAndGetIndex(Vector3 v, out int a, out int b)
    // {
    //     if (grid.CheckIfIndexPossible(v))
    //     {
    //         Vector2 x = grid.getXYPosition(v);
    //         a = (int)x.x;
    //         b = (int)x.y;
    //     }
    //     else
    //     {
    //         checkCreated = true;
    //         a = -1;
    //         b = -1;
    //     }
    // }
    // bool CheckAndGet(Vector3 v)
    // {
    //     if (grid.CheckIfIndexPossible(v))
    //     {
    //         Vector2 x = grid.getXYPosition(v);
    //         if()
    //         return grid.getCreated((int)x.x, (int)x.y);
    //     }
    //     else
    //     {
    //         return true;

    //     }
    // }
    void resetBool()
    {
        right = false;
        up = false;
        left = false;
        down = false;
    }
    void findVertices()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.getCreated(x, y))
                {
                    Vector3 tilePosition = grid.GetWorldPosition(x, y);
                    //bottom left
                    AddVertice(tilePosition, -1, -1);
                    //top left
                    AddVertice(tilePosition, -1, 1);
                    // if (y + 1 < height grid.getCreated(x, y + 1))
                    // {
                    //bottom right

                    AddVertice(tilePosition, 1, -1);
                    //top right
                    AddVertice(tilePosition, 1, 1);

                }
            }
        }
    }
    void AddVertice(Vector3 tilePosition, int x, int y)
    {
        // verticesofGrid.Add(new Vector3(tilePosition.x + (x * cellSize) / 2, tilePosition.y + (y * cellSize) / 2, zPos));
    }
    private void DrawOutline()
    {
        Vector3[] vertices = mesh.vertices;
        List<Edge> boundaryPath = EdgeHelpers.GetEdges(mesh.triangles).FindBoundary().SortEdges();
        for (int i = 0; i < boundaryPath.Count; i++)
        {
            Vector3 pos = vertices[boundaryPath[i].v1];
            linePoints.Add(pos);

            // do something with pos
        }
        // linePoints = new List<Vector3>(new HashSet<Vector3>(linePoints));
        linePoints = linePoints.Distinct().ToList();
        // StartCoroutine(activeLine.DrawLineOutline(linePoints));
    }
    void printVertices()
    {
        foreach (var item in verticesofGrid)
        {
            // Debug.Log(item.vertice + " " + item.getUsed());
            if (item.getUsed())
            {

                // Instantiate(WrongPointSprite, new Vector3(item.vertice.x, item.vertice.y, 0), Quaternion.identity);
            }
            else
            {
                finalVertices.Add(item.vertice);
                // Debug.Log(item.vertice + " " + item.getUsed());
                // Instantiate(CorrectPointSprite, new Vector3(item.vertice.x, item.vertice.y, 0), Quaternion.identity);
                finalCount++;
            }
        }
    }
    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    public void gridGenerated()
    {
        MakeDiscreteProceduralGrid();
        // UpdateMesh();
    }
    void MakeDiscreteProceduralGrid()
    {
        // Debug.Log(grid);
        // Debug.Log(grid.getTotalCreatedTiles());
        vertices = new Vector3[grid.getTotalCreatedTiles() * 4];
        triangles = new int[grid.getTotalCreatedTiles() * 6];
        VerticeForGrid vertice, tempVertice;
        int v = 0, t = 0;
        float vertexOffset = cellSize * 0.5f;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid.getCreated(x, y))
                {
                    Vector3 cellOffset = new Vector3(x * cellSize, y * cellSize, 0);
                    vertices[v] = new Vector3(-vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                    vertices[v + 1] = new Vector3(-vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                    vertices[v + 2] = new Vector3(vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                    vertices[v + 3] = new Vector3(vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;

                    for (int i = 0; i < 4; i++)
                    {
                        tempVertice = verticesofGrid.Find(f => f.vertice.x == vertices[v + i].x && f.vertice.y == vertices[v + i].y);
                        if (tempVertice != null)
                        {
                            if (tempVertice.usedCount < 2)
                                tempVertice.IncreaseUseCount();
                            else
                                tempVertice.setUsed();
                        }
                        else
                        {
                            vertice = new VerticeForGrid(vertices[v + i]);
                            verticesofGrid.Add(vertice);

                        }
                    }

                    // linePoints.Add(vertices[v]);
                    // linePoints.Add(vertices[v + 1]);

                    triangles[t] = v;
                    triangles[t + 1] = triangles[t + 4] = v + 1;
                    triangles[t + 2] = triangles[t + 3] = v + 2;
                    triangles[t + 5] = v + 3;

                    v += 4;
                    t += 6;
                }
                // else
                // {
                //     Vector3 cellOffset = new Vector3(x * cellSize, y * cellSize, 0);
                //     Vector3 verticeTemp;
                //     tempVertice = null;
                //     verticeTemp = new Vector3(-vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                //     test(tempVertice, verticeTemp);
                //     verticeTemp = new Vector3(-vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                //     test(tempVertice, verticeTemp);
                //     verticeTemp = new Vector3(vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                //     test(tempVertice, verticeTemp);
                //     verticeTemp = new Vector3(vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                //     test(tempVertice, verticeTemp);
                // }

            }
        }
        setPermanentUnUsed(vertexOffset);
    }

    private void setPermanentUnUsed(float vertexOffset)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!grid.getCreated(x, y))
                {
                    Vector3 cellOffset = new Vector3(x * cellSize, y * cellSize, 0);
                    Vector3 vertice;
                    VerticeForGrid tempVertice = null;
                    vertice = new Vector3(-vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                    test(tempVertice, vertice);
                    vertice = new Vector3(-vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                    test(tempVertice, vertice);
                    vertice = new Vector3(vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                    test(tempVertice, vertice);
                    vertice = new Vector3(vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                    test(tempVertice, vertice);
                }


            }
        }

    }
    void test(VerticeForGrid tempVertice, Vector3 vertice)
    {
        tempVertice = verticesofGrid.Find(f => f.vertice.x == vertice.x && f.vertice.y == vertice.y);
        if (tempVertice != null)
            tempVertice.setPermanentUnUsed();
    }


    // void MakeDiscreteProceduralGrid()
    // {
    //     vertices = new Vector3[width * height * 4];
    //     triangles = new int[width * height * 6];

    //     int v = 0, t = 0;
    //     float vertexOffset = cellSize * 0.5f;
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             Vector3 cellOffset = new Vector3(x * cellSize, y * cellSize, 0);
    //             vertices[v] = new Vector3(-vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
    //             vertices[v + 1] = new Vector3(-vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
    //             vertices[v + 2] = new Vector3(vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
    //             vertices[v + 3] = new Vector3(vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;

    //             triangles[t] = v;
    //             triangles[t + 1] = triangles[t + 4] = v + 1;
    //             triangles[t + 2] = triangles[t + 3] = v + 2;
    //             triangles[t + 5] = v + 3;

    //             v += 4;
    //             t += 6;
    //         }
    //     }

    // }

    public class VerticeForGrid
    {
        public Vector2 vertice;
        private bool used;
        public int usedCount;
        private bool permanentUnUsed;
        public VerticeForGrid(Vector3 vertices)
        {
            vertice = new Vector2(vertices.x, vertices.y);
            used = false;
            permanentUnUsed = false;
            usedCount = 0;
        }
        public void setUsed()
        {
            if (!permanentUnUsed)
                used = true;
        }
        public bool getUsed()
        {
            return used;
        }
        public void setPermanentUnUsed()
        {
            permanentUnUsed = true;
            used = false;
        }

        internal void IncreaseUseCount()
        {
            usedCount++;
        }
    }

}
public enum Direction
{
    up, down, left, right
}

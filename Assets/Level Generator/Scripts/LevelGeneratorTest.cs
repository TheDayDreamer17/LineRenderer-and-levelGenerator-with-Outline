using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelGeneratorTest : MonoBehaviour
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
    List<VerticeForGridWithDivision> verticesofGrid = new List<VerticeForGridWithDivision>();
    // List<VerticeForGridLevelGenerator> verticesofGrid = new List<VerticeForGridLevelGenerator>();
    private Vector3 gridOffset;
    public int divisionCount = 0;
    public Dictionary<int, DivisionType> divisionType = new Dictionary<int, DivisionType>();
    private drawLine activeLine;
    private GridForLevelGenerator<GameObject> grid;

    public GameObject GridSprite;
    public Color currentColor;
    /*public Color[] colors;*/
    public GameObject Line;
    public GameObject CorrectPointSprite;
    public GameObject WrongPointSprite;
    public GameObject CorrectAndWrongSpriteParent;
    public bool UnSorted = true;
    Dictionary<int, List<Vector3>> finalVertices = new Dictionary<int, List<Vector3>>();
    Dictionary<int, List<Vector3>> SortedVertices = new Dictionary<int, List<Vector3>>();
    // List<Vector3> finalVertices = new List<Vector3>();
    private int finalCount = 0;
    int x, y;
    Vector3[] vertices;
    int[] triangles;
    public GameObject SpriteGO;
    public GameObject TextureParent;
    private Dictionary<int, Color> colorsD = new Dictionary<int, Color>();
    private bool divisionUp, divisionDown;
    // Start is called before the first frame update
    void Start()
    {
        /*currentColor = colors[divisionCount - 1];*/
        originPosition = new Vector3(-width / 2, -height / 2, 0);
        gridOffset = new Vector3(originPosition.x + 0.5f, originPosition.y + 0.5f, 0);

        currentColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
        colorsD.Add(divisionCount, currentColor);
        grid = new GridForLevelGenerator<GameObject>(width, height, cellSize, originPosition, (GridForLevelGenerator<GameObject> g, int x, int y) => Instantiate(GridSprite, new Vector3(x + originPosition.x + cellSize / 2, y + originPosition.y + cellSize / 2, 0), Quaternion.identity, this.transform));


    }
    public void save()
    {
        for (int i = 0; i < divisionCount + 1; i++)
        {
            if (grid.checkDivision(i))
                verticesofGrid.Add(new VerticeForGridWithDivision(i));
        }
        // Debug.Log(verticesofGrid[0]);
        gridGenerated();
        createTexture();
        printVertices();



        drawlinerenderer();
    }
    private void drawlinerenderer()
    {


        // activeLine.DrawOutline(grid, zPos);
        // DrawOutline();
        // for (int division = 0; division <= divisionCount; division++)
        // {
        //     UnSorted = true;
        //     if (grid.checkDivision(division))
        //     {
        //         // SortVertices(division);
        //         // createLine(division);
        //     }
        // }
        // removeRedundant();



        for (int i = 0; i < divisionCount; i++)
        {
            // Debug.Log(i);
            if (grid.checkDivision(i))
            {
                // Debug.Log(finalVertices[i].Count);
                for (int j = 0; j < finalVertices[i].Count; j++)
                {
                    CheckIfExistInOtherDivision(i, finalVertices[i][j]);
                }
            }
        }
        StartCoroutine(SortVerticesAndCreateLine());

        // SortVertices();
        // StartCoroutine(activeLine.DrawLineOutline(finalVertices));
    }



    private void SortVertices(int division)
    {

        SortedVertices[division] = new List<Vector3>();
        List<Vector3> temp = new List<Vector3>();

        int x = 0, y = 0;

        // activeLine.extendLine(new Vector3(v.x, v.y + 0.05f));
        float offset = cellSize / 2;

        Vector3 v = finalVertices[division][0];
        Vector3 prev = v;
        Vector3 first = v;
        temp.Add(v);

        SortedVertices[division].Add(v);

        while (UnSorted)
        {
            // Debug.Log(temp.Exists(e => e.x == v.x && e.y == v.y));
            if (!temp.Exists(e => e.y == v.y + 1 && e.x == v.x) && finalVertices[division].Exists(e => e.y == v.y + 1 && e.x == v.x))
            {
                /*                Debug.Log("prev " + prev.x + " " + prev.y);*/
                /*up = true;*/
                // resetBool(1);
                prev = v;
                v = finalVertices[division].Find(e => e.y == v.y + 1 && e.x == v.x);
                if (temp.Contains(v))
                {
                    Debug.Log("unsorted false");
                    UnSorted = false;
                }
                // CheckAndGetIndex(new Vector3((v.x) + cellSize / 2, v.y + (cellSize / 2)), out x, out y);
                // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                SortedVertices[division].Add(v);
                // activeLine.extendLine(v);
                if (temp.Count == finalVertices[division].Count)
                {
                    Debug.Log("unsorted false");
                    UnSorted = false;
                }
            }
            else if (grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y - offset), true, division) && !temp.Exists(e => e.x == v.x + 1 && e.y == v.y) && finalVertices[division].Exists(e => e.x == v.x + 1 && e.y == v.y))
            {
                // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                // right = true;
                // resetBool();
                Debug.Log("prev " + prev.x + " " + prev.y);
                prev = v;
                v = finalVertices[division].Find(e => e.x == v.x + 1 && e.y == v.y);

                // CheckAndGetIndex(new Vector3((v.x + cellSize) + cellSize / 2, (v.y) - (cellSize / 2)), out x, out y);
                // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                SortedVertices[division].Add(v);
                // activeLine.extendLine(v);
                if (temp.Count == finalVertices[division].Count)
                {
                    Debug.Log("unsorted false");
                    UnSorted = false;
                }
            }
            else if (grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y - offset), true, division) && !grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y - offset), false, division) && !temp.Exists(e => e.y == v.y - 1 && e.x == v.x) && finalVertices[division].Exists(e => e.y == v.y - 1 && e.x == v.x))
            {
                // ((x < width && y < height) ? !grid.getCreated(x, y) : true)
                // down = true;
                // resetBool();
                Debug.Log("prev " + prev.x + " " + prev.y);
                prev = v;
                v = finalVertices[division].Find(e => e.y == v.y - 1 && e.x == v.x);
                // if (temp.Contains(v))
                // {
                //     Debug.Log("unsorted false");
                //     UnSorted = false;
                // }
                // CheckAndGetIndex(v, out x, out y);
                grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                SortedVertices[division].Add(v);
                // activeLine.extendLine(v);
                if (temp.Count == finalVertices[division].Count)
                {
                    Debug.Log("unsorted false");
                    UnSorted = false;
                }
            }

            else if (!grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y - offset), false, division) && (!temp.Exists(e => e.x == v.x - 1 && e.y == v.y)) && finalVertices[division].Exists(e => e.x == v.x - 1 && e.y == v.y))
            {
                // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                // left = true;
                // resetBool();
                Debug.Log("prev " + prev.x + " " + prev.y);
                prev = v;
                v = finalVertices[division].Find(e => e.x == v.x - 1 && e.y == v.y);
                // if (temp.Contains(v))
                // {
                //     Debug.Log("unsorted false");
                //     UnSorted = false;
                // }
                // CheckAndGetIndex(v, out x, out y);
                Debug.Log(v.x + " " + v.y);
                temp.Add(v);
                SortedVertices[division].Add(v);
                // activeLine.extendLine(v);
                if (temp.Count == finalVertices[division].Count)
                {
                    Debug.Log("unsorted false");
                    UnSorted = false;
                }
            }



            // activeLine.extendLine(new Vector3(first.x, first.y + 0.05f));
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
        // activeLine.extendLine(first);



    }
    private void createLine(int division)
    {
        GameObject newline = Instantiate(Line);
        activeLine = newline.GetComponent<drawLine>();

        foreach (var item in SortedVertices[division])
        {
            activeLine.extendLine(item);
        }
        activeLine.extendLine(SortedVertices[division][0]);
    }
    private IEnumerator SortVerticesAndCreateLine()
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
        // for (int x = 1; x < temp.Count; x++)
        // {
        //     for (int y = x; y < temp.Count; y++)
        //     {
        //         if (temp[x].x == 1)
        //     }
        // }
        for (int division = 0; division <= divisionCount; division++)
        {
            if (grid.checkDivision(division) && divisionType[division] == DivisionType.Fill)
            {
                Debug.Log(division);

                List<Vector3> temp = new List<Vector3>();
                GameObject newline = Instantiate(Line);
                activeLine = newline.GetComponent<drawLine>();
                int x = 0, y = 0;
                // for (int i = 0; i < finalVertices[division].Count; i++)
                // {
                //     // Debug.LogError(finalVertices[division][i]);
                // }
                // activeLine.extendLine(new Vector3(v.x, v.y + 0.05f));
                float offset = cellSize / 2;

                Vector3 v = finalVertices[division][0];
                Vector3 prev = v;
                Vector3 first = v;
                temp.Add(v);
                activeLine.extendLine(v);
                UnSorted = true;
                while (UnSorted)
                {
                    // Debug.Log(temp.Exists(e => e.x == v.x && e.y == v.y));
                    if (!grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y + offset), false, division) && grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y + offset), true, division) && !temp.Exists(e => e.y == v.y + 1 && e.x == v.x) && finalVertices[division].Exists(e => e.y == v.y + 1 && e.x == v.x))
                    {
                        /*                Debug.Log("prev " + prev.x + " " + prev.y);*/
                        /*up = true;*/
                        // resetBool(1);
                        prev = v;
                        v = finalVertices[division].Find(e => e.y == v.y + 1 && e.x == v.x);
                        if (temp.Contains(v))
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                        // CheckAndGetIndex(new Vector3((v.x) + cellSize / 2, v.y + (cellSize / 2)), out x, out y);
                        // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                        /*Debug.Log(v.x + " " + v.y);*/
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            UnSorted = false;
                        }
                    }
                    else if ((!grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y + offset), false, division) || !grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y + offset), false, division)) && grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y - offset), true, division) && !temp.Exists(e => e.x == v.x + 1 && e.y == v.y) && finalVertices[division].Exists(e => e.x == v.x + 1 && e.y == v.y))
                    {
                        // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                        // right = true;
                        // resetBool();
                        Debug.Log("prev " + prev.x + " " + prev.y);
                        prev = v;
                        v = finalVertices[division].Find(e => e.x == v.x + 1 && e.y == v.y);

                        // CheckAndGetIndex(new Vector3((v.x + cellSize) + cellSize / 2, (v.y) - (cellSize / 2)), out x, out y);
                        // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                    }
                    else if (grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y - offset), true, division) && !grid.CheckIfIndexPossible(new Vector3(v.x + offset, v.y - offset), false, division) && !temp.Exists(e => e.y == v.y - 1 && e.x == v.x) && finalVertices[division].Exists(e => e.y == v.y - 1 && e.x == v.x))
                    {
                        // ((x < width && y < height) ? !grid.getCreated(x, y) : true)
                        // down = true;
                        // resetBool();
                        Debug.Log("prev " + prev.x + " " + prev.y);
                        prev = v;
                        v = finalVertices[division].Find(e => e.y == v.y - 1 && e.x == v.x);
                        if (temp.Contains(v))
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                        // CheckAndGetIndex(v, out x, out y);
                        grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                    }

                    else if (!grid.CheckIfIndexPossible(new Vector3(v.x - offset, v.y - offset), false, division) && (!temp.Exists(e => e.x == v.x - 1 && e.y == v.y)) && finalVertices[division].Exists(e => e.x == v.x - 1 && e.y == v.y))
                    {
                        // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                        // left = true;
                        // resetBool();
                        Debug.Log("prev " + prev.x + " " + prev.y);
                        prev = v;

                        v = finalVertices[division].Find(e => e.x == v.x - 1 && e.y == v.y);
                        // temp.Contains(new Vector3(v.x - 1, v.y, 0))
                        if (new Vector3(v.x - 1, v.y, 0) == first)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                        // CheckAndGetIndex(v, out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                    }
                    yield return new WaitForSeconds(0.01f);


                    // activeLine.extendLine(new Vector3(first.x, first.y + 0.05f));
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
                activeLine.extendLine(first);

            }
            else if (divisionType[division] == DivisionType.Empty)
            {
                Debug.Log("Empty");
                for (int i = 0; i < finalVertices[division].Count; i++)
                {
                    Debug.Log(finalVertices[division][i]);
                }
                List<Vector3> temp = new List<Vector3>();
                GameObject newline = Instantiate(Line);
                activeLine = newline.GetComponent<drawLine>();
                int x = 0, y = 0;

                // activeLine.extendLine(new Vector3(v.x, v.y + 0.05f));
                float offset = cellSize / 2;
                Debug.Log(division);
                Debug.Log(finalVertices[division].Count);
                Vector3 v = finalVertices[division][0];
                Vector3 prev = v;
                Vector3 first = v;
                temp.Add(v);
                activeLine.extendLine(v);
                UnSorted = true;
                while (UnSorted)
                {
                    // Debug.Log(temp.Exists(e => e.x == v.x && e.y == v.y));
                    if (!temp.Exists(e => e.y == v.y + 1 && e.x == v.x) && finalVertices[division].Exists(e => e.y == v.y + 1 && e.x == v.x))
                    {
                        /*                Debug.Log("prev " + prev.x + " " + prev.y);*/
                        /*up = true;*/
                        // resetBool(1);
                        prev = v;
                        v = finalVertices[division].Find(e => e.y == v.y + 1 && e.x == v.x);

                        // CheckAndGetIndex(new Vector3((v.x) + cellSize / 2, v.y + (cellSize / 2)), out x, out y);
                        // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            UnSorted = false;
                        }
                    }
                    else if (!temp.Exists(e => e.x == v.x + 1 && e.y == v.y) && finalVertices[division].Exists(e => e.x == v.x + 1 && e.y == v.y))
                    {
                        // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                        // right = true;
                        // resetBool();
                        Debug.Log("prev " + prev.x + " " + prev.y);
                        prev = v;
                        v = finalVertices[division].Find(e => e.x == v.x + 1 && e.y == v.y);

                        // CheckAndGetIndex(new Vector3((v.x + cellSize) + cellSize / 2, (v.y) - (cellSize / 2)), out x, out y);
                        // grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                    }
                    else if (!temp.Exists(e => e.y == v.y - 1 && e.x == v.x) && finalVertices[division].Exists(e => e.y == v.y - 1 && e.x == v.x))
                    {
                        // ((x < width && y < height) ? !grid.getCreated(x, y) : true)
                        // down = true;
                        // resetBool();
                        Debug.Log("prev " + prev.x + " " + prev.y);
                        prev = v;
                        v = finalVertices[division].Find(e => e.y == v.y - 1 && e.x == v.x);

                        // CheckAndGetIndex(v, out x, out y);
                        grid.getXYPosition(new Vector3((v.x) + cellSize / 2, v.y - (cellSize / 2)), out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                    }

                    else if ((!temp.Exists(e => e.x == v.x - 1 && e.y == v.y)) && finalVertices[division].Exists(e => e.x == v.x - 1 && e.y == v.y))
                    {
                        // ((x < width && y < height) ? grid.getCreated(x, y) : true)
                        // left = true;
                        // resetBool();
                        Debug.Log("prev " + prev.x + " " + prev.y);
                        prev = v;

                        v = finalVertices[division].Find(e => e.x == v.x - 1 && e.y == v.y);
                        // temp.Contains(new Vector3(v.x - 1, v.y, 0))

                        // CheckAndGetIndex(v, out x, out y);
                        Debug.Log(v.x + " " + v.y);
                        temp.Add(v);
                        activeLine.extendLine(v);
                        if (temp.Count == finalVertices[division].Count)
                        {
                            Debug.Log("unsorted false");
                            UnSorted = false;
                        }
                    }
                    yield return new WaitForSeconds(0.01f);

                }
                activeLine.extendLine(first);

            }

        }
        yield return null;
    }
    private void removeRedundantFromPreviousDivision(int currentDivision, Vector3 v)
    {
        for (int i = 0; i < currentDivision; i++)
        {
            if (finalVertices[i].Exists(e => e == v))
            {
                finalVertices[i].Remove(v);
                Debug.LogError("Removed from division " + i + " " + v);
            }
        }
    }
    private void CheckIfExistInOtherDivision(int division, Vector3 v)
    {
        // bool checkedDivision = false;

        for (int i = 0; i < finalVertices.Count; i++)
        {
            // Debug.Log(i + " " + finalVertices[i].Count);
            if (i != division && divisionType[division] == DivisionType.Fill)
            {
                if (finalVertices[i].Exists(e => e == v))
                {
                    finalVertices[division].Remove(v);
                    Debug.LogError("Removed from division " + division + " of d" + i + " " + v);
                }
                else
                {

                }

            }
        }

    }

    void printVertices()
    {

        foreach (var division in verticesofGrid)
        {
            finalVertices[division.divisionNumber] = new List<Vector3>();

            for (int i = 0; i < division.verticesofGrid.Count; i++)
            {

                if (division.verticesofGrid[i].getUsed())
                {
                    Instantiate(WrongPointSprite, new Vector3(division.verticesofGrid[i].vertice.x, division.verticesofGrid[i].vertice.y, 0), Quaternion.identity, CorrectAndWrongSpriteParent.transform);
                }
                else
                {
                    finalVertices[division.divisionNumber].Add(division.verticesofGrid[i].vertice);
                    removeRedundantFromPreviousDivision(division.divisionNumber, division.verticesofGrid[i].vertice);
                    Debug.Log("division " + division.divisionNumber + " " + finalVertices[division.divisionNumber].Count + " " + finalVertices[division.divisionNumber][finalVertices[division.divisionNumber].Count - 1]);

                    // Debug.Log(item.vertice + " " + item.getUsed());
                    Instantiate(CorrectPointSprite, new Vector3(division.verticesofGrid[i].vertice.x, division.verticesofGrid[i].vertice.y, 0), Quaternion.identity, CorrectAndWrongSpriteParent.transform);
                    finalCount++;
                }
            }
            // Debug.Log(item.vertice + " " + item.getUsed());
        }
        Debug.Log(divisionCount);
        for (int i = 0; i <= divisionCount; i++)
        {
            Debug.Log(finalVertices[i].Count);
            for (int j = 0; j < finalVertices[i].Count; j++)
            {
                Debug.Log(finalVertices[i][j]);
            }
        }
    }
    void createTexture()
    {
        Texture2D tex;
        for (int division = 0; division <= divisionCount; division++)
        {
            tex = new Texture2D(width, height);
            if (grid.checkDivision(division) && divisionType[division] == DivisionType.Fill)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    for (int y = 0; y < tex.height; y++)
                    {
                        tex.SetPixel(x, y, !grid.getCreated(division, x, y) ? Color.clear : Color.black);
                    }
                }
                tex.Apply();
                tex.wrapMode = TextureWrapMode.Clamp;
                tex.filterMode = FilterMode.Point;
                GameObject sprite = Instantiate(SpriteGO, TextureParent.transform);

                sprite.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), cellSize);

            }

        }
        // for (int x = 0; x < tex.width; x++)
        // {
        //     for (int y = 0; y < tex.height; y++)
        //     {
        //         tex.SetPixel(x, y, !grid.getCreated(x, y) ? Color.clear : Color.black);
        //     }
        // }


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
        VerticeForGridLevelGenerator vertice, tempVertice;
        int v = 0, t = 0;
        float vertexOffset = cellSize * 0.5f;
        for (int division = 0; division <= divisionCount; division++)
        {
            if (grid.checkDivision(division))
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (grid.getCreated(division, x, y))
                        {
                            Vector3 cellOffset = new Vector3(x * cellSize, y * cellSize, 0);
                            // Debug.Log(v);
                            vertices[v] = new Vector3(-vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                            vertices[v + 1] = new Vector3(-vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                            vertices[v + 2] = new Vector3(vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                            vertices[v + 3] = new Vector3(vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;

                            for (int i = 0; i < 4; i++)
                            {
                                // Debug.Log(verticesofGrid[division]);
                                tempVertice = verticesofGrid[division].verticesofGrid.Find(f => f.vertice.x == vertices[v + i].x && f.vertice.y == vertices[v + i].y);
                                if (tempVertice != null)
                                {
                                    if (tempVertice.usedCount < 2)
                                        tempVertice.IncreaseUseCount();
                                    else
                                        tempVertice.setUsed();
                                }
                                else
                                {
                                    vertice = new VerticeForGridLevelGenerator(vertices[v + i]);
                                    verticesofGrid[division].verticesofGrid.Add(vertice);

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
        }
    }

    private void setPermanentUnUsed(float vertexOffset)
    {
        for (int division = 0; division <= divisionCount; division++)
        {
            if (grid.checkDivision(division))
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (!grid.getCreated(division, x, y))
                        {
                            Vector3 cellOffset = new Vector3(x * cellSize, y * cellSize, 0);
                            Vector3 vertice;
                            VerticeForGridLevelGenerator tempVertice = null;
                            vertice = new Vector3(-vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                            test(division, tempVertice, vertice);
                            vertice = new Vector3(-vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                            test(division, tempVertice, vertice);
                            vertice = new Vector3(vertexOffset, -vertexOffset, 0) + cellOffset + gridOffset;
                            test(division, tempVertice, vertice);
                            vertice = new Vector3(vertexOffset, vertexOffset, 0) + cellOffset + gridOffset;
                            test(division, tempVertice, vertice);
                        }


                    }
                }
            }
        }
    }
    void test(int division, VerticeForGridLevelGenerator tempVertice, Vector3 vertice)
    {
        tempVertice = verticesofGrid[division].verticesofGrid.Find(f => f.vertice.x == vertice.x && f.vertice.y == vertice.y);
        if (tempVertice != null)
            tempVertice.setPermanentUnUsed();
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.F))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            if (grid.GetValue(position) != null)
            {
                /*Debug.Log(position);*/
                grid.getXYPosition(position, out x, out y);
                int tempDivision;
                tempDivision = grid.getCreatedDivision(x, y);
                if (tempDivision != -1)
                    grid.SetCreated(tempDivision, x, y, false, false);

                divisionType[divisionCount] = DivisionType.Fill;
                grid.SetCreated(divisionCount, x, y, true, true);
                GameObject Tile = grid.GetValue(position);
                Tile.GetComponent<SpriteRenderer>().color = currentColor;
                Debug.Log("D" + divisionCount + " F");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            if (grid.GetValue(position) != null)
            {


                GameObject Tile = grid.GetValue(position);
                Tile.GetComponent<SpriteRenderer>().color = Color.white;
                Debug.Log("D" + divisionCount + " E");
            }
        }
        else if (Input.GetKey(KeyCode.O))
        {
            //empty space with outline
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            if (grid.GetValue(position) != null)
            {

                grid.getXYPosition(position, out x, out y);
                int tempDivision;
                tempDivision = grid.getCreatedDivision(x, y);
                if (tempDivision != -1)
                    grid.SetCreated(tempDivision, x, y, false, false);
                divisionType[divisionCount] = DivisionType.Empty;
                grid.SetCreated(divisionCount, x, y, true, true);
                GameObject Tile = grid.GetValue(position);
                Tile.GetComponent<SpriteRenderer>().color = currentColor;
                Debug.Log("D" + divisionCount + " O");
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //increment division
                Debug.Log("division count up");
                divisionCount++;
                grid.addDivisions(divisionCount);
                Color tempcurrentColor;
                colorsD.TryGetValue(divisionCount, out tempcurrentColor);
                Debug.Log(tempcurrentColor);
                if (tempcurrentColor.a != 0)
                    currentColor = tempcurrentColor;
                else
                {
                    currentColor = new Color(Random.Range(0F, 1F), Random.Range(0, 1F), Random.Range(0, 1F));
                    colorsD.Add(divisionCount, currentColor);
                }
                /*currentColor = colors[divisionCount - 1];*/
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //decrement division
                Debug.Log("division count down");
                if (divisionCount > 0)
                {
                    divisionCount--;
                    // Color tempcurrentColor;
                    // colorsD.TryGetValue(divisionCount, out tempcurrentColor);
                    // if (tempcurrentColor.a != 0)
                    //     currentColor = tempcurrentColor;
                    currentColor = colorsD[divisionCount];
                }
            }


        }
        /*else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.D))
        {


        }*/
        /*if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            if (grid.GetValue(position) != null)
            {
                Vector3Int selectedTile = tileMap.WorldToCell(position);
                tileMap.SetTile(selectedTile, currentSelectedTile);
            }
        }*/


    }
    public class VerticeForGridWithDivision
    {
        public int divisionNumber;
        public List<VerticeForGridLevelGenerator> verticesofGrid;

        public VerticeForGridWithDivision(int divisionNumber)
        {
            this.divisionNumber = divisionNumber;
            this.verticesofGrid = new List<VerticeForGridLevelGenerator>();
        }
    }
    public class VerticeForGridLevelGenerator
    {
        public Vector2 vertice;
        private bool used;
        public int usedCount;
        private bool permanentUnUsed;

        public VerticeForGridLevelGenerator(Vector3 vertices)
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
    public class Divisions
    {
        public int divisionNumber;
        public DivisionType divisionType;

        public Divisions(int divisionNumber, DivisionType divisionType)
        {
            this.divisionNumber = divisionNumber;
            this.divisionType = divisionType;
        }

    }
    public enum DivisionType
    {
        Fill,
        Empty
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Grid<TGrid>
{
    private int height;
    private int width;
    private float cellSize;
    private Vector3 originPosition;
    public TGrid[,] gridArray;
    private bool[,] created;
    private int totalCreatedTiles = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGrid>, int, int, TGrid> createGridObject, Func<Grid<TGrid>, int, int, TGrid> alternativeGridObject = null)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        int upperBoundx, upperBoundy, lowerBoundx, lowerBoundy;
        gridArray = new TGrid[width, height];
        created = new bool[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (x == 0 || x == gridArray.GetLength(0) - 1 || y == 0 || y == gridArray.GetLength(1) - 1)
                {



                }
                else
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                    totalCreatedTiles++;
                    created[x, y] = true;
                }

            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    /*debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 7, Color.white, TextAnchor.MiddleCenter);*/
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    if (x == 0 || x == gridArray.GetLength(0) - 1 || y == 0 || y == gridArray.GetLength(1) - 1)
                    {



                        upperBoundx = x + 1 < gridArray.GetLength(0) ? x + 1 : -1;
                        upperBoundy = y + 1 < gridArray.GetLength(1) ? y + 1 : -1;
                        lowerBoundx = x - 1 > 0 ? x - 1 : -1;
                        lowerBoundy = y - 1 > 0 ? y - 1 : -1;

                        if ((upperBoundx != -1 && created[upperBoundx, y] == true) || (upperBoundy != -1 && created[x, upperBoundy] == true) || (lowerBoundx != -1 && created[lowerBoundx, y] == true) || (lowerBoundy != -1 && created[x, lowerBoundy] == true))
                        {
                            int random = UnityEngine.Random.Range(0, 2);
                            if (random == 1)
                            {
                                gridArray[x, y] = alternativeGridObject(this, x, y);
                                totalCreatedTiles++;
                                created[x, y] = true;
                            }
                            else
                            {
                                created[x, y] = false;
                            }
                        }
                        else
                        {
                            created[x, y] = false;
                        }
                    }
                }
            }
            // Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            // Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                /*debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();*/
            };
        }
        // gridGenerated.gridGenerated();
    }

    internal void SetCreated(int x, int y)
    {
        created[x, y] = true;
    }

    public int getTotalCreatedTiles()
    {
        // Debug.Log(totalCreatedTiles);
        return totalCreatedTiles;
    }
    public bool getCreated(int x, int y)
    {
        // Debug.Log("get created" + x + " " + y + " " + created[x, y]);
        return created[x, y];
    }
    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }
    public Vector3 GetWorldPosition(int x, int y)
    {
        /*return new Vector3(x - width / 2, y - height / 2) * cellSize;*/
        /*        Debug.Log(originPosition);*/
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public void getXYPosition(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public Vector2 getXYPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        int y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        return new Vector2(x, y);
    }
    public void SetValue(int x, int y, TGrid value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;

            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }
    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }
    public void SetValue(Vector3 worldPosition, TGrid value)
    {
        int x, y;
        getXYPosition(worldPosition, out x, out y);
        /*Debug.Log((x+width/2)+" "+(y + height / 2));*/
        /*x += width / 2;
        y += height / 2;*/
        SetValue(x, y, value);
    }

    internal bool CheckIfIndexPossible(Vector3 worldPosition, bool defaultValue)
    {
        int x, y;
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        if (x < width && x >= 0 && y < height && y >= 0)
            return getCreated(x, y);
        else
            return defaultValue;

    }

    public TGrid GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];

        }
        else
        {
            return default(TGrid);
        }

    }
    public TGrid GetValue(Vector3 worldPosition)
    {
        int x, y;
        getXYPosition(worldPosition, out x, out y);
        /*Debug.Log((x+width/2)+" "+(y + height / 2));*/
        /*x += width / 2;
        y += height / 2;*/
        return GetValue(x, y);

    }

}

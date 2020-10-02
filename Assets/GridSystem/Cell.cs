using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int xIndex, yIndex;
    public bool isEmpty = true;

    public Cell(int x, int y)
    {
        this.xIndex = x;
        this.yIndex = y;
        isEmpty = true;
    }
}

public static class GridExtension
{
    public static readonly Vector2Int RIGHT = Vector2Int.right;
    public static readonly Vector2Int LEFT = Vector2Int.left;
    public static readonly Vector2Int UP = Vector2Int.up;
    public static readonly Vector2Int DOWN = Vector2Int.down;

    public static Vector2Int[] GetAllAdjacent(this Cell cell)
    {
        return new Vector2Int[] { RIGHT, UP, LEFT, DOWN };
    }

    public static Vector2Int[] GetAll(int level = 1)
    {
        Vector2Int[] dirs = new Vector2Int[(level * 2 + 1) * (level * 2 + 1)];
        int index = 0;
        for (int x = -level; x <= level; x++)
        {
            for (int y = -level; y <= level; y++)
            {
                dirs[index] = new Vector2Int(x, y);
                index++;
            }
        }

        return dirs;
    }


}

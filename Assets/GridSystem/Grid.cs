using System.Collections.Generic;
using UnityEngine;

public class GridBS
{
    public Cell[,] cells;

    private int _width, _height;

    public Cell this[int x, int y] => cells[x, y];

    public bool IsWithinBounds(int x, int y) => x >= 0 && y >= 0 && x < _width && y < _height;

    public GridBS(int width, int height)
    {
        _width = width;
        _height = height;

        cells = new Cell[_width, _height];

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                Cell newCell = new Cell(x, y);
                cells[x, y] = newCell;
            }
        }
    }

    bool IsAdjacentIndex(int x1, int y1, int x2, int y2)
    {
        return (Mathf.Abs(x1 - x2) == 1 && y1 == y2) || (Mathf.Abs(y1 - y2) == 1 && x1 == x2) ? true : false;
    }

    public Cell GetAdjacentCell(Cell target, Vector2Int adjacentDirection)
    {
        int adjX = target.xIndex + adjacentDirection.x;
        int adjY = target.yIndex + adjacentDirection.y;

        return IsWithinBounds(adjX, adjY) && IsAdjacentIndex(target.xIndex, target.yIndex, adjX, adjY) ? cells[adjX, adjY] : null;
    }

    public List<Cell> GetAllEmptyCells()
    {
        List<Cell> emptyCells = new List<Cell>();
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                if (cells[x, y].isEmpty)
                {
                    emptyCells.Add(cells[x, y]);
                }
            }
        }

        return emptyCells;
    }
}

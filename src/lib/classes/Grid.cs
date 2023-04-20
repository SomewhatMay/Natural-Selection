using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Classes.CellObjects;
using Auios.QuadTree;

namespace Classes.GameGrid;

#nullable enable
public delegate bool IterateGridDelegate(int X, int Y, Cell? cell);
public delegate bool IterateExclusiveDelegate(Cell cell);
public delegate Cell FillDelegate(int X, int Y);

public class CellBounds : IQuadTreeObjectBounds<Cell> {
    public float GetBottom(Cell obj) => obj.Bounds.Bottom;
    public float GetTop(Cell obj) => obj.Bounds.Top;
    public float GetLeft(Cell obj) => obj.Bounds.Left;
    public float GetRight(Cell obj) => obj.Bounds.Right;
}

public class Grid {
    public List<Cell> cellList;
    public Cell[,] cellGrid;
    public QuadTree<Cell> quadGrid;

    public Vector2 worldSize;

    public Grid(Vector2 worldSize) {
        this.worldSize = worldSize;

        cellList = new List<Cell>();
        quadGrid = new QuadTree<Cell>(worldSize.X, worldSize.Y, new CellBounds());
        cellGrid = new Cell[(int) worldSize.X, (int) worldSize.Y];
    }

    public void Clear() {
        cellList.Clear();
        Array.Clear(cellGrid);
        quadGrid.Clear();
    }

    public void InsertCell(Cell? cell) {
        // Check if there is already a cell in a specific position
        Cell? currentValue = GetInGrid(cell.Position.X, cell.Position.Y);
        if (currentValue != null) {
            throw new System.Exception("Cannot overwrite a grid field that already has a cell");
        }

        cellList.Add(cell);
        quadGrid.Insert(cell);
        cellGrid[cell.Position.X, cell.Position.Y] = cell;
    }

    public Cell GetInGrid(int X, int Y) {
        return cellGrid[X, Y];
    }

    // Exclusive by deafult
    public void FillGrid(FillDelegate callback) {
        for (int X = 0; X < worldSize.X; ++X) {
            for (int Y = 0; Y < worldSize.Y; ++Y) {
                Cell result = callback(X, Y);

                if (result != null) InsertCell(result);
            }
        }
    }

    public void IterateGrid(IterateGridDelegate callback) {
        for (int X = 0; X < worldSize.X; ++X) {
            for (int Y = 0; Y < worldSize.Y; ++Y) {
                Cell value = cellGrid[X, Y];
                bool result = callback(X, Y, value);

                if (!result) {
                    break;
                }
            }
        }
    }

    public void IterateExclusiveAll(IterateExclusiveDelegate callback) {
        foreach (Cell cell in this.cellList) {
            bool result = callback(cell);

            // if we cannot continue, we break
            if (!result) {
                break;
            }
        }
    }

    public void IterateExclusiveRegion(IterateExclusiveDelegate callback, int x, int y, int width, int height)
        => IterateExclusiveRegion(callback, new QuadTreeRect(x, y, width, height));

    public void IterateExclusiveRegion(IterateExclusiveDelegate callback, QuadTreeRect searchArea) {
        Cell[] cellsInRegion = quadGrid.FindObjects(searchArea);

        foreach (Cell cell in cellsInRegion) {
            bool result = callback(cell);

            if (!result) {
                break;
            }
        }
    }

    public Cell[] Search(int x, int y, int width, int height) {
        QuadTreeRect searchArea = new QuadTreeRect(x, y, width, height);
        return quadGrid.FindObjects(searchArea);
    }
}
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Classes.CellObjects;
using Auios.QuadTree;

namespace Classes.GameGrid;

#nullable enable
public delegate bool IterateGridDelegate(int X, int Y, Cell? cell);
public delegate bool IterateExclusiveDelegate(Cell cell);
public delegate Cell? FillDelegate(int X, int Y);

public class CellBounds : IQuadTreeObjectBounds<Cell> {
    public float GetBottom(Cell obj) => obj.Position.Y;
    public float GetTop(Cell obj) => obj.Position.Y;
    public float GetLeft(Cell obj) => obj.Position.X;
    public float GetRight(Cell obj) => obj.Position.X;
}

public class Grid {
    public List<Cell> cellList;
    public Cell[,] cellGrid;
    public QuadTree<Cell> quadGrid;

    public Point worldSize;

    public Grid(Vector2 worldSize) : this(new Point((int) worldSize.X, (int) worldSize.Y)) { }
    public Grid(Point worldSize) {
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

    public void InsertCell(Cell cell) {
        // Check if there is already a cell in a specific position
        Cell? currentValue = GetInGrid(cell.Position.X, cell.Position.Y);
        if (currentValue != null) {
            throw new System.Exception($"Cannot overwrite a grid field that already has a cell. Current Cell Type:{currentValue.GetType()}. New cell:{cell.GetType()}");
        }

        cellList.Add(cell);
        quadGrid.Insert(cell);
        cellGrid[cell.Position.X, cell.Position.Y] = cell;
    }

    public Cell GetInGrid(Vector2 position) => GetInGrid((int) position.X, (int) position.Y);
    public Cell GetInGrid(Point position) => GetInGrid(position.X, position.Y);
    public Cell GetInGrid(int X, int Y) {
        // try {
        //     var cellGrid[X, Y];
        // } catch (System.IndexOutOfRangeException) {
        //     Console.WriteLine($"Index out of range: ({X}, {Y})");
        // }

        return cellGrid[X, Y];
    }

    // Exclusive by deafult
    public void FillGrid(FillDelegate callback) {
        for (int X = 0; X < worldSize.X; ++X) {
            for (int Y = 0; Y < worldSize.Y; ++Y) {
                Cell? result = callback(X, Y);

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
                //Console.WriteLine("Breaking!");
                break;
            }
        }
    }

    public void IterateExclusiveRegion(int x, int y, int width, int height, IterateExclusiveDelegate callback)
        => IterateExclusiveRegion(new QuadTreeRect((float) x, (float) y, (float) width, (float) height), callback);

    public void IterateExclusiveRegion(QuadTreeRect searchArea, IterateExclusiveDelegate callback) {
        Cell[] cellsInRegion = quadGrid.FindObjects(searchArea);

        //Console.WriteLine($"Got {cellsInRegion.Length} in area");

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
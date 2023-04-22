using Microsoft.Xna.Framework;
using Classes.CellObjects;
using ConstantsNamespace;
using Other;
using System;
using Interfaces;
using Classes.GameGrid;

namespace Actions;

public class FindNearestFood : IAction {
    static Grid gameGrid;

    public FindNearestFood() { }

    public static void Init(Grid _gameGrid) {
        gameGrid = _gameGrid;
    }

    #nullable enable
    public int Invoke(Cell cell, int? _) {
        int direction = 0;

        bool success = Spiral(cell.Position.X, cell.Position.Y, (x, y, xOffset, yOffset) => {
            if (OutOfBounds(x, y)) return false;

            Cell? value = gameGrid.GetInGrid(x, y);

            if (value != null && value is FoodCell) {
                if (Math.Abs(xOffset) > Math.Abs(yOffset)) {
                    direction = xOffset > 0 ? 1 : 3; // 1 is right, 3 is left
                } else {
                    direction = yOffset > 0 ? 0 : 2; // 0 is up, 2 is down
                }

                return true;
            }

            return false;
        });

        // if (!success) {
        //     Console.WriteLine("Unable to find food in radius!");
        // }

        return direction;
    }

    private bool OutOfBounds(int x, int y)
    {   
        return (x < 0 || x >= Constants.WorldExtents.X) || (y < 0 || y >= Constants.WorldExtents.Y);
    }

    private bool Spiral(int x, int y, Func<int, int, int, int, bool> callback)
    {
        int dx = 1, dy = 0;
        int steps = 1;
        int initX = x, initY = y;

        int life = 50000;
        while (life > 0)
        {
            --life;

            for (int i = 0; i < steps; ++i)
            {
                x += dx;
                y += dy;

                if (callback(x, y, x - initX, y - initY)) {
                    return true;
                }
            }

            int temp = dx;
            dx = -dy;
            dy = temp;

            if (dy == 0)
            {
                ++steps;
            }
        }

        return false;
    }
}
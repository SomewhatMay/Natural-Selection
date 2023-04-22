using Microsoft.Xna.Framework;
using Classes.CellObjects;
using Constants;
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
        
        FoodCell? closestCell = null;
        float distanceToClosestCell = float.MaxValue;

        gameGrid.IterateExclusiveAll(
            // cell.Position.X - ActionConstants.FindNearestFood.SearchBoundsPositionOffset.X,
            // cell.Position.Y - ActionConstants.FindNearestFood.SearchBoundsPositionOffset.Y,

            // ActionConstants.FindNearestFood.SearchBounds.X,
            // ActionConstants.FindNearestFood.SearchBounds.Y,

            // The delegate that finds the nearest cell
            (Cell iteratingCell) => {
                // Let's move on if the current cell is not a food cell
                if (! (iteratingCell is FoodCell))
                    return true;

                // If the closest cell doesnt exist, let's automatically update that with the current cell
                if (closestCell == null) {
                    closestCell = (FoodCell) iteratingCell;
                }

                // Distance formula; let's use floats to conserve memory..
                float distanceToIteratingCell = (float) Math.Sqrt(
                    Math.Pow(iteratingCell.Position.X - cell.Position.X, 2) + Math.Pow(iteratingCell.Position.Y - cell.Position.Y, 2)
                );

                // If the distance to the current cell is less than the distance to the closest cell, then we reassign the values
                if (distanceToIteratingCell < distanceToClosestCell) {
                    closestCell = (FoodCell) iteratingCell;
                    distanceToClosestCell = distanceToIteratingCell;
                }

                // If the closest cell is within 1 square, we know that that is the closest a cell can be.
                // Therefore, we can exit out of the loop
                // if (Math.Floor(distanceToCurrentCell) <= 1f) {
                //     Console.WriteLine("Distance less than 1");
                //     return false;
                // }

                return true;
            }
        );
        
        if (closestCell != null) {
            Point offsetPosition = (closestCell.Position - cell.Position);
            int xOffset = offsetPosition.X;
            int yOffset = offsetPosition.Y;

            // Let's check whether X or Y has a greater impact and move to that direction
            if (Math.Abs(xOffset) > Math.Abs(yOffset)) {
                // Move right or left

                // 1 is right, 3 is left
                direction = (xOffset > 0) ? 1 : 3;
            } else {
                // 0 is up, 2 is down
                direction = (yOffset < 0) ? 0 : 2;
            }

            Console.WriteLine($"Found cell at {closestCell.Position.ToString()}, currently at {cell.Position.ToString()}. Offsets: ({xOffset}, {yOffset}) - Moving at direction {direction}");
        } else {
            Console.WriteLine($"No closest cell found at XY: ({cell.Position.X - ActionConstants.FindNearestFood.SearchBoundsPositionOffset.X}, {cell.Position.Y - ActionConstants.FindNearestFood.SearchBoundsPositionOffset.Y})  with size  XY: ({ActionConstants.FindNearestFood.SearchBounds.X}, {ActionConstants.FindNearestFood.SearchBounds.Y}) ");
        }

        return direction;
    }
}
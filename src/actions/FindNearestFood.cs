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

        gameGrid.IterateExclusiveRegion(
            cell.drawPosition.X - ActionConstants.FindNearestFood.SearchBoundsPositionOffset.X,
            cell.drawPosition.Y - ActionConstants.FindNearestFood.SearchBoundsPositionOffset.Y,

            ActionConstants.FindNearestFood.SearchBounds.X,
            ActionConstants.FindNearestFood.SearchBounds.Y,

            // The delegate that finds the nearest cell
            (Cell currentCell) => {
                // Let's move on if the current cell is not a food cell
                if (! (currentCell is FoodCell))
                    return true;

                // If the closest cell doesnt exist, let's automatically update that with the current cell
                if (closestCell == null) {
                    closestCell = (FoodCell) currentCell;
                }

                // Distance formula; let's use floats to conserve memory..
                float distanceToCurrentCell = (float) Math.Sqrt(
                    (currentCell.Position.X - closestCell.Position.X)^2 + (currentCell.Position.Y - closestCell.Position.Y)^2
                );

                // If the distance to the current cell is less than the distance to the closest cell, then we reassign the values
                if (distanceToCurrentCell < distanceToClosestCell) {
                    closestCell = (FoodCell) currentCell;
                    distanceToClosestCell = distanceToCurrentCell;
                }

                // If the closest cell is within 1 square, we know that that is the closest a cell can be.
                // Therefore, we can exit out of the loop
                if (Math.Floor(distanceToCurrentCell) <= 1f) {
                    return false;
                }

                return true;
            }
        );
        
        if (closestCell != null) {
            Point offsetPosition = (closestCell.Position - cell.Position);
            int xOffset = offsetPosition.X;
            int yOffset = offsetPosition.Y;

            // Let's check whether X or Y has a greater impact and move to that direction
            if (Math.Abs(xOffset)  > Math.Abs(yOffset)) {
                // Move right or left

                // 1 is right, 3 is left
                direction = (xOffset > 0) ? 1 : 3;
            } else {
                // 0 is up, 2 is down
                direction = (yOffset > 0) ? 0 : 2;
            }

            Console.WriteLine($"Found cell at {closestCell.Position.ToString()}, currently at {cell.Position.ToString()}. Offsets: ({xOffset}, {yOffset}) - Moving at direction {direction}");
        }

        return direction;
    }
}
using Microsoft.Xna.Framework;
using Classes.CellObjects;
using ConstantsNamespace;
using Other;
using System;
using Interfaces;

namespace Actions;

//wraps if it doesnt fit in the values
//@param {number} direction - Can be `0` (up) `1` (right) `2` (down) `3` (left)
public class Move : IAction {
    public Move() { }
    
    public int Invoke(Cell cell, int? direction) {
        if (direction == null) {
            direction = 2;
        } else {
            // Wrap the direction
            direction = direction % 4;
        }

        Point directionOffset;

        if (direction == 0) {
            directionOffset = new Point(0, 1);
        } else if (direction == 1) {
            directionOffset = new Point(1, 0);
        } else if (direction == 2) {
            directionOffset = new Point(0, -1);
        } else { // if (direction == 3) {
            directionOffset = new Point(-1, 0);
        }

        cell.Position = cell.Position + directionOffset;

        // Let's wrap the cell if it's out of bounds
        if (cell.Position.X >= Constants.WorldExtents.X) {
            cell.Position = new Point(0, cell.Position.Y);
        } else if (cell.Position.X < 0) {
            cell.Position = new Point(Constants.WorldExtents.X - 1, cell.Position.Y);
        } else if (cell.Position.Y >= Constants.WorldExtents.Y) {
            cell.Position = new Point(cell.Position.X, 0);
        } else if (cell.Position.Y < 0) {
            cell.Position = new Point(cell.Position.X, Constants.WorldExtents.Y - 1);
        }

        return (int) direction;
    }
}
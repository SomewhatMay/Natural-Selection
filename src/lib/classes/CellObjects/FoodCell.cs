using Microsoft.Xna.Framework;

namespace Classes.CellObjects;

public class FoodCell : Cell {
    public bool Alive;

    public FoodCell(Point? position = null) : base(position, Color.Yellow) {
        Alive = true;
    }
}
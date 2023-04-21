using Microsoft.Xna.Framework;

namespace Classes.CellObjects;

public class FoodCell : Cell {
    public bool Alive;

    public FoodCell(Point? position = null, Color? color = null) : base(position, color) {
        this.Color = Color.Yellow;
        Alive = true;
    }
}
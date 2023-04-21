using Microsoft.Xna.Framework;
using Core.ScheduleService;

namespace Classes.CellObjects;

public class LifeCell : Cell {
    public int Points;
    public string Ancestry;
    public int Pointer;
    public int LatestResult;
    public string[] Schedule;
    public bool Alive;

    public LifeCell(Point? position = null, Color? color = null) : base(position, color) {
        Points = 0;
        Ancestry = "00000";
        Alive = true;
    }

    public void Next() {

    }
}
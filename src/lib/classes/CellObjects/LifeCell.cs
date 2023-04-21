using Microsoft.Xna.Framework;
using Core.Schedule;
using System;

namespace Classes.CellObjects;

public class LifeCell : Cell {
    public int Points;
    public string Ancestry;
    public int Pointer;
    public int LatestResult;
    public string[] Schedule;
    public bool Alive;

    private ScheduleService scheduleService;

    #nullable enable
    public LifeCell(Point? position = null, string[]? schedule = null) : base(position, Color.White) {
        Points = 0;
        Ancestry = "00000";
        Alive = true;
        
        scheduleService = (ScheduleService) loadedServices["Schedule"];
        Schedule = schedule ?? scheduleService.newSchedule();
    }

    public void Next() {
        scheduleService.readRoutine(
            this, this.Schedule[this.Pointer], this.LatestResult
        );
    }
}
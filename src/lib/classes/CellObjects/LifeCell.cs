using Microsoft.Xna.Framework;
using Core.ScheduleService;
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

    public LifeCell(Point? position = null, Color? color = null) : base(position, color) {
        Points = 0;
        Ancestry = "00000";
        Alive = true;
        
        scheduleService = (ScheduleService) loadedServices["Schedule"];
        Schedule = scheduleService.newSchedule();
    }

    public void Next() {
        scheduleService.readRoutine(
            this, this.Schedule[this.Pointer], this.LatestResult
        );
    }
}
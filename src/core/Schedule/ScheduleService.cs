using Microsoft.Xna.Framework;
using Constants;
using System;
using Classes;
using Classes.CellObjects;
using Other;
using Interfaces;
using Actions;
using Core;
using System.Collections.Generic;

namespace Core.Schedule;

public class ScheduleService : Service {
    Random gameRandom;
    IAction[] Actions;

    MainWorld mainWorld;

    public ScheduleService(Game game, Random gameRandom) : base(game) {
        this.gameRandom = gameRandom;

        RoutineBitLoader.Init(gameRandom);
        Evaluators.Init();

        // Load all actions
        Actions = new IAction[2] {
            new Move(),
            new FindNearestFood(),
        };
    }

    public override void Init(Dictionary<string, Service> loadedServices) {
        base.Init(loadedServices);

        mainWorld = (MainWorld) loadedServices["MainWorld"];
    }

    public override void LoadContent() {
        // Lets init any actions that may need to be initiated
        
        FindNearestFood.Init(mainWorld.GameGrid);
    }

    public string newRoutine() {
        string routine = "";

        foreach (BitLoaderDelegate callback in RoutineBitLoader.RoutineOrder) {
            string postFix = callback();
            routine = routine + postFix;
        }

        return routine;
    }

    public string[] newSchedule() {
        string[] Schedule = new string[GameConstants.ScheduleSize];

        for (int index = 0; index < GameConstants.ScheduleSize; ++index) {
            Schedule[index] = newRoutine();
        }

        return Schedule;
    }

    // Automatically assigns the last result and pointer in the provided cell 
    public void readRoutine(LifeCell cell, string routine, int previousArgument) {
        // Lets calculate all the bits from the routine
        int evalType = int.Parse( routine.Substring(0, 1) ); // The type of evaulation being done
        int connectionA = int.Parse( routine.Substring(1, 1) ); // Pointer to connection A
        int connectionB = int.Parse( routine.Substring(2, 1) ); // Pointer to connection B
        string actionType = routine.Substring(3, 1); // Determiens whether a static variable or an action pointer is being used
        int assistingBit1 = int.Parse( routine.Substring(4, 1) ); // Either a static variable, or an action pointer

        // Determines whether a static variable gets passed into the next action or
        // if a static variable is the previous one, determines whether that variable
        // is passed or a static one.
        string assistingBit2 = routine.Substring(5, 1);

        // / The bit that get passed into the next method if a seperate static variable is chosen by the previous bit
        int assistingBit3 = int.Parse( routine.Substring(6, 1) );
        int evalBit = int.Parse( routine.Substring(7, 1) ); // the bit that gets evaled against

        int actionReturn;

        if (actionType == "0") {

            if (assistingBit2 == "1") {
                previousArgument = assistingBit3;
            }

            actionReturn = Actions[wrap(assistingBit1, Actions.Length)].Invoke(cell, previousArgument);
        } else {
            actionReturn = assistingBit1;
        }

        if (Evaluators.List[wrap(evalType, Evaluators.List.Length)](actionReturn, evalBit)) {
            cell.Pointer = connectionA;
        } else {
            cell.Pointer = connectionB;
        }

        cell.LatestResult = actionReturn;
    }

    public void MutateCell(LifeCell cell, string[] targetSchedule) {
        cell.Schedule = targetSchedule;
    }

    private int wrap(int number, int max) {
        return number % max;
    }
}
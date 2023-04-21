using Microsoft.Xna.Framework;
using ConstantsNamespace;
using System;
using Classes;
using Other;
using Actions;

namespace Core.ScheduleService;

public class ScheduleService : Service {
    Random gameRandom;
    IAction[] Actions;

    public ScheduleService(Game game, Random gameRandom) : base(game) {
        this.gameRandom = gameRandom;

        RoutineBitLoader.Init(gameRandom);
        Evaluators.Init();

        // Load all actions
        Actions = new IAction[1] {
            new Move(),
        };
    }

    public override void LoadContent() {
        // Lets init any actions that may need to be initiated
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
        string[] Schedule = new string[Constants.ScheduleSize];

        for (int index = 0; index < Constants.ScheduleSize; ++index) {
            Schedule[index] = newRoutine();
        }

        return Schedule;
    }

    public string 


}
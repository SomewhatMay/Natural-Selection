using System;
using ConstantsNamespace;

namespace Core.ScheduleService;

public delegate string BitLoaderDelegate();

public static class RoutineBitLoader {
    static Random gameRandom;

    public static BitLoaderDelegate[] RoutineOrder;

    public static void Init(Random _gameRandom) {
        gameRandom = _gameRandom;

        // Lets add add the Routine Bit Loaders to a list for easy access when creating a routine
        RoutineOrder = new BitLoaderDelegate[8] {
            EvalType,
            ConnectionPointer, // Connection A
            ConnectionPointer, // Connection B
            ActionType,
            AssistingBit1,
            AssistingBit2,
            AssistingBit3,
            EvalBit,
        };
    }

    public static string EvalType() {
        return gameRandom.Next(0, 5).ToString();
    }

    public static string ConnectionPointer() {
        return gameRandom.Next(0, Constants.ScheduleSize).ToString();
    }

    public static string ActionType() {
        return gameRandom.Next(0, 1).ToString();
    }

    public static string AssistingBit1() {
        return gameRandom.Next().ToString();
    }

    public static string AssistingBit2() {
        return gameRandom.Next(0, 1).ToString();
    }

    public static string AssistingBit3() {
        return gameRandom.Next().ToString();
    }

    public static string EvalBit() {
        return gameRandom.Next().ToString();
    }
}

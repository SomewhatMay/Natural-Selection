using System;
using ConstantsNamespace;

namespace Core.Schedule;

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

    //  **  NOTE  ** 
    // All of the folloing bits get an extra added to them because Random.Next(int min, int maax) returns a value that is max exclusive
    // meaning it only returns a value that is less than the max; however, the connection does not have one added because indexes of arrays
    // are 0 to the lenght - 1, so adding one to it would go out of the array. 

    public static string EvalType() {
        return gameRandom.Next(0, 6).ToString();
    }

    public static string ConnectionPointer() {
        return gameRandom.Next(0, Constants.ScheduleSize).ToString();
    }

    public static string ActionType() {
        return gameRandom.Next(0, 2).ToString();
    }

    public static string AssistingBit1() {
        return gameRandom.Next(0, 10).ToString();
    }

    public static string AssistingBit2() {
        return gameRandom.Next(0, 2).ToString();
    }

    public static string AssistingBit3() {
        return gameRandom.Next(0, 10).ToString();
    }

    public static string EvalBit() {
        return gameRandom.Next(0, 10).ToString();
    }
}

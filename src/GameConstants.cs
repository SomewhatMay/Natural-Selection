using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Constants;

public static class GameConstants {
	public static string version = "1.9.2";

    public static int? Seed = 936864398; // can be null, null => random seed // 936864398
    public static double UpdateRate = 0f; // The rate at which .Next() is called in a cell; in miliseconds
    public static int ScheduleSize = 8; // The size of the schedules each cell has
    public static float MutationChance = 0.4f; // The chance that the next cell generation mutates 
    public static int ChanceMax = 1000; // the max value put in when calculating the number of cells to be generated

	// The number of threads that the game should split the game grid up into
	// *** NOTE *** Ensure this is a square number!
	public static int Threads = 4;

    // The values to watch for when determining which cell to spawn
    public static int[] LifeCellRandomValues = new int[1] { 1 };
    public static int[] FoodCellRandomValues = new int[5] { 2 , 3 , 4 , 5 , 6 };

    // This will be used to determine the chance of cell reproduction based on their ranks
    public static Dictionary<int, float> ReproductionPercentDictionary = new Dictionary<int, float> {
        [0] = .3f,
        [1] = .15f,
        [2] = .1f,
        [3] = .08f,
        [4] = .06f,
        [5] = .05f,
        [6] = .04f,
        [7] = .04f,
        [8] = .03f,
        [9] = .02f,
    };
    public static float ReproductionUnsignedSchedulePercent; // Gets calculated based on the sum of the previous table

    public static Point WindowSize = new Point(1300, 1000);
    public static Point WorldExtents = new Point(500, 500); // number of rows and columns
    public static int SidebarWidth = 300;
    public static int WorldPixelWidth = WindowSize.X - SidebarWidth;
    public static Point CellSize;

    public static class RewardSetting {
        public static int Food = 10;
        public static int Death = -100;
    }

    // use this method to change any constants that need to be calculated in runtime 
    public static void Initialize() {
        if (Seed == null) {
            Seed = new Random().Next();
        }

        CellSize = new Point(
            WorldPixelWidth / WorldExtents.X,
            WindowSize.Y / WorldExtents.Y
        );

        // Let's update all the entries into a value from the previous number plus the current number
        // and set the unsigned area as well
        ReproductionUnsignedSchedulePercent = 0;
        foreach (var (index, value) in ReproductionPercentDictionary) {
            ReproductionUnsignedSchedulePercent += value;
            ReproductionPercentDictionary[index] = ReproductionUnsignedSchedulePercent;
        }
    }
}
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConstantsNamespace;

public static class Constants {
    public static int? Seed = null; // can be null, null => random seed
    public static double UpdateRate = 0f; // The rate at which .Next() is called in a cell; in miliseconds
    public static int ScheduleSize = 8; // The size of the schedules each cell has
    public static float MutationChance = 0.4f; // The chance that the next cell generation mutates 
    public static int ChanceMax = 500; // the max value put in when calculating the number of cells to be generated

    // The values to watch for when determining which cell to spawn
    public static int[] LifeCellRandomValues = new int[1] { 1 };
    public static int[] FoodCellRandomValues = new int[2] { 2 , 3 };

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
    public static int ReproductionUnsignedSchedule; // Gets calculated based on the sum of the previous table

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
        CellSize = new Point(
            WorldPixelWidth / WorldExtents.X,
            WindowSize.Y / WorldExtents.Y
        );
    }
}
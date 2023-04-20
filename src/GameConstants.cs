using Microsoft.Xna.Framework;

namespace Constants;

public static class Constants {
    public static Vector2 WindowSize = new Vector2(800, 800);
    public static Vector2 WorldExtents = new Vector2(150, 150);
    public static Point CellSize;

    // use this method to change any constants that need to be calculated in runtime 
    public static void Initialize() {
        CellSize = new Point(
            (int) (WindowSize.X / WorldExtents.X),
            (int) (WindowSize.Y / WorldExtents.Y)
        );
    }
}
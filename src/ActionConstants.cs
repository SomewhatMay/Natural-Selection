using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Constants;

public static class ActionConstants {
    public static class FindNearestFood {
        public static Point SearchBounds = new Point( // A 1/4 of the world size
            (int) (GameConstants.WorldExtents.X / 4),
            (int) (GameConstants.WorldExtents.Y / 4)
        );

        // Used to determine the top left position of the search area
        public static Point SearchBoundsPositionOffset = new Point(
            (int) (SearchBounds.X / 2),
            (int) (SearchBounds.Y / 2)
        );
    }
}
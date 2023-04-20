using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Constants;

namespace Classes.CellObjects;

public abstract class Cell {
    // static fields and methods
    protected static GraphicsDevice graphicsDevice;

    // loads the cell class statically before calling any object-based methods
    public static void Load(GraphicsDevice _graphicsDevice) {
        graphicsDevice = _graphicsDevice;
    }

    // object-oriented fields and methods
    private Rectangle rectangle;
    protected Texture2D cellBackground;

    public Rectangle Bounds {
        get {
            return this.rectangle;
        } 
        set { } // we dont want it to be changeable
    }

    private Point drawPosition;
    private Point position;
    public Point Position {
        get { return position; } set { 
            position = value;
            drawPosition = value * Constants.Constants.CellSize;
            newRectangle();
        }
    }

    private Point size;
    public Point Size {
        get { return size; } set {
            size = value;
            newRectangle();
        }
    }

    private Color color;
    public Color Color {
        get { return color; } set {
            color = value;
            this.cellBackground.SetData(new[] { this.Color });
        }
    }

    protected Cell(Point? position = null, Color? color = null) {
        cellBackground = new Texture2D(graphicsDevice, 1, 1);

        this.Position = position ?? new Point(0, 0);
        this.Size = Constants.Constants.CellSize;
        this.Color = color ?? Color.White;

        newRectangle();
    }

    private void newRectangle() {
        rectangle = new Rectangle(this.drawPosition, this.Size);
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(this.cellBackground, this.rectangle, this.Color);
    }
}
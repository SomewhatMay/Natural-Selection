using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

using Constants;

namespace Classes.CellObjects;

public abstract class Cell {
    // static fields and methods
    protected static GraphicsDevice graphicsDevice;
    protected static Dictionary<string, Service> loadedServices;

    // loads the cell class statically before calling any object-based methods
    public static void Load(GraphicsDevice _graphicsDevice, Dictionary<string, Service> _loadedServices) {
        graphicsDevice = _graphicsDevice;
        loadedServices = _loadedServices;
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

    public Point drawPosition { get; private set; }
    private Point position;
    public Point Position {
        get { return position; } set { 
            position = value;
            drawPosition = value * GameConstants.CellSize;
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
        this.Size = GameConstants.CellSize;
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
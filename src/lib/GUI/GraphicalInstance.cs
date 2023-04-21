using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Classes;

namespace GUI;

public abstract class GraphicalInstance {
    protected static GraphicsDevice graphicsDevice;
    protected static Dictionary<string, Service> loadedServices;

    // loads the GraphicalInstance class statically before calling any object-based methods
    public static void Load(GraphicsDevice _graphicsDevice, Dictionary<string, Service> _loadedServices) {
        graphicsDevice = _graphicsDevice;
        loadedServices = _loadedServices;
    }

    public bool Visible;
    
    protected Point drawPosition;
    public Point Position { get { return drawPosition; } set {
        drawPosition = value;
        onPositionUpdated();
    }}

    protected Point size;
    public Point Size {
        get { return size; } set {
            size = value;
            onSizeUpdated();
        }
    }

    protected GraphicalInstance() : this(new Point(0, 0), new Point(50, 50), true) { }
    protected GraphicalInstance(Point position) : this(position, new Point(50, 50), true) { }
    protected GraphicalInstance(Point position, Point size) : this(position, size, true) { }
    protected GraphicalInstance(Point position, Point size, bool visible) {
        this.Position = position;
        this.Size = size;
        this.Visible = visible;
    }

    protected virtual void onPositionUpdated() { }
    protected virtual void onSizeUpdated() { }

    public virtual void Update() { }
    public abstract void Draw();
}
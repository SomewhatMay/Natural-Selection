#nullable enable

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
        OnPositionUpdated();
        UpdateAllChildrenOffsets();
    }}

    protected Point size;
    public Point Size {
        get { return size; } set {
            size = value;
            OnSizeUpdated();
        }
    }

    protected Point drawOffset;
    private Dictionary<int, GraphicalInstance> children;

    protected GraphicalInstance() : this(new Point(0, 0), new Point(50, 50), true) { }
    protected GraphicalInstance(Point position) : this(position, new Point(50, 50), true) { }
    protected GraphicalInstance(Point position, Point size) : this(position, size, true) { }
    protected GraphicalInstance(Point position, Point size, bool visible) {
        this.Position = position;
        this.Size = size;
        this.Visible = visible;

        children = new Dictionary<int, GraphicalInstance>();
    }

    private int nextChildAddIndex = 0;
    //@return - returns the index at which the child was added to
    public int AddChild(GraphicalInstance child) {
        children[nextChildAddIndex] = child;
        ++nextChildAddIndex;

        return nextChildAddIndex - 1;
    }

    public void RemoveChild(int index) {
        children.Remove(index);
    }

    private void UpdateAllChildrenOffsets() {
        foreach (var (index, child) in children) {
            child.OffsetChanged(Position);
        }
    }

    protected virtual void OffsetChanged(Point newOffset) {
        this.drawOffset = newOffset;
    }

    protected virtual void OnPositionUpdated() { }
    protected virtual void OnSizeUpdated() { }

    public virtual void Update(GameTime gameTime) { }
    public abstract void Draw(SpriteBatch spriteBatch);
}
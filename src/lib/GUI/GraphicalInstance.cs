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

    #nullable enable
    protected Point drawOffset;
    private Dictionary<int, GraphicalInstance>? children;
    private int nextChildAddIndex = 0;
    private GraphicalInstance parent;
    private int? childIndexInparent;

    #nullable disable
    public GraphicalInstance Parent {
        get { return parent; } set {
            if (parent != null) {
                parent.RemoveChild((int) childIndexInparent);
            }

            if (value == null) {
                throw new System.Exception("Attempted to parent object to null");
            } else {
                parent = value;
                childIndexInparent = value.AddChild(this);
            }
        }
    }

    protected GraphicalInstance() : this(new Point(0, 0), new Point(50, 50), true) { }
    protected GraphicalInstance(Point position) : this(position, new Point(50, 50), true) { }
    protected GraphicalInstance(Point position, Point size) : this(position, size, true) { }
    protected GraphicalInstance(Point position, Point size, bool visible) {
        this.Position = position;
        this.Size = size;
        this.Visible = visible;

        children = new Dictionary<int, GraphicalInstance>();
    }
    
    // Should never be called! Only change the object's parent!
    protected int AddChild(GraphicalInstance child) {
        children[nextChildAddIndex] = child;
        ++nextChildAddIndex;

        return nextChildAddIndex - 1;
    }

    // Should never be called! Only change the object's parent!
    protected void RemoveChild(int index) {
        children[index] = null;
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
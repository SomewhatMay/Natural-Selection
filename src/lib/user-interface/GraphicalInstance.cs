using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Classes;

namespace GUI;

public delegate void onClickedDelegate(bool alreadyClicked, int mouseX, int mouseY);

public abstract class GraphicalInstance {
	protected static GraphicsDevice graphicsDevice;
	protected static Dictionary<string, Service> loadedServices;
	private static Dictionary<int, GraphicalInstance> clickableInstances;
	private static int allClickableInstanceIndex = 0;

	private static MouseState previousMouseState;

	// loads the GraphicalInstance class statically before calling any object-based methods
	public static void Load(GraphicsDevice _graphicsDevice, Dictionary<string, Service> _loadedServices) {
		graphicsDevice = _graphicsDevice;
		loadedServices = _loadedServices;

		clickableInstances = new Dictionary<int, GraphicalInstance>();
	}

	public static void FrameCheck(GameTime gameTime) {
		// Let's check if we are clicking on any clickable instances;
		MouseState mouseState = Mouse.GetState();

		// Let's only do the checking if the mosue is clicked
		if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed) {

			int mouseX = mouseState.X, mouseY = mouseState.Y;
			// Determines whether the input was passed on to another UI instance already
			// Useful if you dont want to click on multiple things if they are overlapped
			bool alraedyClicked = false;

			foreach (var (_, clickableInstance) in clickableInstances) {
				// Standard method of checking if our mouse coordinates are in the draw object's position and size
				// Does not work with circles, Skull emoji
				if ((mouseX >= clickableInstance.Position.X) && (mouseY > clickableInstance.Position.Y)) {
					if ((mouseX <= (clickableInstance.Position.X + clickableInstance.Size.X)) && (mouseY <= (clickableInstance.Position.Y + clickableInstance.Size.Y))) {
						if (!(clickableInstance.Active && clickableInstance.Visible))
							continue;

						clickableInstance.Clicked(alraedyClicked, mouseX, mouseY);
						alraedyClicked = true;
					}
				}
			}
		}

		previousMouseState = mouseState;
	}

	// Determines whether the instance is clickable or not
	public bool ClickableInstance { get; private set; }
	// Determines whether the instance acts on interaction. If it's not visible, it's not clickable
	public bool Active;

	// The delegate that gets called when the instance is clicked
	onClickedDelegate onClickedCallback;

	// Is the instance visible? If it's not visible, it's not clickable
	public bool Visible;

	public Point AbsolutePosition { get; private set; }
	protected Point position;
	public Point Position
	{
		get { return position; }
		set
		{
			position = value;
			OnPositionUpdated();
			UpdateAllChildrenOffsets();
		}
	}

	protected Point size;
	public Point Size
	{
		get { return size; }
		set
		{
			size = value;
			OnSizeUpdated();
		}
	}

#nullable enable
	public string Name;
	protected Point drawOffset;
	private Dictionary<int, GraphicalInstance>? children;
	private int nextChildAddIndex = 0;
	private GraphicalInstance? parent;
	private int? childIndexInparent;

#nullable disable
	public GraphicalInstance Parent
	{
		get { return parent; }
		set
		{
			if (parent != null)
			{
				parent.RemoveChild((int)childIndexInparent);
			}

			if (value == null)
			{
				throw new System.Exception("Attempted to parent object to null");
			}
			else
			{
				parent = value;
				childIndexInparent = value.AddChild(this);
				OffsetChanged(value.Position + value.drawOffset);
			}
		}
	}

#nullable enable
	protected GraphicalInstance() : this(new Point(0, 0), new Point(50, 50), true) { }
	protected GraphicalInstance(Point position) : this(position, new Point(50, 50), true) { }
	protected GraphicalInstance(Point position, Point size) : this(position, size, true) { }
	protected GraphicalInstance(Point position, Point size, GraphicalInstance? parent) : this(position, size, true, parent) { }
	protected GraphicalInstance(Point position, Point size, bool visible, GraphicalInstance? parent = null)
	{
		Name = "GraphicalInstance";
		children = new Dictionary<int, GraphicalInstance>();

		this.Position = position;
		this.Size = size;
		this.Visible = visible;
		this.Active = true;

		if (parent != null)
		{
			this.Parent = parent;
		}
	}

	// Clickable stuff
	private int objectClickableIndex;
	public void MakeClickableInstance() {
		if (ClickableInstance) return; // it's already a clickable instance

		ClickableInstance = true;

		clickableInstances.Add(allClickableInstanceIndex, this);
		this.objectClickableIndex = allClickableInstanceIndex;
		++allClickableInstanceIndex;
	}

	public void NoLongerClickable()
	{
		if (!ClickableInstance) return; // The object is not a clickable instance

		ClickableInstance = false;
		clickableInstances[objectClickableIndex] = null;
	}

#nullable disable
	// Should never be called! Only change the object's parent!
	protected int AddChild(GraphicalInstance child)
	{
		children[nextChildAddIndex] = child;
		++nextChildAddIndex;

		return nextChildAddIndex - 1;
	}

	// Should never be called! Only change the object's parent!
	protected void RemoveChild(int index)
	{
		children[index] = null;
	}

	private void UpdateAllChildrenOffsets()
	{
		Point totalOffset = this.Position + this.drawOffset;

		foreach (var (index, child) in children)
		{
			child.OffsetChanged(totalOffset);
		}
	}

	protected virtual void OffsetChanged(Point newOffset)
	{
		this.drawOffset = newOffset;
		UpdateAllChildrenOffsets();
	}

	public void SetOnClicked(onClickedDelegate callback)
	{
		if (onClickedCallback != null)
			throw new InvalidOperationException("This object already has a delegate set");

		this.onClickedCallback = callback;
	}

	protected virtual void Clicked(bool alreadyClicked, int mouseX, int mouseY)
	{
		if (onClickedCallback != null)
		{
			onClickedCallback(alreadyClicked, mouseX, mouseY);
		}
	}

	protected virtual void OnPositionUpdated() { }
	protected virtual void OnSizeUpdated() { }

	public virtual void Update(GameTime gameTime) { }
	public abstract void Draw(SpriteBatch spriteBatch);
}
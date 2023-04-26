using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Classes;

namespace GUI;

public delegate void onClickedDelegate(bool alreadyClicked, int mouseX, int mouseY);
public delegate void onPaddingChangedDelegate();

public class Padding
{
	private onPaddingChangedDelegate paddingChangedCallback;

	/// <summary> The padding at the top. Will overwrite all `AllPadding` with assigned value </summary>
	private int topPadding;
	public int TopPadding { get { return TopPadding; } set { TopPadding = value; onPaddingChanged(); } }

	/// <summary> The padding at the left. Will overwrite all `AllPadding` with assigned value </summary>
	private int leftPadding;
	public int LeftPadding { get { return leftPadding; } set { leftPadding = value; onPaddingChanged(); } }

	/// <summary> The padding at the right. Will overwrite all `AllPadding` with assigned value </summary>
	private int rightPadding;
	public int RightPadding { get { return rightPadding; } set { rightPadding = value; onPaddingChanged(); } }

	/// <summary> The padding at the bottom. Will overwrite all `AllPadding` with assigned value </summary>
	private int bottomPadding;
	public int BottomPadding { get { return bottomPadding; } set { bottomPadding = value; onPaddingChanged(); } }

	/// <summary> Will overwrite all `TopPadding` `LeftPadding` `RightPadding` and `BottomPadding` with the same value </summary>
	private int allPadding;
	public int AllPadding
	{
		get { return allPadding; }
		set
		{
			TopPadding = value;
			LeftPadding = value;
			RightPadding = value;
			BottomPadding = value;
			allPadding = value;

			onPaddingChanged();
		}
	}

	private void onPaddingChanged()
	{
		if (paddingChangedCallback != null)
		{
			paddingChangedCallback();
		}
	}

	protected void SetOnPaddingChanged(onPaddingChangedDelegate callback)
	{
		this.paddingChangedCallback = callback;
	}
}

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
				if ((mouseX >= clickableInstance.AbsolutePosition.X) && (mouseY > clickableInstance.AbsolutePosition.Y)) {
					if ((mouseX <= (clickableInstance.AbsolutePosition.X + clickableInstance.Size.X)) && (mouseY <= (clickableInstance.AbsolutePosition.Y + clickableInstance.Size.Y))) {
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
	private Point parentOffset;
	protected Point ParentOffset { get { return parentOffset; } 
		set {
			parentOffset = value;
			UpdateAbsolutePosition();
		}
	}
	private Point position;
	public Point Position
	{
		get { return position; }
		set
		{
			position = value;
			UpdateAbsolutePosition();
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
				ParentOffset = value.AbsolutePosition;
				childIndexInparent = value.AddChild(this);
				UpdateAbsolutePosition();
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
		foreach (var (index, child) in children)
		{
			child.ParentOffset = this.AbsolutePosition;
		}
	}

	public virtual void UpdateAbsolutePosition()
	{
		this.AbsolutePosition = this.Position + this.ParentOffset;
		OnPositionUpdated();
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
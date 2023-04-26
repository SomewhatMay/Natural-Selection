using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GUI;

public class Frame : GraphicalInstance
{
	private Rectangle rectangle;
	protected Texture2D cellBackground;

	private Color backgroundColor;
	public Color BackgroundColor
	{
		get { return backgroundColor; }
		set
		{
			backgroundColor = value;
			this.cellBackground.SetData(new[] { value });
		}
	}

#nullable enable
	public Frame(Point position, Point size, GraphicalInstance parent) : this(position, size, Color.White, parent) { }
	public Frame(Point position, Point size, Color? backgroundColor = null, GraphicalInstance? parent = null) : base(position, size, parent)
	{
		cellBackground = new Texture2D(graphicsDevice, 1, 1);
		this.BackgroundColor = backgroundColor ?? Color.White;
		newRectangle();
		this.Name = "Frame";
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(this.cellBackground, this.rectangle, this.BackgroundColor);
	}

	protected override void OnPositionUpdated()
	{
		base.OnPositionUpdated();
		newRectangle();
	}

	protected override void OnSizeUpdated()
	{
		base.OnSizeUpdated();
		newRectangle();
	}

	private void newRectangle()
	{
		rectangle = new Rectangle(this.AbsolutePosition, this.Size);
	}
}
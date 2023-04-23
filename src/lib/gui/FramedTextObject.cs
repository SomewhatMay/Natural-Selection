using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI;
internal class FramedTextObject : Frame
{
	public TextLabel label;

	public Color TextColor 
	{ 
		get { return label.TextColor; } set { label.TextColor = value; }	
	}

	public TextAllignment Allignment
	{
		get { return label.Allignment; } set { label.Allignment = value; }
	}

	public string Text
	{
		get { return label.Text; } set { label.Text = value; }
	}

#nullable enable
	public FramedTextObject(Point position, Point size, string? text = "Framed Text Object", GraphicalInstance? parent = null) : base (position, size, parent) {
		label = new TextLabel(Point.Zero, size, text);
		label.TextColor = Color.Black;

		// Set the text label's parent to the frame object so it inherits the positions
		label.Parent = this;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		label.Draw(spriteBatch);
	}
}
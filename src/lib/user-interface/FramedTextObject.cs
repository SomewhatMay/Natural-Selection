using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI;

public class FramedTextObject : Frame
{
	public TextLabel TextLabel;

	public Color TextColor 
	{ 
		get { return TextLabel.TextColor; } set { TextLabel.TextColor = value; }	
	}

	public TextAllignment Allignment
	{
		get { return TextLabel.Allignment; } set { TextLabel.Allignment = value; }
	}

	public string Text
	{
		get { return TextLabel.Text; } set { TextLabel.Text = value; }
	}

#nullable enable
	public FramedTextObject(Point position, Point size, string? text = "Framed Text Object", GraphicalInstance? parent = null) : base (position, size, parent) {
		TextLabel = new TextLabel(Point.Zero, size, text);
		TextLabel.TextColor = Color.Black;

		// Set the text label's parent to the frame object so it inherits the positions
		TextLabel.Parent = this;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		TextLabel.Draw(spriteBatch);
	}
}
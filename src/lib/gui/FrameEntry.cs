using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GUI;

public class FrameEntry : Frame {
	public TextLabel TitleLabel;
	public TextLabel ValueLabel;

	public Color TitleColor {
		get { return TitleLabel.TextColor; }
		set { TitleLabel.TextColor = value; }
	}

	public Color ValueColor {
		get { return ValueLabel.TextColor; }
		set { ValueLabel.TextColor = value; }
	}

	public string TitleText {
		get { return TitleLabel.Text; }
		set { TitleLabel.Text = value; }
	}

	public string ValueText {
		get { return ValueLabel.Text; }
		set { ValueLabel.Text = value; }
	}

#nullable enable
	public FrameEntry(Point position, Point size, string? titleText = "(Title)", string? valueText = "(null)", GraphicalInstance? parent = null) : base(position, size, parent) {
		TitleLabel = new TextLabel(Point.Zero, size, titleText);
		TitleColor = Color.Black;
		TitleLabel.Allignment = TextAllignment.LEFT;

		ValueLabel = new TextLabel(Point.Zero, size, valueText);
		ValueColor = Color.Black;
		ValueLabel.Allignment = TextAllignment.RIGHT;

		// Set the text label's parent to the frame object so it inherits the positions
		TitleLabel.Parent = this;
		ValueLabel.Parent = this;
	}

	public override void Draw(SpriteBatch spriteBatch) {
		base.Draw(spriteBatch);
		TitleLabel.Draw(spriteBatch);
		ValueLabel.Draw(spriteBatch);
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaturalSelectionRemastered;
using System.ComponentModel.DataAnnotations;

namespace GUI;

public enum TextAllignment {
	LEFT,
	RIGHT,
	CENTER
}

internal class TextLabel : GraphicalInstance
{

	public Point TextSize { get; private set; }

	private TextAllignment allignment;
	public TextAllignment Allignment
	{
		get { return allignment; } set
		{
			allignment = value;
			updateAllignment();
		}
	}

	private Point allignmentPosition;

	public Color TextColor;

	private string text;
	public string Text
	{
		get { return text; }
		set 
		{
			text = value;
			TextSize = NaturalSelection.TextFont.MeasureString(value).ToPoint();
			updateAllignment();
		}
	}

#nullable enable
	public TextLabel(Point position, Point size, string? text = "Label", GraphicalInstance? parent = null) : base(position, size, parent)
	{
		this.Allignment = TextAllignment.LEFT;
		this.Text = text;
		this.TextColor = Color.White;
		this.Name = "TextLabel";
	}

	private void updateAllignment() {
		if (Allignment == TextAllignment.LEFT)
		{
			allignmentPosition = Point.Zero;
		}
		else if (Allignment == TextAllignment.CENTER)
		{
			allignmentPosition = (new Point((Size.X - TextSize.X) / 2, 0));
		}
		else if (Allignment == TextAllignment.RIGHT)
		{
			allignmentPosition = (new Point(Size.X - TextSize.X, 0));
		}
		else throw new NotImplementedException($"Not implemented allignment mode{Allignment}");
	}

	protected override void OffsetChanged(Point newOffset)
	{
		drawOffset = newOffset;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(NaturalSelection.TextFont, Text, Position.ToVector2() + drawOffset.ToVector2() + allignmentPosition.ToVector2(), TextColor);
	}

}

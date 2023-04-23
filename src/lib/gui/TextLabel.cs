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

	private Point drawOffset;
	private Point textPosition;

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
	}

	private void updateAllignment() {
		if (Allignment == TextAllignment.LEFT)
		{
			textPosition = Position;
		}
		else if (Allignment == TextAllignment.CENTER)
		{
			textPosition = (new Point((Size.X - TextSize.X) / 2, 0)) + Position;
		}
		else if (Allignment == TextAllignment.RIGHT)
		{
			textPosition = (new Point(Size.X - TextSize.X, 0)) + Position;
		}
		else throw new NotImplementedException($"Not implemented allignment mode{Allignment}");

		Console.WriteLine($"Alligned text with {Allignment} with position {textPosition.ToString()}");
	}

	protected override void OffsetChanged(Point newOffset)
	{
		drawOffset = newOffset;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.DrawString(NaturalSelection.TextFont, Text, drawOffset.ToVector2() + textPosition.ToVector2(), TextColor);
	}

}

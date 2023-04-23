using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GUI;

public class Frame : GraphicalInstance {
    private Rectangle rectangle;
    protected Texture2D cellBackground;

    private Color backgroundColor;
    public Color BackgroundColor {
        get { return backgroundColor; } set {
            backgroundColor = value;
            this.cellBackground.SetData(new[] { value });
        }
    }

    public Frame(Point position, Point size) : base(position, size) {
        cellBackground = new Texture2D(graphicsDevice, 1, 1);
        newRectangle();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(this.cellBackground, this.rectangle, this.BackgroundColor);
    }

    protected override void OffsetChanged(Point newOffset) {
        newRectangle();

        base.OffsetChanged(newOffset);
    }

    private void newRectangle() {
        rectangle = new Rectangle(this.drawPosition + this.drawOffset, this.Size);
    }
}
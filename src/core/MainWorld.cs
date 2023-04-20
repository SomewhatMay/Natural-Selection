using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using NaturalSelectionRemastered;
using Classes.GameGrid;
using Classes;
using Classes.CellObjects;
using Constants;

namespace Core;

public class MainWorld : Service {
    Grid gameGrid;
    SpriteBatch spriteBatch;

    Random gameRandom;

    public MainWorld(Game game, Random gameRandom) : base(game) {
        this.game = game;
        this.gameRandom = gameRandom;
    }

    public override void LoadContent() {
        Cell.Load(this.game.GraphicsDevice);
        spriteBatch = new SpriteBatch(this.game.GraphicsDevice);

        gameGrid = new Grid(Constants.Constants.WorldExtents);

        // Load all the cells
        #nullable enable
        gameGrid.FillGrid((int x, int y) => {
            int chance = gameRandom.Next(0, 1000);

            if (chance == 1) {
                Cell lifeCell = new LifeCell(
                    new Point(x, y)
                );

                return lifeCell;
            } else if (chance == 2 || chance == 3) {
                Cell foodCell = new FoodCell(
                    new Point(x, y)
                );

                return foodCell;
            }

            return null;
        });
    }

    public override void Update(GameTime gameTime) {
        
    } 

    public override void Draw(GameTime gameTime) {
        this.spriteBatch.Begin();

        gameGrid.IterateExclusiveAll((Cell cell) => {
            if (cell != null) {
                cell.Draw(spriteBatch);
            }

            return true;
        });

        this.spriteBatch.End();
    }
}
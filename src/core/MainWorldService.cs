using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using NaturalSelectionRemastered;
using Classes.GameGrid;
using Classes;
using Classes.CellObjects;
using ConstantsNamespace;
using System.Collections.Generic;

namespace Core;

public class MainWorld : Service {
    Grid gameGrid;
    SpriteBatch spriteBatch;

    Random gameRandom;
    Dictionary<string, Service> loadedServices;

    public decimal lastUpdate { get; private set; }

    public MainWorld(Game game, Random gameRandom) : base(game) {
        this.game = game;
        this.gameRandom = gameRandom;

        lastUpdate = System.Environment.TickCount;
    }

    public override void LoadContent() {
        Cell.Load(this.game.GraphicsDevice, loadedServices);
        spriteBatch = new SpriteBatch(this.game.GraphicsDevice);

        gameGrid = new Grid(Constants.WorldExtents);

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

    private bool isUpdating = false;
    public override void Update(GameTime gameTime) {
        // gameGrid.IterateExclusiveAll((Cell cell) => {
        //     if (cell is LifeCell) {
        //         LifeCell lifeCell = (LifeCell) cell;
        //         lifeCell.Next();
        //     }

        //     return true;
        // });

        if (((System.Environment.TickCount - lastUpdate) / 1000 >= Constants.UpdateRate) && !isUpdating) {
            isUpdating = true;

            gameGrid.IterateExclusiveAll((Cell cell) => {
                if (cell is LifeCell) {
                    LifeCell lifeCell = (LifeCell) cell;
                    lifeCell.Next();
                }

                return true;
            });

            lastUpdate = System.Environment.TickCount;
            isUpdating = false;
        }
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
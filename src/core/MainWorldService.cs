using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

using NaturalSelectionRemastered;
using Classes.GameGrid;
using Classes;
using Classes.CellObjects;
using ConstantsNamespace;
using System.Collections.Generic;
using Other;

namespace Core;

public class MainWorld : Service {
    public static class Statistics {
        public static int Day { get; set; }
        public static int Generation { get; set; }
        public static int FoodAlive { get; set; }
        public static int FoodEaten { get; set; }
        public static int CellsAlive { get; set; }
        public static int CellsGarrisoned { get; set; }
    }

    private GameState gameState;
    public GameState GameState { get { return gameState; } set {
        gameState = value;
    }}

    // Class used fields
    Grid gameGrid;
    Grid nextGameGrid;
    SpriteBatch spriteBatch;

    Random gameRandom;

    public double lastUpdate { get; private set; }

    public MainWorld(Game game, Random gameRandom) : base(game) {
        this.game = game;
        this.gameRandom = gameRandom;
        
        lastUpdate = 0f;
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

    public void NextDay() {
        //Console.WriteLine("Next day called");

        ++Statistics.Day; // increment the day

        int currentDay = Statistics.Day;
        int currentGeneration = Statistics.Generation;

        gameGrid.IterateExclusiveAll((Cell cell) => {
            if (cell is LifeCell) {
                LifeCell lifeCell = (LifeCell) cell;

                if (lifeCell.Alive) {
                    lifeCell.Next();

                    // Lets check if two cells overlap, and if they do, then let's garrison the one with the lower points

                    Cell residingCell = gameGrid.GetInGrid(lifeCell.Position);

                    if (residingCell is LifeCell) {
                        LifeCell residingLifeCell = (LifeCell) residingCell;
                        LifeCell garrisonedCell;

                        if (residingLifeCell.Points > lifeCell.Points) {
                            garrisonedCell = lifeCell;
                        } else {
                            garrisonedCell = residingLifeCell;
                        }
                    }
                } 
            }

            return (currentDay == Statistics.Day) && (currentGeneration == Statistics.Generation);
        });
    }

    public override void Update(GameTime gameTime) {
        if ((gameTime.TotalGameTime.TotalMilliseconds - lastUpdate) >= Constants.UpdateRate) {

            NextDay();

            lastUpdate = gameTime.TotalGameTime.TotalMilliseconds;
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
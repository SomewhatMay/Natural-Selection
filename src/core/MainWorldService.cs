#nullable enable

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

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
    List<Cell> garrisonedCells;
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
        this.garrisonedCells = new List<Cell>();

        NextGeneration();
    }

    private bool isFoodFinished() {
        if (Statistics.FoodAlive > 0) {
            return true;
        } else {
            return false;
        }
    }

    private void updateLeaderboard(List<LifeCell> leaderboard, Grid currentGrid) {
        currentGrid.IterateExclusiveAll((Cell cell) => {
            if (cell is LifeCell) {
                LifeCell currentCell = (LifeCell) cell;

                int leaderboardLength = leaderboard.Count;

                if (! (leaderboardLength > 10 && currentCell.Points < leaderboard[leaderboardLength - 1].Points)) {
                    bool inserted = false;

                    for (int index = 0; index < leaderboardLength; ++index) {
                        LifeCell topCell = leaderboard[index];

                        if (currentCell.Points > topCell.Points) {
                            leaderboard.Insert(index, currentCell);
                            inserted = true;
                            ++leaderboardLength;

                            break;
                        }
                    }

                    if ((!inserted) && (leaderboardLength < 10)) {
                        leaderboard.Add(currentCell);
                        ++leaderboardLength;
                    }

                    if (leaderboardLength > 10) {
                        leaderboard.RemoveAt(leaderboardLength - 1);
                    }
                }
            }
            
            return true;
        });
    }

    public void NextGeneration(Grid? currentGrid = null, Grid? garrisonedGrid = null) {
        ++Statistics.Generation; // Let's increment the generation
        Statistics.Day = 0;
        Statistics.CellsAlive = 0;
        Statistics.CellsGarrisoned = 0;
        Statistics.FoodAlive = 0;
        Statistics.FoodEaten = 0;

        // Determine the top 10 cells
        // [1 ...] = LifeCell;
        List<LifeCell> leaderboard = new List<LifeCell>();

        if ((currentGrid != null) && (garrisonedGrid != null)) {
            updateLeaderboard(leaderboard, currentGrid);
            updateLeaderboard(leaderboard, garrisonedGrid);
        }

        // Load all the cells
        #nullable enable
        gameGrid.FillGrid((int x, int y) => {
            int chance = gameRandom.Next(0, Constants.ChanceMax);

            if (Constants.LifeCellRandomValues.Contains(chance)) {
                Cell lifeCell = new LifeCell(
                    new Point(x, y)
                );

                return lifeCell;
            } else if (Constants.FoodCellRandomValues.Contains(chance)) {
                Cell foodCell = new FoodCell(
                    new Point(x, y)
                );

                return foodCell;
            }

            return null;
        });
    }

    public void NextDay() {

        ++Statistics.Day; // increment the day

        int currentDay = Statistics.Day;
        int currentGeneration = Statistics.Generation;

        // Create a 2D array for all the cells that will be repasted
        Cell?[,] repastableCells = new Cell?[Constants.WorldExtents.X, Constants.WorldExtents.Y];

        gameGrid.IterateExclusiveAll((Cell cell) => {
            bool repaste = false; // whether we should repaste the cell into the next grid

            if (cell is LifeCell) {
                LifeCell lifeCell = (LifeCell) cell;

                if (lifeCell.Alive) {
                    lifeCell.Next();
                    repaste = true;

                    // Lets check if two cells overlap, and if they do, then let's garrison the one with the lower points

                    Cell residingCell = gameGrid.GetInGrid(lifeCell.Position);

                    if ((residingCell is LifeCell) && (!residingCell.Equals(lifeCell))) {
                        LifeCell residingLifeCell = (LifeCell) residingCell;
                        LifeCell garrisonedCell;

                        if (residingLifeCell.Points > lifeCell.Points) {
                            garrisonedCell = lifeCell;
                            repaste = false;
                        } else {
                            garrisonedCell = residingLifeCell;

                            // Remove the garrisoned cell from being repastable
                            repastableCells[garrisonedCell.Position.X, garrisonedCell.Position.Y] = null;
                        }

                        garrisonedCell.Points += Constants.RewardSetting.Death;
                        garrisonedCell.Alive = false;

                        ++Statistics.CellsGarrisoned;
                        --Statistics.CellsAlive;

                        garrisonedCells.Add(garrisonedCell);
                    } else if (residingCell is FoodCell) {
                        FoodCell residingFoodCell = (FoodCell) residingCell;

                        residingFoodCell.Alive = false;

                        ++Statistics.FoodEaten;
                        --Statistics.FoodAlive;

                        ++lifeCell.Points;

                        // Remove the food cell from being repastable
                        repastableCells[residingFoodCell.Position.X, residingFoodCell.Position.Y] = null;

                        if (isFoodFinished()) {
                            // break out of the whole loop because there is no more food left!
                            return false;
                        }
                    }
                } 
            } else if (cell is FoodCell) {
                FoodCell foodCell = (FoodCell) cell;

                if (foodCell.Alive) {
                    repaste = true;
                }
            }

            if (repaste) {
                repastableCells[cell.Position.X, cell.Position.Y] = cell;
            }

            return (currentDay == Statistics.Day) && (currentGeneration == Statistics.Generation);
        });

        

        if ((currentDay == Statistics.Day) && (currentGeneration == Statistics.Generation)) {
            // Let's empty the gamegrid and then fill it up with the cells to be repasted
            gameGrid.Clear();

            foreach (Cell? repastableCell in repastableCells) {
                if (repastableCell != null) {
                    gameGrid.InsertCell(repastableCell);
                }
            }
        }
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
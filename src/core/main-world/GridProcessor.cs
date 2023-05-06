using Classes.CellObjects;
using Classes.GameGrid;
using Constants;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NaturalSelectionRemastered.src.core.main_world;

public class ProcessedReturn
{
	// Determines the cells that will get repasted to the next generation
	public Cell[,] RepasteTable;
	
	// Determines the cells that will get ignored in the next generation
	public Cell[,] DestroyTable;

	public ProcessedReturn(int worldExtentsX, int worldExtentsY)
	{
		RepasteTable = new Cell[worldExtentsX, worldExtentsY];
		DestroyTable = new Cell[worldExtentsX, worldExtentsY];
	}
}

public class GridProcessor
{
	Thread thread;
	Grid mainGrid;

	CountdownEvent threadCountdownEvent;

	int currentDay;
	int currentGeneration;

	public GridProcessor(Grid mainGrid, CountdownEvent threadCountdownEvent, int x, int y, int width, int height)
	{
		this.mainGrid = mainGrid;
		this.threadCountdownEvent = threadCountdownEvent;

		thread = new Thread(() =>
		{
			mainGrid.IterateExclusiveRegion(
				x, // Top left X
				y, // Top left Y
				width, // Size X
				height, // Size Y

				(Cell cell) =>
				{
					return ProcessCell(cell);
				}
			);

			threadCountdownEvent.Signal();
		});
	}

	public ProcessedReturn Process(int currentDay, int currentGeneration)
	{
		ProcessedReturn processedReturn = new ProcessedReturn();

		this.currentDay = currentDay;
		this.currentGeneration = currentGeneration;

		this.thread.Start();
	}

	private bool ProcessCell(Cell cell)
	{
		bool repaste = false; // whether we should repaste the cell into the next grid

		if (cell is LifeCell)
		{
			LifeCell lifeCell = (LifeCell)cell;

			if (lifeCell.Alive)
			{
				lifeCell.Next();
				repaste = true;

				// Lets check if two cells overlap, and if they do, then let's garrison the one with the lower points

				Cell residingCell = mainGrid.GetInGrid(lifeCell.Position);

				if ((residingCell is LifeCell) && (!residingCell.Equals(lifeCell)))
				{
					LifeCell residingLifeCell = (LifeCell)residingCell;
					LifeCell garrisonedCell;

					if (residingLifeCell.Points > lifeCell.Points)
					{
						garrisonedCell = lifeCell;
						repaste = false;
					}
					else
					{
						garrisonedCell = residingLifeCell;

						// Remove the garrisoned cell from being repastable
						repsateTable[garrisonedCell.Position.X, garrisonedCell.Position.Y] = null;
					}

					garrisonedCell.Points += GameConstants.RewardSetting.Death;
					garrisonedCell.Alive = false;

					++Statistics.CellsGarrisoned;
					--Statistics.CellsAlive;

					garrisonedCells.Add(garrisonedCell);
				}
				else if (residingCell is FoodCell)
				{
					FoodCell residingFoodCell = (FoodCell)residingCell;

					residingFoodCell.Alive = false;

					++Statistics.FoodEaten;
					--Statistics.FoodAlive;

					++lifeCell.Points;

					// Remove the food cell from being repastable
					repsateTable[residingFoodCell.Position.X, residingFoodCell.Position.Y] = null;

					if (isFoodFinished())
					{
						// break out of the whole loop because there is no more food left!
						return false;
					}
				}
			}
		}
		else if (cell is FoodCell)
		{
			FoodCell foodCell = (FoodCell)cell;

			if (foodCell.Alive)
			{
				repaste = true;
			}
		}

		if (repaste)
		{
			repsateTable[cell.Position.X, cell.Position.Y] = cell;
		}

		return (currentDay == Statistics.Day) && (currentGeneration == Statistics.Generation);
	}
}

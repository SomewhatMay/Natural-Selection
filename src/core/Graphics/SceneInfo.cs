using Microsoft.Xna.Framework;
using Classes;
using GUI;
using System.Collections.Generic;
using System.Threading;
using Constants;
using System.Runtime.InteropServices;
using System;
using Core;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Net.Http.Headers;
using System.Reflection;
using static Core.MainWorld;

namespace Core.Graphics;

public class SceneInfo {
	public Point Position = new Point(10, 30);
	public Point Size = new Point(GameConstants.SidebarWidth - 20, 300);

	private Dictionary<string, GraphicalInstance> children;
	private MainWorld mainWorld;

	private GraphicsService graphicsService;
	private Frame sidebarFrame;

	public SceneInfo() {
		children = new Dictionary<string, GraphicalInstance>();
	}

	public void Init(GraphicsService graphicsService, Dictionary<string, Service> loadedServices) {
		this.graphicsService = graphicsService;
		mainWorld = (MainWorld)loadedServices["MainWorld"];
	}

	Frame entriesParent;
	public void LoadContent(Frame sidebarFrame) {
		this.sidebarFrame = sidebarFrame;

		// Load all the objects needed
		Frame background = new Frame(
				Position,
				Size
		);
		background.Parent = sidebarFrame;
		background.BackgroundColor = new Color(.4f, .4f, .4f);
		background.Name = "Scene Info Background";

		FramedTextObject title = new FramedTextObject(
			new Point(5, 5),
			new Point(background.Size.X - 10, 20),
			"Scene Info"
		);
		title.Allignment = TextAllignment.CENTER;
		title.Name = "Scene Info Title";
		title.TextLabel.Position = new Point(5, 5);
		title.BackgroundColor = new Color(.6f, .6f, .6f);
		title.TextColor = Color.White;
		title.Parent = background;

		entriesParent = new Frame(
			new Point(5, title.Size.Y + title.Position.Y + 5),
			new Point(title.Size.X, background.Size.Y - (title.Size.Y + title.Position.Y + 10)),
			background
		);
		entriesParent.BackgroundColor = new Color(.6f, .6f, .6f);

		// Let's add all the created ui objects in our children
		children.Add("Background", background);
		children.Add("Title", title);
		children.Add("EntriesParent", entriesParent);

		LoadEntries();

		// Let's draw the children
		foreach (var (_, child) in children) {
			graphicsService.AddInstance(child);
		}
	}

	// Just a helper function that loads all the entries :)

	FrameEntry UpdateRate;
	FrameEntry GameState;
	FrameEntry Generation;
	FrameEntry Day;
	FrameEntry CellInfo;
	FrameEntry FoodInfo;
	FramedTextObject PauseButton;

	int entryHeight = 20;
	Color EntryBackgroundColor = new Color(.4f, .4f, .4f);
	Color ValueColor = new Color(.79f, .29f, .18f);
	Point entrySize;
	Point TitleLabelPadding = new Point(5, 5);
	Point ValueLabelPadding = new Point(-5, 5);
	int entryPad = 5;

	private int getPositionByIndex(int index) 
	{
		return ((index + 1) * entryPad) + (index * entryHeight);
	}

	private FrameEntry createSceneEntry(string title, int index) 
	{
		Point position = new Point(entryPad, getPositionByIndex(index));
		FrameEntry entry = new FrameEntry(position, entrySize);
		entry.Parent = entriesParent;
		entry.TitleText = title;
		entry.BackgroundColor = EntryBackgroundColor;
		entry.ValueColor = ValueColor;
		entry.TitleColor = Color.White;

		entry.TitleLabel.Position = TitleLabelPadding;
		entry.ValueLabel.Position = ValueLabelPadding;

		children.Add(title, entry);

		return entry;
	}

	private void LoadEntries() {
		entrySize = new Point(entriesParent.Size.X - 10, entryHeight);

		// Let's create all of the entries
		UpdateRate = createSceneEntry("Update Rate", 0);
		GameState = createSceneEntry("Game State", 1);
		Generation = createSceneEntry("Generation", 2);
		Day = createSceneEntry("Day", 3);
		CellInfo = createSceneEntry("Cell Info", 4);
		FoodInfo = createSceneEntry("Food Info", 5);

		// Let's create a text button for the pause game
		Point position = new Point(entryPad, getPositionByIndex(6));
		PauseButton = new FramedTextObject(position, entrySize);
		PauseButton.Parent = entriesParent;
		PauseButton.Text = "Pause Game";
		PauseButton.TextColor = Color.White;
		PauseButton.TextLabel.Allignment = TextAllignment.CENTER;
		PauseButton.BackgroundColor = EntryBackgroundColor;
		PauseButton.TextLabel.Position = new Point(0, 5);

		children.Add("Pause Button", PauseButton);

		// Let's connect a method that gets called upon it being clicked
		PauseButton.MakeClickableInstance();
		PauseButton.SetOnClicked((bool alreadyClicked, int mouseX, int mouseY) =>
		{
			if (alreadyClicked)
				return;

			mainWorld.TogglePause();

			if (mainWorld.GameState == Other.GameState.RUNNING)
			{
				PauseButton.Text = "Pause Game";
			}
			else
			{
				PauseButton.Text = "Play Game";
			}
		});

		// Connect listeners to all the entries
		MainWorld.Statistics.OnUpdateRateChanged += OnUpdateRateChanged;
		OnUpdateRateChanged(EventArgs.Empty);

		mainWorld.OnGameStateChanged += OnGameStateChanged;
		OnGameStateChanged(EventArgs.Empty);

		MainWorld.Statistics.OnGenerationChanged += OnGenerationChanged;
		OnGenerationChanged(EventArgs.Empty);

		MainWorld.Statistics.OnDayChanged += OnDayChanged;
		OnDayChanged(EventArgs.Empty);

		MainWorld.Statistics.OnFoodAliveChanged += OnFoodInfoChanged;
		MainWorld.Statistics.OnFoodEatenChanged += OnFoodInfoChanged;
		OnFoodInfoChanged(EventArgs.Empty);

		MainWorld.Statistics.OnCellsAliveChanged += OnCellInfoChanged;
		MainWorld.Statistics.OnCellsGarrisonedChanged += OnCellInfoChanged;
		OnCellInfoChanged(EventArgs.Empty);
	}

	// Create listeners for each of the entries
	private void OnUpdateRateChanged(EventArgs _) => UpdateRate.ValueText = MainWorld.Statistics.UpdateRate.ToString();
	private void OnGameStateChanged(EventArgs _) => GameState.ValueText = mainWorld.GameState.ToString();
	private void OnGenerationChanged(EventArgs _) => Generation.ValueText = MainWorld.Statistics.Generation.ToString();
	private void OnDayChanged(EventArgs _) => Day.ValueText = MainWorld.Statistics.Day.ToString();
	private void OnCellInfoChanged(EventArgs _) => CellInfo.ValueText = MainWorld.Statistics.CellsAlive.ToString() + $" ({MainWorld.Statistics.CellsGarrisoned})";
	private void OnFoodInfoChanged(EventArgs _) => FoodInfo.ValueText = MainWorld.Statistics.FoodAlive.ToString() + $" ({MainWorld.Statistics.FoodEaten})";
}


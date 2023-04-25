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

		children.Add(title, entry);

		return entry;
	}

	private void LoadEntries() {
		entrySize = new Point(entriesParent.Size.X - 10, entryHeight);

		GameState = createSceneEntry("Game State", 0);
		Generation = createSceneEntry("Generation", 1);
		Day = createSceneEntry("Day", 2);
		CellInfo = createSceneEntry("Cell Info", 3);
		FoodInfo = createSceneEntry("Food Info", 4);

		Point position = new Point(entryPad, getPositionByIndex(5));
		PauseButton = new FramedTextObject(position, entrySize);
		PauseButton.Parent = entriesParent;
		PauseButton.Text = "Pause Game";
		PauseButton.TextColor = Color.White;
		PauseButton.TextLabel.Allignment = TextAllignment.CENTER;
		PauseButton.BackgroundColor = EntryBackgroundColor;

		children.Add("Pause Button", PauseButton);

		PauseButton.MakeClickableInstance();
		PauseButton.SetOnClicked((bool alreadyClicked, int mouseX, int mouseY) => {
			if (alreadyClicked)
				return;

			mainWorld.TogglePause();

			if (mainWorld.GameState == Other.GameState.RUNNING) {
				PauseButton.Text = "Pause Game";
			} else {
				PauseButton.Text = "Play Game";
			}
		});
	}

	public void Update(GameTime gameTime) {
		GameState.ValueText = mainWorld.GameState.ToString();
		Generation.ValueText = MainWorld.Statistics.Generation.ToString();
		Day.ValueText = MainWorld.Statistics.Day.ToString();
		CellInfo.ValueText = MainWorld.Statistics.FoodAlive.ToString() + $" ({MainWorld.Statistics.CellsGarrisoned})";
		FoodInfo.ValueText = MainWorld.Statistics.Generation.ToString() + $" ({MainWorld.Statistics.FoodEaten})";
	}
}
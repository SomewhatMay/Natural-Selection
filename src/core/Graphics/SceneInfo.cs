using Microsoft.Xna.Framework;
using Classes;
using GUI;
using System.Collections.Generic;
using System.Threading;
using Constants;
using System.Runtime.InteropServices;
using System;

namespace Core.Graphics;

public class SceneInfo {
	public Point Position = new Point(10, 30);
	public Point Size = new Point(GameConstants.SidebarWidth - 20, 300);

	private Dictionary<string, GraphicalInstance> children;
	private Dictionary<string, Service> loadedServices;

	private GraphicsService graphicsService;
	private Frame sidebarFrame;

	public SceneInfo() {
		children = new Dictionary<string, GraphicalInstance>();
    }

	public void Init(GraphicsService graphicsService, Dictionary<string, Service> loadedServices);
	{
		this.graphicsService = graphicsService;
		this.loadedServices = loadedServices;
	}

	public void LoadContent(Frame sidebarFrame)
	{
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
		title.label.Position = new Point(5, 5);
		title.BackgroundColor = new Color(.6f, .6f, .6f);
		title.TextColor = Color.White;
		title.Parent = background;

		Frame entriesParent = new Frame(
			new Point(5, title.Size.Y + title.Position.Y + 5),
			new Point(title.Size.X, background.Size.Y - (title.Size.Y + title.Position.Y + 10)),
			background
		);
		entriesParent.BackgroundColor = new Color(.6f, .6f, .6f);

		LoadEntries();

		// Let's add all the created ui objects in our children
		children.Add("Background", background);
		children.Add("Title", title);
		children.Add("EntriesParent", entriesParent);

		// Let's draw the children
		foreach (var (_, child) in children)
		{
			graphicsService.AddInstance(child);
		}
	}

	// Just a helper function that loads all the entries :)
	private void LoadEntries()
	{

	}
}
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
	private static Point startingPosition = new Point(10, 25);

	private Dictionary<string, GraphicalInstance> children;

	private GraphicsService graphicsService;
	private Frame sidebarFrame;

	public SceneInfo() {
		children = new Dictionary<string, GraphicalInstance>();
    }

	public void Init(GraphicsService graphicsService)
	{
		this.graphicsService = graphicsService;
	}

	public void LoadContent(Frame sidebarFrame)
	{
		this.sidebarFrame = sidebarFrame;

		// Load all the objects needed
		Frame background = new Frame(
				startingPosition,
				new Point(GameConstants.SidebarWidth - 20, 300)
		);
		background.Parent = sidebarFrame;
		background.Name = "scene Info background";

		FramedTextObject title = new FramedTextObject(
			new Point(2, 2),
			new Point(background.Size.X - 4, 14),
			"Scene Info"
		);
		title.Name = "Scene Info Title";
		title.label.Position = new Point(2, 2);
		title.BackgroundColor = new Color(.5f, .5f, .5f);
		title.TextColor = Color.White;
		title.Parent = background;

		// Let's add all the created ui objects in our children
		children.Add("Background", background);
		children.Add("Title", title);

		// Let's draw the children
		foreach (var (_, child) in children)
		{
			graphicsService.AddInstance(child);
		}
	}
}
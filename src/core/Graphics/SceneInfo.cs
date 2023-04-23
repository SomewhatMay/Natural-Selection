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

		TextLabel title = new TextLabel(
			new Point(20, 10),
			new Point(background.Size.X - 10, 15),
			"Scene Info"
		);
		title.Name = "Scene Info Title";
		title.Parent = background;
		title.TextColor = Color.Black;

		children.Add("Background", background);
		children.Add("Title", title);

		foreach (var (_, child) in children)
		{
			graphicsService.AddInstance(child);
		}
	}
}
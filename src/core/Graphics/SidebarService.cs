using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Classes;
using Constants;
using GUI;

namespace Core.Graphics;

public class SidebarService : Service {
    private Dictionary<string, GraphicalInstance> children;
    private GraphicsService graphicsService;

	private SceneInfo sceneInfo;
	private SelectionInfo selectionInfo;

    public SidebarService(Game game) : base(game) {
        children = new Dictionary<string, GraphicalInstance>();
		sceneInfo = new SceneInfo();
		selectionInfo = new SelectionInfo();
	}

    public override void Init(Dictionary<string, Service> loadedServices) {
        base.Init(loadedServices);

        graphicsService = (GraphicsService) loadedServices["Graphics"];
		sceneInfo.Init(graphicsService, loadedServices);
		selectionInfo.Init(graphicsService, loadedServices, sceneInfo);
    }

    public override void LoadContent() {
        base.LoadContent();

		// Let's create all the UI objects
		Frame sidebarBackground = new Frame(
			new Point(GameConstants.WorldPixelWidth, 0),
			new Point(GameConstants.SidebarWidth, GameConstants.WindowSize.Y),
			Color.Gray
		);

		TextLabel title = new TextLabel(
			new Point(10, 10),
			new Point(GameConstants.SidebarWidth, 10),
			$"Natural Selection [REMASTERED v{GameConstants.version}]"
		);
		title.TextColor = Color.White;
		title.Parent = sidebarBackground;

		//Let's parent the objcets to our childrens table
		children.Add("Background", sidebarBackground);
		children.Add("Title", title);

		// Insert each object to the graphics service so it gets loaded
		graphicsService.AddInstance(sidebarBackground);
		graphicsService.AddInstance(title);

		sceneInfo.LoadContent(sidebarBackground);
		selectionInfo.LoadContent(sidebarBackground);
	}
}
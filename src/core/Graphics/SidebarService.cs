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

    public SidebarService(Game game) : base(game) {
        children = new Dictionary<string, GraphicalInstance>();
    }

    public override void Init(Dictionary<string, Service> loadedServices) {
        base.Init(loadedServices);

        graphicsService = (GraphicsService) loadedServices["Graphics"];
    }

    public override void LoadContent() {
        base.LoadContent();

        children.Add("Sidebar background", new Frame(
            new Point(GameConstants.WorldPixelWidth, 0),
            new Point(GameConstants.SidebarWidth, GameConstants.WindowSize.Y),
            Color.Gray
        ));
        children.Add("Test inside background", new Frame(
            new Point(10, 10),
            new Point(100, 100),
            Color.Red,
            children["Sidebar background"]
        ));
        graphicsService.AddInstance(children["Sidebar background"]);
        graphicsService.AddInstance(children["Test inside background"]);
    }
}
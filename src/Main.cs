using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using Core.Graphics;
using Core.Schedule;
using Constants;
using Classes;
using Core;

namespace NaturalSelectionRemastered;

public class NaturalSelection : Game
{
	public GraphicsDeviceManager _graphics;
	public SpriteBatch _spriteBatch;

	public static SpriteFont TextFont { get; private set; }

	public Random gameRandom;

	public Dictionary<string, Service> loadedServices;

	public Service LoadService(string serviceName, Service service)
	{
		loadedServices.Add(serviceName, service);

		return service;
	}

	public NaturalSelection()
	{
		// initialize the constants 
		GameConstants.Initialize();

		_graphics = new GraphicsDeviceManager(this);
		_graphics.PreferredBackBufferWidth = GameConstants.WindowSize.X;
		_graphics.PreferredBackBufferHeight = GameConstants.WindowSize.Y;
		_graphics.SynchronizeWithVerticalRetrace = false;
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		this.IsFixedTimeStep = false;

		gameRandom = new Random((int)GameConstants.Seed);

		// let's load all the services
		loadedServices = new Dictionary<string, Service>();
		LoadService("MainWorld", new MainWorld(this, gameRandom));
		LoadService("Schedule", new ScheduleService(this, gameRandom));
		LoadService("Graphics", new GraphicsService(this));
		LoadService("Sidebar", new SidebarService(this));

		Console.WriteLine($"Starting Natural Selection [REMASTERED v{GameConstants.version}] with Seed {GameConstants.Seed}");
	}

	protected override void Initialize()
	{
		_graphics.ApplyChanges();
		// TODO: Add your initialization logic here

		// let's call initialize after all services are loaded
		foreach (KeyValuePair<string, Service> pair in loadedServices)
		{
			pair.Value.Init(loadedServices);
		}

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		TextFont = Content.Load<SpriteFont>("Fonts/Consolas");

		// TODO: use this.Content to load your game content here
		foreach (KeyValuePair<string, Service> pair in loadedServices)
		{
			pair.Value.LoadContent();
		}
	}

	protected override void Update(GameTime gameTime)
	{
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Keys.Escape))
			Exit();

		// TODO: Add your update logic here

		foreach (KeyValuePair<string, Service> pair in loadedServices)
		{
			pair.Value.Update(gameTime);
		}

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);

		// TODO: Add your drawing code here

		foreach (KeyValuePair<string, Service> pair in loadedServices)
		{
			pair.Value.Draw(gameTime);
		}


		base.Draw(gameTime);
	}
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using Constants;
using Classes;
using Core;

namespace NaturalSelectionRemastered;

public class Game1 : Game
{
    public GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;

    public Random gameRandom;

    public List<Service> loadedServices;

    public Service LoadService(Service service) {
        loadedServices.Add(service);

        return service;
    }

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        gameRandom = new Random();

        // initialize the constants 
        Constants.Constants.Initialize();

        // let's load all the services
        loadedServices = new List<Service>();
        LoadService(new MainWorld(this, gameRandom));
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        foreach (Service service in loadedServices) {
            service.LoadContent();
        }
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        foreach (Service service in loadedServices) {
            service.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        foreach (Service service in loadedServices) {
            service.Draw(gameTime);
        }


        base.Draw(gameTime);
    }
}

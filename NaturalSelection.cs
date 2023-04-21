using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using ConstantsNamespace;
using Classes;
using Core;

namespace NaturalSelectionRemastered;

public class NaturalSelection : Game
{
    public GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;

    public Random gameRandom;

    public Dictionary<string, Service> loadedServices;

    public Service LoadService(string serviceName, Service service) {
        loadedServices.Add(serviceName, service);

        return service;
    }

    public NaturalSelection() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        if (Constants.Seed == null) {
            gameRandom = new Random();
        } else {
            gameRandom = new Random((int) Constants.Seed);
        }

        // initialize the constants 
        Constants.Initialize();

        // let's load all the services
        loadedServices = new Dictionary<string, Service>();
        LoadService("MainWorld", new MainWorld(this, gameRandom));
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here

        // let's call initialize after all services are loaded
        foreach (KeyValuePair<string, Service> pair in loadedServices) {
            pair.Value.Init(loadedServices);
        }

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        foreach (KeyValuePair<string, Service> pair in loadedServices) {
            pair.Value.LoadContent();
        }
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        foreach (KeyValuePair<string, Service> pair in loadedServices) {
            pair.Value.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here

        foreach (KeyValuePair<string, Service> pair in loadedServices) {
            pair.Value.Draw(gameTime);
        }


        base.Draw(gameTime);
    }
}

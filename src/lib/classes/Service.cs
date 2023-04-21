using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Classes;

public abstract class Service {
    public Game game;
    protected Dictionary<string, Service> loadedServices;

    protected Service(Game game) {
        this.game = game;
    }

    // used for getting references to other services
    public virtual void Init(Dictionary<string, Service> loadedServices) => this.loadedServices = loadedServices; 

    // Used to load content such as textures
    public virtual void LoadContent() { }
    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(GameTime gameTime) { }
}

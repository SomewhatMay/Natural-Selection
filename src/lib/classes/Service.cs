using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Classes;

public abstract class Service {
    public Game game;

    protected Service(Game game) {
        this.game = game;
    }

    public virtual void Init(Dictionary<string, Service> loadedServices) { } // used for getting references to other services
    public virtual void LoadContent() { } // used to load content
    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(GameTime gameTime) { }
}

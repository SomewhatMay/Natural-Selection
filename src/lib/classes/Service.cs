using Microsoft.Xna.Framework;

namespace Classes;

public abstract class Service {
    public Game game;

    protected Service(Game game) {
        this.game = game;
    }

    public abstract void LoadContent();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(GameTime gameTime);

}

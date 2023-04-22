using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Classes;
using GUI;

namespace Core.Graphics;

public class GraphicsService : Service {
    private SpriteBatch spriteBatch;

    private Dictionary<int, GraphicalInstance> graphicalInstances;
    private int graphicalInstancesIndex = 0;

    public GraphicsService(Game game) : base(game) {
        graphicalInstances = new Dictionary<int, GraphicalInstance>();
        spriteBatch = new SpriteBatch(game.GraphicsDevice);
    }

    public int AddInstance(GraphicalInstance newInstance) {
        graphicalInstances[graphicalInstancesIndex] = newInstance;
        ++graphicalInstancesIndex;

        return graphicalInstancesIndex;
    }

    // Returns whether it successfully removed the instance or not
    public bool RemoveInstance(GraphicalInstance instanceToRemove) {
        foreach (var (index, instance) in graphicalInstances) {
            if (object.ReferenceEquals(instanceToRemove, instance)) {
                graphicalInstances[index] = null;

                return true;
            }
        }

        return false;
    } 

    public void RemoveInstance(int instanceIndex) {
        graphicalInstances[instanceIndex] = null;
    }

    public override void Draw(GameTime game) {
        spriteBatch.Begin();

        foreach (var (_, instance) in graphicalInstances) {
            instance.Draw(spriteBatch);
        }

        spriteBatch.End();
    }
}
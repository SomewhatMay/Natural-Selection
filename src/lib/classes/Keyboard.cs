using Microsoft.Xna.Framework.Input;

namespace Classes;

public class Keyboard {
    static KeyboardState currentKeyState;
    static KeyboardState previousKeyState;

    public static KeyboardState GetState()
    {
        previousKeyState = currentKeyState;
        currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
        return currentKeyState;
    }

    public static bool IsPressed(Keys key)
    {
        return currentKeyState.IsKeyDown(key);
    }

    public static bool HasClicked(Keys key)
    {
        return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
    }
}
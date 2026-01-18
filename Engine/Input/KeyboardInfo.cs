using Microsoft.Xna.Framework.Input;

namespace Engine.Input;

public class KeyboardInfo
{
    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    public KeyboardInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState = Keyboard.GetState();
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key) => CurrentState.IsKeyDown(key);
    
    public bool IsKeyUp(Keys key) => CurrentState.IsKeyDown(key);
    
    public bool IsKeyPressed(Keys key) => CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    
    public bool IsKeyReleased(Keys key) => CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);

}
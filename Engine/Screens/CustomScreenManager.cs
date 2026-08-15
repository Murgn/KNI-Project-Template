using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Engine.Screens;

public class CustomScreenManager : ScreenManager
{
    public void LateUpdate(GameTime gameTime)
    {
        IReadOnlyList<Screen> screens = this.Screens;
        for (int index = 0; index < screens.Count; ++index)
        {
            Screen screen = screens[index];
            if (screen is GameObjectScreen engineScreen && (screen.IsActive || screen.UpdateWhenInactive))
                engineScreen.LateUpdate(gameTime);
        }
    }
}
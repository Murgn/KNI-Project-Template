using Engine.Screens;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Engine.UI.Canvases;

public abstract class Canvas(Game game, GameObjectScreen screen)
{
    protected Game Game { get; private set; } = game;
    protected GameObjectScreen Screen { get; private set; } = screen;

    public abstract void Initialize();
}
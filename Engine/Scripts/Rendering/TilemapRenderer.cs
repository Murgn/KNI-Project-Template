using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tilemaps;
using MonoGame.Extended.Tilemaps.LDtk;
using MonoGame.Extended.Tilemaps.Rendering;

namespace Engine.Scripts.Rendering;

public class TilemapRenderer : Script
{
    public List<string> backgroundLayers = new List<string>();
    public List<string> foregroundLayers = new List<string>();
    
    public Tilemap Tilemap { get; private set; }
    public TilemapSpriteBatchRenderer TileRenderer { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        
        LDtkJsonParser parser = new LDtkJsonParser();
        Tilemap = parser.ParseFromFile("Content/tilemaps/Map1.ldtk", Runtime.GraphicsDevice);

        TileRenderer = new TilemapSpriteBatchRenderer();
        TileRenderer.LoadTilemap(Tilemap);
        TileRenderer.SpriteSortMode = SpriteSortMode.FrontToBack;
        TileRenderer.SamplerState = SamplerState.PointClamp;
    }
    
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        TileRenderer.Update(gameTime);
    }
}
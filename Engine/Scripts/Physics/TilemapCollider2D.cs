using System.Collections.Generic;
using Engine.Scripts.Rendering;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tilemaps;
using nkast.Aether.Physics2D.Dynamics;

namespace Engine.Scripts.Physics;

public class TilemapCollider2D : Script
{
    public string collisionLayerName = "Platforms";

    private Body body;

    public void Setup()
    {
        Tilemap tilemap = FindGameObjectByScript<TilemapRenderer>().GetScript<TilemapRenderer>().Tilemap;

        if (tilemap.Layers[collisionLayerName] is not TilemapTileLayer layer)
            return;

        bool[,] solid = new bool[layer.Width, layer.Height];
        for (int y = 0; y < layer.Height; y++)
            for (int x = 0; x < layer.Width; x++)
                solid[x, y] = layer.GetTile(x, y).HasValue;

        List<Rectangle> rects = GreedyMerge(solid, layer.Width, layer.Height);

        body = Runtime.PhysicsWorld.CreateBody(Vector2.Zero, 0f, BodyType.Static);

        foreach (Rectangle r in rects)
        {
            float w = r.Width * layer.TileWidth;
            float h = r.Height * layer.TileHeight;
            float cx = (r.X * layer.TileWidth + r.Width * layer.TileWidth * 0.5f);
            float cy = (r.Y * layer.TileHeight + r.Height * layer.TileHeight * 0.5f);

            body.CreateRectangle(w, h, 1.0f, new Vector2(cx, cy));
        }
    }

    private static List<Rectangle> GreedyMerge(bool[,] solid, int width, int height)
    {
        bool[,] visited = new bool[width, height];
        List<Rectangle> result = new();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!solid[x, y] || visited[x, y]) continue;

                int w = 1;
                while (x + w < width && solid[x + w, y] && !visited[x + w, y]) w++;

                int h = 1;
                bool canExtend = true;
                while (y + h < height && canExtend)
                {
                    for (int xx = x; xx < x + w; xx++)
                    {
                        if (!solid[xx, y + h] || visited[xx, y + h]) { canExtend = false; break; }
                    }
                    if (canExtend) h++;
                }

                for (int yy = y; yy < y + h; yy++)
                    for (int xx = x; xx < x + w; xx++)
                        visited[xx, yy] = true;

                result.Add(new Rectangle(x, y, w, h));
            }
        }

        return result;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (body != null)
            Runtime.PhysicsWorld.Remove(body);
    }
}
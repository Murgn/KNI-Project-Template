using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics;

public class TextureAtlas
{
    public Texture2D Texture;
    private Dictionary<string, Animation> animations;
    
    private Dictionary<string, TextureRegion> regions;

    public TextureAtlas()
    {
        regions = new Dictionary<string, TextureRegion>();
        animations = new Dictionary<string, Animation>();
    }

    public TextureAtlas(Texture2D texture)
    {
        Texture = texture;
        regions = new Dictionary<string, TextureRegion>();
        animations = new Dictionary<string, Animation>();
    }

    public TextureRegion AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion region = new TextureRegion(Texture, x, y, width, height);
        regions.Add(name, region);
        return regions[name];
    }

    public TextureRegion GetRegion(string name) => regions[name];

    public bool RemoveRegion(string name) => regions.Remove(name);

    public void ClearRegions() => regions.Clear();

    public Animation AddAnimation(string name, Animation animation)
    {
        animations.Add(name, animation);
        return animations[name];
    }

    public Animation GetAnimation(string name) => animations[name];

    public bool RemoveAnimation(string name) => animations.Remove(name);
    
    public void ClearAnimations() => animations.Clear();

    public Sprite CreateSprite(string name)
    {
        TextureRegion region = GetRegion(name);
        return new Sprite(region);
    }

    public AnimatedSprite CreateAnimatedSprite(string name)
    {
        Animation animation = GetAnimation(name);
        return new AnimatedSprite(animation);
    }
    
    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        TextureAtlas atlas = new TextureAtlas();

        string filePath = Path.Combine(content.RootDirectory, fileName);

        using Stream stream = TitleContainer.OpenStream(filePath);
        using XmlReader reader = XmlReader.Create(stream);
        
        XDocument doc = XDocument.Load(reader);
        XElement root = doc.Root;

        string texturePath = root.Element("Texture").Value;
        atlas.Texture = content.Load<Texture2D>(texturePath);

        var regions = root.Element("Regions")?.Elements("Region");
        if (regions != null)
        {
            foreach (var region in regions)
            {
                string name = region.Attribute("name")?.Value;
                int x = int.Parse(region.Attribute("x")?.Value ?? "0");
                int y = int.Parse(region.Attribute("y")?.Value ?? "0");
                int width = int.Parse(region.Attribute("width")?.Value ?? "0");
                int height = int.Parse(region.Attribute("height")?.Value ?? "0");
                        
                if(!string.IsNullOrEmpty(name))
                    atlas.AddRegion(name, x, y, width, height);
            }
        }

        var animationElements = root.Element("Animations")?.Elements("Animation");
        if (animationElements != null)
        {
            foreach (var animationElement in animationElements)
            {
                string name = animationElement.Attribute("name")?.Value;
                float delayInMilliseconds = float.Parse(animationElement.Attribute("delay")?.Value ?? "0");
                TimeSpan delay = TimeSpan.FromMilliseconds(delayInMilliseconds);

                List<TextureRegion> frames = new List<TextureRegion>();

                var frameElements = animationElement.Elements("Frame");

                if (frameElements != null)
                {
                    foreach (var frameElement in frameElements)
                    {
                        string regionName = frameElement.Attribute("region").Value;
                        TextureRegion region = atlas.GetRegion(regionName);
                        frames.Add(region);
                    }
                }

                Animation animation = new Animation(frames, delay);
                atlas.AddAnimation(name, animation);
            }
        }

        return atlas;
    }
}
using Microsoft.Xna.Framework;

namespace Engine.Maths;

public static class VectorHelpers
{
    public static Vector2 Parse2(this string value)
    {
        value = value.Trim('{', '}');
        var parts = value.Split(',');

        float x = float.Parse(parts[0].Split(':')[1]);
        float y = float.Parse(parts[1].Split(':')[1]);

        return new Vector2(x, y);
    }
    
    public static Vector3 Parse3(this string value)
    {
        value = value.Trim('{', '}');
        var parts = value.Split(',');

        float x = float.Parse(parts[0].Split(':')[1]);
        float y = float.Parse(parts[1].Split(':')[1]);
        float z = float.Parse(parts[2].Split(':')[1]);

        return new Vector3(x, y, z);
    }
}
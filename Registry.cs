using System.Collections.Frozen;
using Raylib_cs;

public static class Registry
{
    public static FrozenDictionary<TerrainType, Texture2D> Terrain {get; private set;}

    public static void Init()
    {
        var terrainDict = new Dictionary<TerrainType, Texture2D>
        {
            { TerrainType.Plains, Raylib.LoadTexture("assets/tiles/plains.png") }
        };

        Terrain = terrainDict.ToFrozenDictionary();
    }
}
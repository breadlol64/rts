using System.Collections.Frozen;
using Raylib_cs;

public static class Registry
{
    public static FrozenDictionary<TerrainType, Texture2D> Terrain {get; private set;}
    public static FrozenDictionary<BuildingType, BuildingDef> Buildings {get; private set;}

    public static void Init()
    {
        var terrainDict = new Dictionary<TerrainType, Texture2D>
        {
            { TerrainType.Plains, Raylib.LoadTexture("assets/tiles/plains.png") },
            { TerrainType.Water, Raylib.LoadTexture("assets/tiles/water.png") },
            { TerrainType.Sand, Raylib.LoadTexture("assets/tiles/sand.png") },
        };

        Terrain = terrainDict.ToFrozenDictionary();

        var buildingsDict = new Dictionary<BuildingType, BuildingDef>
        {
            { BuildingType.Sawmill, new BuildingDef
                {
                    cost = new Dictionary<ResourceType, int> { { ResourceType.Wood, 10 } },
                    production = new Dictionary<ResourceType, int> { { ResourceType.Wood, 5 } },
                    texture = Raylib.LoadTexture("assets/buildings/sawmill.png"),
                    name = "Sawmill"
                }
            },
            { BuildingType.Farm, new BuildingDef
                {
                    cost = new Dictionary<ResourceType, int> { { ResourceType.Wood, 5 } },
                    production = new Dictionary<ResourceType, int> { { ResourceType.Food, 10 } },
                    texture = Raylib.LoadTexture("assets/buildings/farm.png"),
                    name = "Farm"
                }
            }
        };

        Buildings = buildingsDict.ToFrozenDictionary();
    }
}
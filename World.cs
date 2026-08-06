using Raylib_cs;
using static Globals;

public class World
{
    Tile[] world;
    Dictionary<(int x, int y), Building> buildings = new();
    public static int selectedX = -1;
    public static int selectedY = -1;

    float tickTimer = 0.0f;
    const float tickInterval = 1.0f; // seconds

    public World()
    {
        world = new Tile[WorldSize*WorldSize];

        var noise = new FastNoiseLite(Raylib.GetRandomValue(0, int.MaxValue));
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        for (int y = 0; y < WorldSize; y++)
        {
            for (int x = 0; x < WorldSize; x++)
            {
                float scale = 2.0f;
                var value = noise.GetNoise(x * scale, y * scale);
                TerrainType terrain;
                if (value < -0.4)
                    terrain = TerrainType.Water;
                else if (value < 0.0)
                    terrain = TerrainType.Sand;
                else
                    terrain = TerrainType.Plains;

                world[y * WorldSize + x] = new Tile(x, y, terrain);
            }
        }
    }

    public bool TryPlaceBuilding(BuildingType type, int x, int y)
    {
        if (buildings.ContainsKey((x, y)))
            return false;

        buildings[(x, y)] = new Building(type, x, y);
        return true;
    }

    public void Draw()
    {
        foreach (var tile in world)
        {
            tile.Draw();
        }
        foreach (var building in buildings.Values)
        {
            building.Draw();
        }
    }

    public void Update(float deltaTime)
    {
        tickTimer += deltaTime;
        if (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;
            Tick();
        }
    }

    public void Tick()
    {
        foreach (var building in buildings.Values)
        {
            foreach (var resource in building.def.production)
            {
                Game.playerResources[resource.Key] += resource.Value;
            }
        }
    }
}
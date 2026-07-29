using Raylib_cs;
using static Globals;

public class World
{
    Tile[] world;

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

    public void Draw()
    {
        foreach (var tile in world)
        {
            tile.Draw();
        }
    }
}
using static Globals;

public class World
{
    Tile[] world;

    public World()
    {
        world = new Tile[WorldSize*WorldSize];
        for (int y = 0; y < WorldSize; y++)
        {
            for (int x = 0; x < WorldSize; x++)
            {
                world[y * WorldSize + x] = new Tile(x, y, TerrainType.Plains);
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
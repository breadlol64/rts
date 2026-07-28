public class World
{
    Tile[] world;

    public World()
    {
        world = new Tile[100*100];
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                world[y * 100 + x] = new Tile(x, y, TerrainType.Plains);
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
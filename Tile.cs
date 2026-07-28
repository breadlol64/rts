using static Globals;
using Graphics;
public enum TerrainType
{
    Plains,
    Water,
    Sand,
}

public class Tile
{
    public int x, y; // isometric coords
    public TerrainType terrain;
    public Sprite sprite;

    public Tile(int ix, int iy, TerrainType t)
    {
        x = ix;
        y = iy;
        terrain = t;

        IsoToWorld(x, y, out int px, out int py);

        sprite = new Sprite(Registry.Terrain[terrain], px, py);
    }

    public void Draw()
    {
        sprite.Draw();
    }

    public static (int x, int y) IsoToWorld(int ix, int iy)
    {
        return ((ix-iy) * (TileWidth / 2), (ix+iy) * (TileHeight / 2));
    }

    public static void IsoToWorld(int ix, int iy, out int x, out int y)
    {
        x = (ix-iy) * (TileWidth / 2);
        y = (ix+iy) * (TileHeight / 2);
    }
}
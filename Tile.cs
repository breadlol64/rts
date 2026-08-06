using static Globals;
using Graphics;
using Raylib_cs;
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
        if (x == World.selectedX && y == World.selectedY)
        {
            sprite.Draw(Color.Gray);
            return;
        }
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

    public static (int x, int y) WorldToIso(int wx, int wy)
    {
        float hw = TileWidth / 2.0f;
        float hh = TileHeight / 2.0f;

        float fx = wx / hw + wy / hh;
        float fy = wy / hh - wx / hw;
        int tx = (int)Math.Floor(fx / 2.0f);
        int ty = (int)Math.Floor(fy / 2.0f);

        float tile_ox = wx - (tx - ty) * hw;
        float tile_oy = wy - (tx + ty) * hh;
        float nx = tile_ox / TileWidth;
        float ny = tile_oy / TileHeight;

        if (nx + ny < 0.5f) tx -= 1;
        if (nx - ny > 0.5f) tx += 1;
        if (ny - nx > 0.5f) ty += 1;
        if (nx + ny > 1.5f) ty += 1;

        return (tx, ty);
    }
}
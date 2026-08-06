using Raylib_cs;
using Graphics;

public enum BuildingType
{
    Sawmill,
    Farm,
    None,
}

public struct BuildingDef
{
    public Dictionary<ResourceType, int> cost;
    public Dictionary<ResourceType, int> production;
    public Texture2D texture;
    public string name;
}

public class Building
{
    public BuildingType type;
    public BuildingDef def;
    public int x, y; // isometric coords
    public Sprite sprite;

    public Building(BuildingType t, int ix, int iy)
    {
        type = t;
        def = Registry.Buildings[type];
        x = ix;
        y = iy;

        Tile.IsoToWorld(x, y, out int px, out int py);

        sprite = new Sprite(def.texture, px, py);
    }

    public void Draw()
    {
        sprite.Draw();
    }
}
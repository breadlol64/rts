using Raylib_cs;

namespace Graphics;

public class Sprite(Texture2D texture, int x, int y)
{
    public Texture2D texture { get; set; } = texture;
    public int x { get; set; } = x;
    public int y { get; set; } = y;

    public void Draw() {
        Raylib.DrawTexture(texture, x, y, Color.White);
    }

    public void Draw(Color tint) {
        Raylib.DrawTexture(texture, x, y, tint);
    }
}
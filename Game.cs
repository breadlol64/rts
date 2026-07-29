using System.Numerics;
using Raylib_cs;

public class Game
{
    World world;
    Camera2D camera;

    public Game()
    {
        Raylib.InitWindow(1280, 720, "h");
        Registry.Init();
        world = new World();
        camera = new Camera2D(Raylib.GetScreenCenter(), Vector2.Zero, 0.0f, 1.0f);
    }

    ~Game()
    {
        Raylib.CloseWindow();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Update();

            Raylib.BeginDrawing();
            Raylib.BeginMode2D(camera);
            Raylib.ClearBackground(Color.DarkGray);

            Draw();

            Raylib.EndMode2D();
            DrawUI();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    void Update()
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Right))
        {
            camera.Target -= Raylib.GetMouseDelta() / camera.Zoom;
        }

        var mw = Raylib.GetMouseWheelMove();
        if (mw != 0)
        {
            var mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
            camera.Offset = Raylib.GetMousePosition();
            camera.Target = mouseWorldPos;
            camera.Zoom *= 1.0f + mw * 0.1f;
            camera.Zoom = Math.Clamp(camera.Zoom, 0.1f, 10.0f);
        }
    }

    void Draw()
    {
        world.Draw();
    }

    void DrawUI()
    {
        Raylib.DrawText(Raylib.GetFPS().ToString(), 10, 10, 20, Color.Red);
    }
}
using System.Numerics;
using Raylib_cs;

public class Game
{
    World world;
    Camera2D camera;
    public static Dictionary<ResourceType, int> playerResources = new Dictionary<ResourceType, int>
    {
        { ResourceType.Wood, 0 },
        { ResourceType.Food, 0 },
    };

    public Game()
    {
        Raylib.InitWindow(1280, 720, "h");
        Registry.Init();
        world = new World();
        world.TryPlaceBuilding(BuildingType.Sawmill, 5, 5);
        world.TryPlaceBuilding(BuildingType.Farm, 5, 6);
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

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            var mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
            (int x, int y) = Tile.WorldToIso((int)mouseWorldPos.X, (int)mouseWorldPos.Y);
            World.selectedX = x;
            World.selectedY = y;
        }

        world.Update(Raylib.GetFrameTime());
    }

    void Draw()
    {
        world.Draw();
    }

    void DrawUI()
    {
        Raylib.DrawText(Raylib.GetFPS().ToString(), 10, 10, 20, Color.Red);
        Raylib.DrawText($"Wood: {Game.playerResources[ResourceType.Wood]}", 10, 40, 20, Color.White);
        Raylib.DrawText($"Food: {Game.playerResources[ResourceType.Food]}", 10, 70, 20, Color.White);
    }
}
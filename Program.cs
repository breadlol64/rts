using System.Numerics;
using Raylib_cs;

internal class Program {
    

    [STAThread]
    public static void Main() {
        Raylib.InitWindow(1280, 720, "h");
        Registry.Init();

        World world = new World();
        Camera2D camera = new Camera2D(Raylib.GetScreenCenter(), Vector2.Zero, 0.0f, 1.0f);

        while (!Raylib.WindowShouldClose())
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

            Raylib.BeginDrawing();
            Raylib.BeginMode2D(camera);
            Raylib.ClearBackground(Color.DarkGray);

            world.Draw();

            Raylib.EndMode2D();
            Raylib.EndDrawing();
        }
    }
}
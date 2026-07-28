using Raylib_cs;

internal class Program {
    

    [STAThread]
    public static void Main() {
        Raylib.InitWindow(1280, 720, "h");
        Registry.Init();

        World world = new World();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.DarkGray);

            world.Draw();

            Raylib.EndDrawing();
        }
    }
}
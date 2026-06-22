package main

import "core:fmt"
import "core:math/noise"
import rl "vendor:raylib"

tile_width :: 64
tile_height :: 32

terrains: map[Terrain]rl.Texture2D

Terrain :: enum {
	Plains,
	Water,
	Sand,
}

Tile :: struct {
	x:       int,
	y:       int,
	terrain: Terrain,
}

world: [dynamic]Tile
camera: rl.Camera2D

main :: proc() {
	rl.InitWindow(1280, 720, "s")
	rl.SetTargetFPS(60)

	camera = rl.Camera2D {
		target   = {0, 0},
		offset   = {f32(rl.GetScreenWidth()) / 2.0, f32(rl.GetScreenHeight()) / 2.0},
		zoom     = 1.0,
		rotation = 0.0,
	}

	seed := rl.GetRandomValue(0, 2000000000)

	for y in 0 ..= 50 {
		for x in 0 ..= 50 {
			t := Terrain.Plains
			scale := 0.05
			value := noise.noise_2d(i64(seed), {f64(x) * scale, f64(y) * scale})
			if value < -0.4 do t = .Water
			else if value < 0.0 do t = .Sand
			append(&world, Tile{x = x, y = y, terrain = t})
		}
	}

	terrains = make(map[Terrain]rl.Texture2D)
	defer delete(terrains)
	terrains[.Plains] = rl.LoadTexture("assets/tiles/plains.png")
	terrains[.Water] = rl.LoadTexture("assets/tiles/water.png")
	terrains[.Sand] = rl.LoadTexture("assets/tiles/sand.png")

	for !rl.WindowShouldClose() {
		if rl.IsMouseButtonDown(.RIGHT) {
			md := rl.GetMouseDelta()
			camera.target -= md / camera.zoom
		}

		mw := rl.GetMouseWheelMove()
		if mw != 0 {
			mouseWorldPos := rl.GetScreenToWorld2D(rl.GetMousePosition(), camera)
			camera.offset = rl.GetMousePosition()
			camera.target = mouseWorldPos
			camera.zoom *= 1.0 + mw * 0.1
			camera.zoom = clamp(camera.zoom, 0.1, 10.0)
		}

		rl.BeginDrawing()
		rl.BeginMode2D(camera)
		rl.ClearBackground(rl.DARKGRAY)

		for tile in world {
			texture, exists := terrains[tile.terrain]
			if !exists {
				// shouldnt be possible
				fmt.println("terrain doesnt exist")
				return
			}

			rl.DrawTexture(
				texture,
				i32((tile.x - tile.y) * (tile_width / 2)),
				i32((tile.x + tile.y) * (tile_height / 2)),
				rl.WHITE,
			)
		}

		rl.EndMode2D()
		rl.EndDrawing()
	}
}

package main

import "core:fmt"
import "core:strings"
import rl "vendor:raylib"

tile_width :: 64
tile_height :: 32
world_size :: 100

world: [dynamic]Tile
camera: rl.Camera2D
selected_tile: ^Tile
menu: bool = false

main :: proc() {
	rl.InitWindow(1280, 720, "aaa")
	rl.SetTargetFPS(60)

	camera = rl.Camera2D {
		target   = {0, 0},
		offset   = {f32(rl.GetScreenWidth()) / 2.0, f32(rl.GetScreenHeight()) / 2.0},
		zoom     = 1.0,
		rotation = 0.0,
	}

	init_tiles()
	defer delete(terrains)
	init_buildings()
	defer delete(buildings)

	playerResources[.Wood] = 100
	playerResources[.Food] = 20

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

		mp := rl.GetMousePosition()
		if rl.IsMouseButtonPressed(.LEFT) && !(mp.y < 100 && menu) {
			mwp := rl.GetScreenToWorld2D(mp, camera)
			tx, ty := world_to_tile(mwp)
			idx := ty * world_size + tx
			if idx >= 0 && idx < len(world) {
				tile := &world[idx]
				selected_tile = tile
			}
		}

		if rl.IsKeyPressed(.E) {
			menu = !menu
		}

		rl.BeginDrawing()
		rl.BeginMode2D(camera)
		rl.ClearBackground(rl.DARKGRAY)

		draw_tiles()

		rl.EndMode2D()

		if menu {
			rl.DrawRectangle(0, 0, rl.GetScreenWidth(), 100, rl.LIGHTGRAY)
			i := 0
			outer: for bt, b in buildings {
				for pt, p in b.price {
					if playerResources[pt] < p {
						continue outer
					}
				}

				if rl.GuiButton(
					{f32(100 * i + 200), 10, 100, 50},
					strings.clone_to_cstring(b.name, context.temp_allocator),
				) {
					for pt, p in b.price {
						playerResources[pt] -= p
					}

					selected_tile.building = bt
				}
				i += 1
			}
		}

		i := 0
		for rt, r in playerResources {
			rl.DrawText(fmt.ctprintf("%d", r), i32(100), i32(5 + i * 20), 20, rl.WHITE)
			i += 1
		}

		rl.DrawText(fmt.ctprintf("fps: %d", rl.GetFPS()), 5, 5, 16, rl.RED)
		rl.DrawText(
			fmt.ctprintf("x: %d y: %d", selected_tile.x, selected_tile.y),
			5,
			25,
			16,
			rl.RED,
		)

		rl.EndDrawing()
	}
}

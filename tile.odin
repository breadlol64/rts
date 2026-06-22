package main

import "core:math/noise"
import rl "vendor:raylib"

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

draw_tiles :: proc() {
	min_x, max_x, min_y, max_y := get_visible_tile_bounds(camera)
	for ty in min_y ..= max_y {
		for tx in min_x ..= max_x {
			tile := world[ty * (world_size + 1) + tx]
			texture := terrains[tile.terrain]
			rl.DrawTexture(
				texture,
				i32((tile.x - tile.y) * (tile_width / 2)),
				i32((tile.x + tile.y) * (tile_height / 2)),
				rl.WHITE,
			)
		}
	}
}

get_visible_tile_bounds :: proc(camera: rl.Camera2D) -> (min_x, max_x, min_y, max_y: int) {
	sw := f32(rl.GetScreenWidth())
	sh := f32(rl.GetScreenHeight())

	corners := [4]rl.Vector2 {
		rl.GetScreenToWorld2D({0, 0}, camera),
		rl.GetScreenToWorld2D({sw, 0}, camera),
		rl.GetScreenToWorld2D({0, sh}, camera),
		rl.GetScreenToWorld2D({sw, sh}, camera),
	}

	tile_xs: [4]f32
	tile_ys: [4]f32
	for c, i in corners {
		tile_xs[i] = c.x / tile_width + c.y / tile_height
		tile_ys[i] = c.y / tile_height - c.x / tile_width
	}

	pad :: 2
	min_tx := tile_xs[0]; max_tx := tile_xs[0]
	min_ty := tile_ys[0]; max_ty := tile_ys[0]
	for i in 1 ..< 4 {
		min_tx = min(min_tx, tile_xs[i]); max_tx = max(max_tx, tile_xs[i])
		min_ty = min(min_ty, tile_ys[i]); max_ty = max(max_ty, tile_ys[i])
	}

	min_x = max(0, int(min_tx) - pad)
	max_x = min(250, int(max_tx) + pad)
	min_y = max(0, int(min_ty) - pad)
	max_y = min(250, int(max_ty) + pad)
	return
}

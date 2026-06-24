package main

import "core:fmt"
import "core:math"
import "core:math/noise"
import rl "vendor:raylib"

terrains: map[Terrain]rl.Texture2D

Terrain :: enum {
	Plains,
	Water,
	Sand,
}

Tile :: struct {
	x:        int,
	y:        int,
	terrain:  Terrain,
	building: Building,
	timer:    f32,
}

init_tiles :: proc() {
	terrains = make(map[Terrain]rl.Texture2D)
	terrains[.Plains] = rl.LoadTexture("assets/tiles/plains.png")
	terrains[.Water] = rl.LoadTexture("assets/tiles/water.png")
	terrains[.Sand] = rl.LoadTexture("assets/tiles/sand.png")

	seed := rl.GetRandomValue(0, 2000000000)

	for y in 0 ..< world_size {
		for x in 0 ..< world_size {
			t := Terrain.Plains
			scale := 0.05
			value := noise.noise_2d(i64(seed), {f64(x) * scale, f64(y) * scale})
			if value < -0.4 do t = .Water
			else if value < 0.0 do t = .Sand
			append(&world, Tile{x = x, y = y, terrain = t, building = .None})
		}
	}

	selected_tile = &world[0]
}

draw_tiles :: proc() {
	min_x, max_x, min_y, max_y := get_visible_tile_bounds(camera)
	for ty in min_y ..= max_y {
		for tx in min_x ..= max_x {
			idx := ty * world_size + tx
			if idx < 0 || idx >= len(world) do continue

			tile := &world[idx]
			tile.timer += rl.GetFrameTime()
			texture := terrains[tile.terrain]

			tint := rl.WHITE
			if tile == selected_tile do tint = rl.LIGHTGRAY
			x := i32((tile.x - tile.y) * (tile_width / 2))
			y := i32((tile.x + tile.y) * (tile_height / 2))
			rl.DrawTexture(texture, x, y, tint)
			if tile.building != .None {
				btexture := buildings[tile.building].texture
				rl.DrawTexture(btexture, x, y, tint)

				if tile.timer >= 2 {
					tile.timer = 0
					building := buildings[tile.building]
					for pt, p in building.production {
						playerResources[pt] += p
					}
				}
			}
		}
	}
}

world_to_tile :: proc(pos: rl.Vector2) -> (tx, ty: int) {
	hw := f32(tile_width / 2)
	hh := f32(tile_height / 2)

	fx := pos.x / hw + pos.y / hh
	fy := pos.y / hh - pos.x / hw
	tx = int(math.floor(fx / 2))
	ty = int(math.floor(fy / 2))

	tile_ox := pos.x - f32((tx - ty)) * hw
	tile_oy := pos.y - f32((tx + ty)) * hh
	nx := tile_ox / f32(tile_width)
	ny := tile_oy / f32(tile_height)

	if nx + ny < 0.5 {tx -= 1}
	if nx - ny > 0.5 {tx += 1}
	if ny - nx > 0.5 {ty += 1}
	if nx + ny > 1.5 {ty += 1}
	return
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
		tile_xs[i] = c.x / f32(tile_width) + c.y / f32(tile_height)
		tile_ys[i] = c.y / f32(tile_height) - c.x / f32(tile_width)
	}

	pad :: 2
	min_tx := tile_xs[0]; max_tx := tile_xs[0]
	min_ty := tile_ys[0]; max_ty := tile_ys[0]
	for i in 1 ..< 4 {
		if tile_xs[i] < min_tx do min_tx = tile_xs[i]
		if tile_xs[i] > max_tx do max_tx = tile_xs[i]
		if tile_ys[i] < min_ty do min_ty = tile_ys[i]
		if tile_ys[i] > max_ty do max_ty = tile_ys[i]
	}

	min_x = max(0, int(min_tx) - pad)
	max_x = min(world_size - 1, int(max_tx) + pad)
	min_y = max(0, int(min_ty) - pad)
	max_y = min(world_size - 1, int(max_ty) + pad)
	return
}

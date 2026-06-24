#+feature dynamic-literals
package main
import rl "vendor:raylib"

Building :: enum {
	Sawmill,
	Farm,
	None,
}

BuildingDef :: struct {
	name:       string,
	price:      map[Resource]int,
	production: map[Resource]int,
	texture:    rl.Texture2D,
}

buildings: map[Building]BuildingDef

init_buildings :: proc() {
	buildings = make(map[Building]BuildingDef)
	buildings[.Sawmill] = BuildingDef {
		name = "Sawmill",
		price = {.Wood = 10},
		production = {.Wood = 10},
		texture = rl.LoadTexture("assets/buildings/sawmill.png"),
	}

	buildings[.Farm] = BuildingDef {
		name = "Farm",
		price = {.Wood = 20},
		production = {.Food = 10},
		texture = rl.LoadTexture("assets/buildings/farm.png"),
	}
}

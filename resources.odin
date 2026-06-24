package main

Resource :: enum {
	Wood,
	Food,
}

playerResources: map[Resource]int

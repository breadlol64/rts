#!/usr/bin/env bash

mkdir -p win
cd win
if [ -d "raylib-5.5_win64_mingw-w64.zip" ]; then
    wget https://github.com/raysan5/raylib/releases/download/5.5/raylib-5.5_win64_mingw-w64.zip
    unzip raylib-5.5_win64_mingw-w64.zip
fi

odin build .. -target:windows_amd64 -build-mode:obj -out:game.o
zig 0.16.0 cc -target x86_64-windows-gnu -c ../fltused.c -o fltused.o
zig 0.16.0 cc -target x86_64-windows-gnu game-*.o fltused.o -L./raylib-5.5_win64_mingw-w64/lib -lraylib -lraygui -lopengl32 -lgdi32 -lwinmm -lm -lbcrypt -mwindows -o game.exe
cp raylib-5.5_win64_mingw-w64/lib/raylib.dll .

# 2D Game Engine Sample Project

This is a simple 2D game engine built with C# and [Raylib](https://github.com/raysan5/raylib). The primary goal is to include enough tooling within the engine to enable building games in a fast and efficient manner.

This project is available under the MIT license. 
[Raylib](https://github.com/raysan5/raylib) and the [Raylib_cs](https://github.com/raylib-cs/raylib-cs) language bindings are available under their respective licenses.

## Engine Features

- Entity Component System (ECS)
- 2D sprite rendering with animation support
- Grid-based and Hexagonal grid pathfinding (A* algorithm)
- UI system with buttons and labels
- Scene management
- Sound effects and music playback
- Camera controls
- Tweening system for smooth animations

## Sample Game

The sample game demonstrates the engine's capabilities through the following features:

### Scene Setup
- Interactive UI with a spinning button
- Bouncing "Hello World" label
- Multiple animated sprite characters
- Grid-based movement demonstration
- Hexagonal grid pathfinding
- Campfire animation

### Controls
- Arrow keys (Left/Right) to move the camera
- Click the "Spin Me!" button to trigger animations and scene transitions

### Notable Components

1. **Grid System**
   - Square grid (8x8)
   - Hexagonal grid (8x8)
   - Path-finding characters that navigate both grid types

2. **Characters**
   - Multiple sprite-based characters with frame animations
   - Automated pathfinding movement
   - Random path selection

3. **Visual Effects**
   - Color tweening
   - Rotation animations
   - Position interpolation
   - Particle-like sprite system

## Getting Started

1. Ensure you have .NET 9.0 installed
2. Clone the repository
3. Open the solution in your preferred IDE
4. Run `Program.cs` to start the sample game

## Project Structure

The main components can be found in:
- `SampleGame.cs` - Contains the sample scene implementation
- `Program.cs` - Entry point and game initialization

## Dependencies

- .NET 9.0
- Raylib-cs
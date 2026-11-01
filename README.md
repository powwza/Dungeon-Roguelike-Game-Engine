# Dungeon Roguelike Engine

A turn-based, ASCII dungeon roguelike written in C#, which runs in the console. It's designed as a free, beginner-friendly introduction to the roguelike genre: simple controls, a clean coloured-text display and adjustable difficulty, with the core roguelike features still intact.

## Features

- **Procedurally generated dungeons**: random room placement with validated corridors, so no two runs are the same
- **Permadeath and endless levels**: beat each floor's boss to go deeper, until you die
- **Scaling difficulty**: later levels bring bigger and more numerous rooms, tougher enemies and new enemy types
- **Varied enemies**: random, horizontal, vertical, static and tracking movers; ranged, area-of-effect and damage-trail attackers
- **Pathfinding**: enemies chase the player using breadth-first search, or Dijkstra's algorithm when they need to avoid damage tiles
- **Items and abilities**: swords (broadsword, longsword, mace), heal and strength potions, passive stat items, plus active and passive abilities (heal, AOE attack, damage boost, damage-tile trail) with cooldowns and durations
- **Interactive tiles**: breakable walls, chests opened with keys, damage tiles and an exit tile
- **Four difficulty modes**: Easy, Normal, Hard and Impossible

## Controls

| Key | Action |
|---|---|
| Arrow keys | Move (and navigate menus) |
| W A S D | Attack up / left / down / right |
| E | Use item |
| Q | Use ability |
| Enter | Select menu option |

## How to play

- **Enemies** are red lowercase letters (`a`, `b`, `c`…). Each type moves and attacks in its own way, so learn their patterns.
- **Coins** (`$`) drop from enemies, which also have a chance of dropping a **key**.
- **Chests** (`+`) open when you walk into them with a key, and drop an item.
- **Walls** can be broken by walking into them.
- **The exit** (`:`) appears once you beat the floor's boss. Watch out: enemies standing on it can hide it.

Collect as much `$` as you can before your inevitable demise. Your final score is your coin total.

## Project structure

The game is built around an object-oriented class hierarchy, which makes new content easy to add:

- `Dungeon`, `Room` (`RandomRoom`, `ChestRoom`, `BossRoom`): generation and display
- `Tile` (abstract): `Floor`, `Wall`, `Door`, `Chest`, `DamageTile`, `ExitTile`, …
- `Entity` → `LivingEntity` → `Player`, `Enemy` (abstract) → individual enemy types and `Boss`
- `Item` (abstract): `Weapon`, `ConsumableItem`, `PassiveStatItem`, `Coin`, `Key`
- `Ability` (abstract): `ActiveAbility` and `PassiveAbility`
- `Queue`, `PriorityQueue`: custom data structures used for pathfinding

## Future improvements

- A tougher boss with more varied attacks
- Ranged weapons, more items and choosable abilities
- Better balancing (healing abilities are currently strong; the strength potion is weak)
- An inventory screen and visible enemy stats
- A richer visual display

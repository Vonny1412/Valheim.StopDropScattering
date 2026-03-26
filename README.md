# Catch Dungeon Drops

[GitHub Repository](https://github.com/Vonny1412/Valheim.StopDropScattering)

Fixes dungeon mining drops that would otherwise spawn outside the dungeon instance and fall to the surface.

## The Problem

When mining objects inside dungeons (e.g. scrap piles in crypts), drops can sometimes spawn slightly outside the dungeon geometry.

Because dungeons exist high above the world (`y ~5000`), the items fall down to the surface and end up scattered around the dungeon entrance.

Players must leave the dungeon and search outside to recover the lost drops.

## What This Mod Does

StopDropScattering detects when a dungeon drop starts falling from dungeon height and safely moves it near the nearest player.

This prevents mining drops from leaving the dungeon instance.

## How It Works

The mod:

1. Watches newly spawned `ItemDrop` objects at dungeon height.
2. Detects when an item is falling rapidly.
3. If the item is falling out of the dungeon, it is caught and moved near the nearest player.

The fix only triggers when an item clearly falls out of the dungeon.

Normal drops are unaffected.

## Multiplayer / Server Safety

The item is only repositioned by the **network owner** to avoid multiplayer desync.

## Compatibility

This mod does **not modify dungeon generation, mining logic, or drop tables**.

It only corrects the position of drops that fall outside dungeon instances.

Should be compatible with most mods.

## Installation

Install using:

* Thunderstore Mod Manager
* r2modman
* manual BepInEx installation

## License

MIT

---

*Created with ♥️ — and AI-assisted tools as a supporting tool*

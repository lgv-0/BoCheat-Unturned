# BoCheat-Unturned
Unturned Fully-Featured Cheat Base
## Information
This project is not finished or finalized, and some code is redacted as it is being refined before being pushed into GitHub.
This project heavily relies on [Harmony](https://github.com/pardeike/Harmony/).


## Injection
This DLL needs to be compiled, then loaded to the game, and you need to call the [Main.Yo](https://github.com/lgv-0/BoCheat-Unturned/blob/master/HInj/HInj/Main.cs#L116-L126) function at least once. There's a bad check to detect if it's called multiple times, in which it will ignore further calls.

## Usage
Once loaded, the menu will be visible on the escape/pause menu's. Group Members are default ignored by the cheat, optionally ignore steam friends too.

Chat commands are also available [here](https://github.com/lgv-0/BoCheat-Unturned/blob/master/HInj/HInj/Hooks/sendChat/Hold.cs#L28).

## Screenshots
![Menu](https://i.imgur.com/wf9QsY1.png)
![Skins](https://i.imgur.com/Cy6vO8l.png)
![Nmode](https://i.imgur.com/aSvqbZK.png)
![Chams](https://i.imgur.com/HmgTxcd.png)
___
## Feature List
- Aim
  - LegitBot
    - Visible Check
    - Drop Calculator
    - Smoothing
  - RageBot
    - Fire Through Walls
    - 2 Bypass Methods
    - Targeting/All
  - Etc
    - Force Headshot
    - No Drop
	- Ignore Steam Friends
___
- Visuals
  - Players
    - Distance Check
    - Glow
    - Box
    - Name
    - Item Holding
  - Items
    - Name
    - Rarity Specific
    - Glow
  - Night Mode
    - Light Intensity
  - Always Day
  - Better Water
  - Reduced Flashbang
  - Nightvision
  - Bullet Colors
  - Chams
___
- Vehicles
  - Fly
    - Lift
  - Ping
    - Force
    - Directional
    - 2 Bypass Methods
  - Sink
___
- Items
  - Grabbing
    - All
    - Guns
    - Clips
    - Medical
    - Food
  - Automation
    - Clothes Swapping (Picks up, swaps for better rarity)
    - Arrow (Fire an arrow from crossbow/bow - Will auto pick-up arrow)
    - Medical (Picks up, and crafts clothing into cloth -> rags -> bandages -> dressing)
  - Target Rarity/Minimum Rarity
  - Custom Item Pickup, amount limit
___
- Misc
  - Full On/Off
  - Third Person
  - GPS
  - No recoil sway
  - Remove Exit Timer
  - Fast Salvage (Admin speed)
  - Compass
  - No-Fog
  - Map Revealer
  - No-Spy
  - Clean-Spy
  - Jesus
  - Player Teleport
  - Chat Spammer
  - Achievement Unlocker
  - __Skin Changer__
    - Items will be added to in-game inventory, from there, equip them, and they will automatically apply in-game.

Credits:
lgv-0,
stagfag

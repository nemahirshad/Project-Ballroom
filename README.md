# Ballroom

A three-level arena shooter with voxel characters, keyboard movement, mouse aiming, and a key-to-house progression loop.

## Open and build

Open the project with **Unity 2019.4.9f1**, its existing editor version. The build scene order is Menu, Level 1, Level 2, Level 3, Victory. Use **File > Build Settings > PC, Mac & Linux Standalone > Windows x86_64** to create a Windows player.

The player and Game view use **1920 x 1080 landscape**. The editor menu **Tools > Ballroom > 1920 x 1080 Landscape** restores the fixed Game view. Product name: Ballroom. Existing package versions and navigation meshes are retained.

## Controls

- WASD or arrow keys: move. Diagonal movement has the same maximum speed as cardinal movement.
- Mouse: aim across the projectile plane.
- Hold left mouse: fire at the weapon's configured fire rate.
- Escape: pause/resume during a level.
- Pause menu: resume, retry the current level, return to the main menu, and adjust volume.

## Progression

1. Defeat turrets. Chasers have a blue protection ring and ignore damage while a turret remains.
2. Defeat vulnerable chasers. The HUD explicitly signals when shields are down.
3. Collect the key dropped by the final enemy. The key and house are activated once.
4. Reach the house to advance to the next level. Level 3 advances to Victory.

Original level populations are 5, 6, and 14 enemies. Player health remains 5, 10, and 15 in Levels 1, 2, and 3 respectively. The original scene-specific chaser contact damage is preserved; a chaser collision can defeat a fully healthy player. Other damage receives a short 0.55-second recovery window to prevent stacked contact hits. Defeat freezes the current level and offers Retry Level and Main Menu.

## Implementation

`BallroomGame` bootstraps automatically when any of the five game scenes loads, including opening a level directly in the editor. The saved Menu scene contains the corrected themed interface with persistent Start, Quit, and volume controls, so it is visible in the editor before Play. `BallroomUI` uses the same presentation at runtime, disabling the legacy canvases while retaining their serialized references. It uses a 2048-pixel SDF font atlas generated at 144-point sampling, with a 1920 x 1080 reference canvas.

`Enemies` owns live enemy tracking, chaser protection, and the single key drop. Existing public serialized fields remain compatible with the prefabs. Projectiles use constant velocity, continuous collision detection, ignore their shooter, and consume a hit once. Turret projectile speed is 12 units/second; its lifetime covers its firing distance. Player projectile speed and fire rate retain their prefab values.

`BallroomArena` adds floor markings and retains the original perspective camera rotation and field of view. It makes a modest pullback for full-HD framing without changing the arena colliders or baked navigation meshes. Level cameras use the MainCamera tag. `BallroomFeedback` uses shared materials for protection rings, hit feedback, muzzle flashes, and impacts. `BallroomAudio` maintains one persistent music/cue service. The volume setting is stored in PlayerPrefs under `Ballroom.Volume`. Existing music is retained; synthesized effect cues need no additional audio assets.

Liberation Sans is distributed with its SIL Open Font License in Assets/Fonts. The TextMesh Pro runtime resources match the project's installed 2.0.1 package.

## Verification

Changes are checked in an isolated copy under Unity 2019.4.9f1: all five scenes, all three original enemy populations, movement speed, protection and key progression, projectile consumption, firing cooldown, pause/resume, defeat/retry, house transitions, UI text fit, volume, and persistent audio uniqueness.

Key drops position the imported visible mesh above the floor rather than relying on its offset root pivot. A gold beacon and KEY label identify the pickup. The enlarged trigger supports physical pickup; a short 0.25-second delay and trigger-stay handling prevent an overlapping player from skipping the visible drop. Collected keys remove their complete visual hierarchy. Turrets retain their original fixed firing paths, with a muzzle height that clears the floor. Clicking interface controls does not fire the player weapon.

## Aim reticle and later-level balance

The cyan/pink aim reticle tracks the mouse during gameplay and restores the normal cursor over UI, outside the game view, and while paused or defeated. Its images do not intercept input.

Level 1 retains its existing combat settings. Levels 2 and 3 keep their original enemy counts and player health totals, with these later-level settings:

| Setting | Level 2 | Level 3 |
| --- | --- | --- |
| Player shots per second | 1.5 | 2 |
| Turret shots per second | 0.85 | 0.65 |
| Turret projectile speed | 12 | 11 |
| Chaser contact damage | 2 | 3 |
| Chaser movement speed | 4.5 | 5 |

Later-level turrets have a one-second opening grace period and staggered firing phases. Their existing fixed firing paths remain intact. No pre-shot turret flash, warning ring, or firing-warning cue is added.

## Stable models and projectile clearance

Original voxel meshes and palette materials are saved in Assets/Ballroom Models. Scene and prefab references use these native assets instead of depending on OBJ importer subasset IDs. Gameplay hearts appear above Jack when the reunion trigger activates them and retain their five-second lifetime; the Victory artwork uses the same saved mesh.

Player shots remain horizontal, spawn above the floor with clearance for the projectile radius, and compensate for Rigidbody interpolation. A swept sphere collision check supplements trigger callbacks for fast projectiles. Green enemies update their destination every 0.12 seconds, sample a reachable navigation target, and use smoother pursuit turning and acceleration. Existing level movement speeds, camera angles, and progression remain intact.

Unity 2019.4.9f1 verification: 61 targeted Level 3 physics checks and 128 full progression checks passed, including thirteen red-enemy shot paths, heart visibility, chaser movement, key collection, all level exits, and Victory meshes.

## Safe house spawn and dance floor

When the key is collected inside the future house footprint, the player moves to the nearest clear position with an unobstructed camera view before the solid house collider activates. Normal pickups outside the footprint retain the player's position. The original door and scene progression remain intact.

The arena uses a saved beveled stage mesh with purple checkerboard dance tiles, pink/cyan edge trim, and a dark backdrop. Floor collision stays on the original flat BoxCollider; tile decorations have no colliders. Lighting uses a neutral key light, gentle cyan fill, brighter ambient light, soft shadows, and 4x antialiasing. The default High quality setting enables shadows and antialiasing in the editor too.

Unity 2019.4.9f1: 149 checks passed, including key drops at the house centre, near the door side, and near a corner across all three levels; no player/house penetration or camera occlusion after spawning; physical projectiles, key pickup, house exits, heart display, and Victory progression.

## Main menu, pause, and victory presentation

The saved Menu and Victory scenes include a checkerboard stage, decorative speakers, Brie and Jack, a floating heart, a slowly rotating faceted disco ball, and gentle pink/cyan lighting. The Victory arrangement places the characters in front of the house. The menu camera is independent of the original gameplay cameras.

Main menu controls: Play, Settings, How to Play, and Quit. Settings include persistent volume and an optional Show Run Results toggle. Help explains controls and the turret/chaser shield, key, and house objectives. Buttons animate their glow and scale on pointer hover or keyboard selection. Native scene callbacks remain wired for editor previews; runtime UI builds matching panels at 1920x1080.

Pause provides Resume, Restart Level, Main Menu, and volume. Restarting or leaving an active level requires confirmation; cancel and Escape return to pause. Defeat and Victory return to the main menu directly.

Victory displays a brief heart animation and one confetti burst. Completion time includes running gameplay only; damage taken counts actual health lost. Successful level exits commit their statistics, retry replaces the current level's results, and Play Again starts a fresh run. Results can be hidden in Settings.

Verification in Unity 2019.4.9f1: 45 menu control/presentation checks and 154 full gameplay checks passed, including native button callbacks, menu navigation, settings persistence, pause timing, confirmation/cancel/restart, victory celebrations, real run statistics, replay, projectile collisions, safe house spawning, and physical progression through all three levels. 1920x1080 captures were checked for text fit and visual layout.

## House reveal and arena decoration

Collecting the key reveals the house with a 0.85-second rise/scale animation and a fading gold-to-pink welcome halo. Visual mesh copies animate independently while the original house collider remains at its safe final position. The original renderers return at the end of the reveal. Door entry waits for completion; trigger-stay handling lets a player already waiting at the entrance proceed automatically. The reveal pauses with gameplay.

Each arena has four decorative speakers with cones and neon pedestals, five rear light columns, and a subtle back rail. All decoration stays outside the playable floor, has no active colliders, and leaves the original camera angle and field of view intact.

Unity 2019.4.9f1: 172 checks passed, covering nonblocking decoration, reveal timing, safe house spawning, early-entry protection, physical door transitions, projectile collisions, key collection, hearts, real run statistics, and all three levels. 1920x1080 previews were inspected.

Animated Pixel Pack
(Check script files for inline comments on how everything works)

This package consists of the following key parts:

/Characters - The skin spritesheets for each character
/Characters/Prefabs - Pre-created prefabs that contain a tree of gameobjects representing each character
/Characters/Animations - Pre-created mecanim animations and the controller that all the characters use

/Editor - The custom import script that creates exact geometry for each sprite imported under the AnimatedPixelPack folder (see comments in .cs file for details)

/Environment - The materials, prefabs used by the demo scene
/Environment/Scripts - The C# code used by the demo scene and the characters so that they can be controlled and animated by user input

/Items - Additional spritesheets that contain sprites that can be added to a game, or swapped for existing pieces on any character

/Projectiles - The sprites and prefabs for example projectiles that can be fired by a character
/Characters/Animations - Pre-created mecanim animations and the controllers that the projectiles use

Information about the character prefabs:
- You MUST set the GroundLayer property for the character prefabs (in the editor) to the layer that contains
  all your ground colliders.
  This will be 'Default' when you first import the AnimatedPixelPack, but you can update it if it is not correct
  Don't set it to be the same layer as the Character prefabs themselves, or they will act strangely
  
- For best results you may need to add some layers to your project (Unity won't import/export them for us)
  They should work out of the box, but having the correct layers set will let you customize the collisions
  in your own project.

  By default the prefabs use:
  Layer 8 - Characters
  Layer 9 - CharacterWeapons
  Layer 10 - Projectiles
  Also a sorting layer 'Characters'

- All parts of the Character prefabs are set to the 'Character' layer, except the Main and Off Item,
  those use the CharacterWeapons layer to filter out collisions

- All the projectiles use the 'Projectiles' layer so you can disable collisions

- The characters consist of 2 script components
  Character.cs
  This is the main script for controlling the characters. 
  It provides all the options for turning on/off things like double jump, air control, wall sliding, etc
  You can turn things off if your game already has code for them.
  It also controls all the animation states based on the settings and the input

  CharacterInput.cs
  This is the script that controls the input for the player.
  The one included in the pack has the following buttons set:
  Left/Right - movement
  Up/Down - ladder movement
  Down - Run modifier
  Space - Jump
  Left Mouse - Quick attack
  Right mouse - Attack
  Middle mouse - Cast/Fire bow
  Z - Throw offhand item
  X - Throw mainhand item
  C - Consume offhand item
  B - Block (if blocking is enabled on the character properties)

  This is designed so you can create a new script that calls the Move() and Perform() methods on the main
  character, based on your own input. For example you might make on screen controls for mobile, or
  2 sets of keys for multiplayer, etc.

Additional demo scene controls:
  + - Next character
  - - Previous character


Customizing the characters
- Duplicate an existing character prefab and rename it
- Swap out any of the sprites on the sprite renderers in the prefab
- You may want to create a new sprite sorting layer and set each sprite renderer to that new layer
  (Doing so reduces drawcalls when not using the sprite atlas, and you can organize which characters appear in front of others)
- You can update the sprite 'order in layer' property to make feet appear infront or behind the body (robes vs shorts for example)
- When creating your own sprite sheet, make sure to import using the same settings as the existing ones
  - 16 pixels per unit
  - Multi sprite
  - Grid -> 16x16 -> Center
  - Packing Tag -> AnimatedPixelPack
  - Filter mode -> point
  - No mipmaps
- Create new or modify existing mecanim animations. (Note that the rotation point for each sprite is the center to make it easier to swap them out)


Customizing Weapons/Spells/Effects
- The Weapon section of the character script shows the options available
- The Equipped Weapon Type select is used in the state machines to select which type of animation to play when you attack/cast (bow vs staff for example)
- IsBlockEnabled can be toggled to true if they are holding a shield to allow the block animation to play
- The LaunchPoint is an empty gameobject that specifies where projectiles should be launched from.
  It can be moved from inside an animation
  The animations use trigger points (see Humaonid_OverheadCast for an example) to specify when the projectile should fire
- CastProjectile should be a WeaponProjectile prefab that is cloned when the cast animation is triggered
- ThrowMainProjectile should be a WeaponProjectile prefab that will be cloned when the throw main item animation is triggered
- ThrowOffProjectile should be a WeaponProjectile prefab that will be cloned when the throw off item animation is triggered
- EffectPoint is an empty gameobject (similar to launch point) that specifies where an effect should be targetted during the cast animation
  See the Orc_Shaman character for an example lightning effect
- Effect is the prefab of the effect to cast (see Orc_Shaman again)


Hopefully the code is self explainatory and you will be able to figure out how to use this in your own game!

Thanks!

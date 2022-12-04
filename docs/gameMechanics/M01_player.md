# Player Mechanics

- The player can move
- The player can jump
- The player can crouch

The character will not collide with entities.

## Moving

The player can move using the movement stick.

- Moving the stick partially to the side, will cause the character to slowly walk in that direction.
- Moving the stick fully to the side, will cause the character to run towards that direction.
- Flicking the movement stick to the side will cause the character to sprint towards that direction.

## Crouching

/* Crouching does not yet have a usefulness in the design

Holding the movement stick down will make the character crouch.
Moving the stick to the side will make the character crouch walk towards that direction.
When the character is crouched his hit box is smaller and allows the character to move through smaller spaces.
Moving the stick back to neutral position or up will make the character stand up.
If the character is on a thin platform, and he performs a crouch, he will fall through the platform.

## Sliding

If the character is running, and he presses the movement stick down, he will initiate a slide.

## Player states
playerIsIdle
playerIsGrounded
playerIsAfloat
playerIsWalking
playerIsRunning
playerIsSprinting
playerIsCrouching
playerIsCrouchWalking
playerIsSliding
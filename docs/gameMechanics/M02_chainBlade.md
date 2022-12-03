# Chain Blade Mechanics

- The player can cast the blade, casting the blade throws the blade into the targeting direction
- The player can pull himself towards the anchor point
- The player can pull the object towards himself

## Casting the blade

The player can cast the blade when he presses the 'Cast-key'. Casting the blade will throw it away from the player towards the casting direction determined by the 'targeting-axis'. 

## Anchoring

When the blade collides with an object it will try to anchor itself onto the object at the collision point.

The blade can only anchor to objects that are an anchor-point.

When anchored the player is constrained to the object by the length of the chain.
+ Who pulls who in this case, is it based on strength or mass, physics or stages?

## Pulling

When the blade is anchored the player can pull the anchored object towards him.

If an anchor point is breakable and the player pulls the blade the anchor point will break.

## Lunging

When the blade is anchored the player can pull himself towards the anchor point.
# Grapple Mechanics

When the player holds down the cast-key, he will throw a hand towards a targeting direction.

When the hand is thrown, it will move towards the throwing direction until:
- 

## Casting the blade

When the player holds down the cast-key, he will throw a blade towards the targeting direction.
When a blade is thrown, it will fly in a straight line towards the target direction for a specified range.
If the blade collides with an object that is an anchor point it will anchor itself to the object.
If the blade does not collide with an object it will return to the player.

## Anchoring

When the blade is anchored to an object, it will constrain the player to the object.
The player and the object can not be further apart than a specified range (length of the chain).

+ Who pulls who in this case, is it based on strength or mass, physics or stages?

## Pulling

When the blade is anchored, and the player flicks the stick to the opposite direction of the anchor, he can pull the anchored object towards him.

If an anchor point is breakable and the player pulls the blade the anchor point will break.

## Lunging

When the blade is anchored, and the player releases the cast key, he can pull himself towards the anchor point. The player will move towards the anchor point in a straight line until it reaches the point.

- The player can throw the blade into a direction
- The blade will fly towards a direction until:
    - The blade hits a collider
    - The blade exceeds the range towards the player
extends KinematicBody2D

var velocity = Vector2()
func _process(delta):
	pass

func _physics_process(delta):
	process_collisions()

func process_collisions():
	for i in get_slide_count():
		var collision = get_slide_collision(i)
		print("collided with: ", collision.collider.name)

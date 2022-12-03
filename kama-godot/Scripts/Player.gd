extends KinematicBody2D

var speed = 5 * 24
var jumpHeight = 1 * 24
var gravity = 9.8 * 24
var velocity = Vector2()

var moveInputAxis = 0
var jumpInputPressed = false

func _process(delta):
	get_input()
	
	$AnimatedSprite.flip_h = velocity.x < 0

func _physics_process(delta):
	# add movement;
	velocity.x = moveInputAxis * speed
	
	#Jump
	if(jumpInputPressed && is_on_floor()):
		velocity.y += -sqrt(2 * gravity * jumpHeight)
	
	# add gravity
	velocity.y += gravity * delta
	
	# apply forces to object
	velocity = move_and_slide(velocity, Vector2.UP)

func get_input():
	moveInputAxis = Input.get_axis("move_left", "move_right")
	jumpInputPressed = Input.is_action_just_pressed("jump")

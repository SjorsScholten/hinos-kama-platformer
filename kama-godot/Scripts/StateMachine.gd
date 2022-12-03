class_name StateMachine
extends Node

export var initial_state := NodePath()
onready var state: State = get_node(initial_state)

func _ready():
	yield(owner, "ready")
	for child in get_children():
		child.state_machine = self
	state.enter()

func _process(delta):
	state.update(delta)

func transition_to(target_state_name: String) -> void:
	if not has_node(target_state_name):
		return
		
	state.exit()
	state = get_node(target_state_name)
	state.enter()

using Godot;
using System;
public partial class StagTest : CharacterBody2D
{
	[Signal]
	public delegate void StagHungryEventHandler(int hunger,StagTest stag);
	private NavigationAgent2D _navAgent;
	private int hunger = 5;
	private Vector2 position;
	[Export] public float Speed = 100f;
	private AnimatedSprite2D _animatedSprite;
	public Vector2 Position
	{
		get { return position; }
		set { position = value; }
	}
	public int Hunger
	{
		get { return hunger; }
		/*set
		{
			hunger = value;
			if (hunger <= 0)
			{
				GD.Print("Stag is dead");
				QueueFree();
			}
		}*/
	}
	public override void _Ready()
	{
		_navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	//GD.Print("Agent map: ", _navAgent.GetNavigationMap());
		_navAgent.PathDesiredDistance = 4f;
		_navAgent.TargetDesiredDistance = 2f;
		_navAgent.AvoidanceEnabled = true;

		_animatedSprite.Play("default");
	}

	public void MoveTo(Vector2 targetPosition)
	{
		// GD.Print("MoveTo() called with: " + targetPosition);
	_navAgent.TargetPosition = targetPosition;
	//GD.Print("Navigation finished? " + _navAgent.IsNavigationFinished());
	}
	public override void _Process(double delta)
	{
		// Runs every frame (render rate)
		//GD.Print("Process: " + delta);
		if (hunger <= 5)
		{
			EmitSignal(nameof(StagHungry), hunger,this);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_navAgent.IsNavigationFinished())
		{
			Vector2 nextPoint = _navAgent.GetNextPathPosition();
			Vector2 direction = (nextPoint - GlobalPosition).Normalized();

			Velocity = direction * Speed;
			MoveAndSlide();
		}
		else
		{
			Velocity = Vector2.Zero;
		}
		
		if (Input.IsActionPressed("move_down"))
			MoveTo(new Vector2(200,200));
		
	}
}

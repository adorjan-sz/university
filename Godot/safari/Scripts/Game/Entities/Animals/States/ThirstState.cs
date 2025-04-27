using Godot;
using System;

public partial class ThirstState : BaseState
{

	int count;
	public override void Enter(BaseState previousState)
	{
		GD.Print("Entered Thirst State");
		_animatedSprite.Play("Idle_top_right");
		//get Stag node which is the grandparent of this state
		count = 0;
		_navAgent.TargetPosition = animal.GlobalPosition;
		animal.EmitSignal(nameof(Animal.AnimalSee), animal);
		animal.EmitSignal(nameof(Animal.AnimalThirsty), animal);
	}
    public override void LoadThirst(int _count)
    {
        _animatedSprite.Play("Idle_top_right");
        count = _count;
		if(animal.CurrentWaterToGo != null)
		{
            //create random offset for target
            Vector2 targetPosition = animal.CurrentWaterToGo.WorldCoords;
            targetPosition += new Vector2((float)GD.RandRange(-10, 10), (float)GD.RandRange(-10, 10));
			_navAgent.TargetPosition = targetPosition;

        }
    }	
    public override void Update(double delta)
	{
		Hunger += 0.001 * Age;
		Thirst += 0.005 * Age;
		Age += 0.001;
		
		count++;
		if (count == 50)
		{
			count = 0;
			animal.EmitSignal(nameof(Animal.AnimalSee), animal);

		}
		if (Hunger > 90 || Thirst > 90 || Age > 40)
		{
			StateMachine.ChangeState("DeathState");
		}
		else if(animal.CurrentWaterToGo is null)
		{
			StateMachine.ChangeState("ThirstState");
		}
		else if (_navAgent.IsTargetReached())
		{
			Thirst = 0;
			animal.CurrentWaterToGo = null;


			StateMachine.ChangeState("IdleState");
		}

	}
	public override void Exit(BaseState nextState)
	{
		GD.Print("Exiting Thirst State");
	}
	
}

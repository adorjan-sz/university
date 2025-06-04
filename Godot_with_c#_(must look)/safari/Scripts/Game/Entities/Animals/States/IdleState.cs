// IdleState.cs
using Godot;
using System;

public partial class IdleState : BaseState
{
	public override void Enter(BaseState previousState)
	{
		animal.Velocity = new Vector2(0, 0);

		GD.Print("Entered Idle State");

		animal.EmitSignal(nameof(Animal.AnimalSee), animal);

		Random random = new Random();
		
		int randomDirection = random.Next(0, 4);
		//randomly choose a direction to face
		
		switch (randomDirection)
		{
			case 0:
				_animatedSprite.Play("Idle_top_right");
				break;
			case 1:
				_animatedSprite.Play("Idle_top_left");
				break;
			case 2:
				_animatedSprite.Play("Idle_bottom_right");
				break;
			case 3:
				_animatedSprite.Play("Idle_bottom_left");
				break;
		}
	}
	public override void LoadIdle()
	{
        animal.Velocity = new Vector2(0, 0);

        GD.Print("Loaded Idle State");

        

        Random random = new Random();

        int randomDirection = random.Next(0, 4);
        //randomly choose a direction to face

        switch (randomDirection)
        {
            case 0:
                _animatedSprite.Play("Idle_top_right");
                break;
            case 1:
                _animatedSprite.Play("Idle_top_left");
                break;
            case 2:
                _animatedSprite.Play("Idle_bottom_right");
                break;
            case 3:
                _animatedSprite.Play("Idle_bottom_left");
                break;
        }
    }

	public override void Update(double delta)
	{




		Hunger += 0.1 * delta;

        Thirst += 0.1 * delta;
		Age += delta;
		

		if (Hunger>90 || Thirst >90 || Age > 1000)
		{
			GD.PrintErr("hunger." + Hunger);
            GD.PrintErr("thirst." + Thirst);
            GD.PrintErr("age." + Age);
            StateMachine.ChangeState("DeathState");
		}
		

		else if (Thirst > 50)
		{
			if (Hunger > 50)
			{
				if(Thirst >= Hunger)
				{
					StateMachine.ChangeState("ThirstState");
				}
				else if (Hunger > Thirst)
				{
					StateMachine.ChangeState("HungerState");
				}

			}
			else
			{
				StateMachine.ChangeState("ThirstState");
			}
		   
		}
		else if (Hunger > 50)
		{
			
			StateMachine.ChangeState("HungerState");
		}
		
	}

	public override void Exit(BaseState nextState)
	{
		GD.Print("Exiting Idle State");
	}
	
}

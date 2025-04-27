using Godot;
using System;

public partial class GroupUpState : BaseState
{
	int count;
	public override void Enter(BaseState previousState)
	{
		GD.Print("Entered GroupUpSate State");
		//get Stag node which is the grandparent of this state

		count = 0;

	}
	public override void LoadGroupUp(int _count)
    {
        count = _count;
       
            
            
       _navAgent.TargetPosition = (Vector2)animal.GroupUpCoord;

    
        
    }
    public override void Update(double delta)
	{
		
		



        Hunger += 0.005 * Age;
		Thirst += 0.005 * Age;
		Age += 0.001;
		
		//save whats around the stag
		count++;
		if (count == 50)
		{
			count = 0;
			animal.EmitSignal(nameof(Animal.AnimalSee), animal);

		}


		//check if the stag is hungry or thirsty
		if (Hunger > 90 || Thirst > 90 || Age > 40)
		{
			StateMachine.ChangeState("DeathState");
		}
		else if (Thirst > 50)
		{
			if (Hunger > 50)
			{
				if (Thirst > Hunger)
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
		if (_navAgent.IsTargetReached())
		{
			animal.GroupUpCoord = null;
            StateMachine.ChangeState("IdleState");
		}

	}

	public override void Exit(BaseState nextState)
	{
		GD.Print("Exiting GroupUpState State");
	}

}

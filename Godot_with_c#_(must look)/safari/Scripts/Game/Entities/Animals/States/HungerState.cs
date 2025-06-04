using Godot;
using System;

public partial class HungerState : BaseState
{
	int count;
	public bool IsHuntGoingOn;
	public bool IsBiteAnimStarted;
	public override void Enter(BaseState previousState)
	{
		GD.Print("Entered hunger State");
		_animatedSprite.Play("Idle_top_right");	
		count = 0;
		_navAgent.TargetPosition = animal.GlobalPosition;
		animal.EmitSignal(nameof(Animal.AnimalSee), animal);
		animal.EmitSignal(nameof(Animal.AnimalHungry), animal);
		IsHuntGoingOn = true;

        //only for carnivores but for easier load it is here
        IsBiteAnimStarted = false;  
		
	}
    public override void LoadHunger(double _hunger, double _thirst, double _age, int _count, bool _isHuntGoingOn)
    {
		GD.Print("Loading hunger State");
        count = _count;
        IsHuntGoingOn = _isHuntGoingOn;
        Hunger = _hunger;
        Thirst = _thirst;
        Age = _age;
		if (!IsHuntGoingOn && animal is Carnivore)
        {   /*during saving the bite animation started but 
             didnt finish so during loading we need to finish it so that the animal
			can go to idle state
             */
            GD.PrintErr("IN BITING ANIMATION");
            Random random = new Random();

            int randomDirection = random.Next(0, 4);

            switch (randomDirection)
            {
                case 0:

                    _animatedSprite.Stop();
                    _animatedSprite.AnimationFinished += OnAnimationFinished;

                    _animatedSprite.Play("Bite_top_right");
                    break;
                case 1:
                    _animatedSprite.Stop();
                    _animatedSprite.AnimationFinished += OnAnimationFinished;
                    _animatedSprite.Play("Bite_top_left");
                    break;
                case 2:
                    _animatedSprite.Stop();
                    _animatedSprite.AnimationFinished += OnAnimationFinished;
                    _animatedSprite.Play("Bite_bottom_right");
                    break;
                case 3:
                    _animatedSprite.Stop();
                    _animatedSprite.AnimationFinished += OnAnimationFinished;
                    _animatedSprite.Play("Bite_bottom_left");
                    break;
            }
            GD.PrintErr("Bite Animation");
		}
		else
		{
			if (animal is Carnivore) {
               
				GD.PrintErr("\nCarnivore Target Position: " + ((Carnivore)animal).CurrentThingToEat.GlobalPosition.ToString());
            }
			else
            {
                _navAgent.TargetPosition = ((Herbivore)animal).CurrentVegetationToEat.WorldCoords;
            }
        }
		
    }
    public override void Update(double delta)
	{
		count++;
		
			animal.EmitSignal(nameof(Animal.AnimalSee), animal);

		

		Hunger += 0.1 * delta;
		Thirst += 0.1 * delta;
		Age += delta;
		if ((Age > 40 || Hunger > 90 || Thirst > 1000) && (IsHuntGoingOn))
		{
			animal.StateMachine.ChangeState("DeathState");
		}
		else if (animal is Carnivore)
		{
			//-------------------Carnivore-------------------
			
			if (IsHuntGoingOn)
			{

				if (((Carnivore)animal).CurrentThingToEat is null)
				{
					StateMachine.ChangeState("HungerState");
					return;
				}


				if (_navAgent.IsTargetReached())
				{


					
					Hunger = 0;
					
					Random random = new Random();

					int randomDirection = random.Next(0, 4);
					
					switch (randomDirection)
					{
						case 0:

							_animatedSprite.Stop();
							_animatedSprite.AnimationFinished += OnAnimationFinished;

							_animatedSprite.Play("Bite_top_right");
							break;
						case 1:
							_animatedSprite.Stop();
							_animatedSprite.AnimationFinished += OnAnimationFinished;
							_animatedSprite.Play("Bite_top_left");
							break;
						case 2:
							_animatedSprite.Stop();
							_animatedSprite.AnimationFinished += OnAnimationFinished;
							_animatedSprite.Play("Bite_bottom_right");
							break;
						case 3:
							_animatedSprite.Stop();
							_animatedSprite.AnimationFinished += OnAnimationFinished;
							_animatedSprite.Play("Bite_bottom_left");
							break;
					}
					GD.PrintErr("Bite Animation");
					
					if(((Carnivore)animal).CurrentThingToEat.StateMachine.CurrentState is not DeathState)
					{
						((Carnivore)animal).CurrentThingToEat.StateMachine.ChangeState("DeathState");
					}
					IsHuntGoingOn = false;



				}
				else if (count % 10 == 0 && !_navAgent.IsTargetReached())
				{
					_navAgent.TargetPosition = ((Carnivore)animal).CurrentThingToEat.GlobalPosition;
				}
			}

		}
		else if (animal is Herbivore)
		{
			//-------------------Herbivore-------------------

			if (((Herbivore)animal).CurrentVegetationToEat is null)
			{
				StateMachine.ChangeState("HungerState");
				return;
			}
			if (_navAgent.IsTargetReached())
			{
				Hunger = 0;

				((Herbivore)animal).CurrentVegetationToEat = null;


				StateMachine.ChangeState("IdleState");
			}
		}

	   
	}
	private void OnAnimationFinished()
	{
		_animatedSprite.AnimationFinished -= OnAnimationFinished;

		GD.PrintErr("Animation Finished");
		StateMachine.ChangeState("IdleState");
	}
	public override void Exit(BaseState nextState)
	{
		GD.Print("Exiting Hunger State");
	}

}

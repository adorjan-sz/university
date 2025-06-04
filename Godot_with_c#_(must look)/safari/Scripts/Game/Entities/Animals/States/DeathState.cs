// IdleState.cs
using Godot;
using System;

public partial class DeathState : BaseState
{
	private double count = 0;
	public int GetCount()
    {
        return (int)count;
    }
    public override void Enter(BaseState previousState)
	{
		animal.Velocity = Vector2.Zero;
		GD.Print("Entered Death State");
		_animatedSprite.Play("Death");
		animal.EmitSignal(nameof(Animal.AnimalDied), animal);
		_navAgent.TargetPosition = animal.GlobalPosition;
		
		animal.CollisonShapeEnabled = false;

	}
	public override void LoadDeath(int _count)
	{	count = _count;
        GD.Print("Loading Death State");
        animal.Velocity = Vector2.Zero;
		if(count< 500)
		{
			if(animal is Carnivore)
			{
                _animatedSprite.Play("LoadDeath");
			}
			else
			{
                _animatedSprite.Play("Death");
            }
				
		}
		else
		{
            _animatedSprite.Play("Skeleton");
        }
        _navAgent.TargetPosition = animal.GlobalPosition;

        animal.CollisonShapeEnabled = false;

    }
	public override void Update(double delta)
	{

		count += delta;
		if (Math.Floor(count) == 500 )
		{
			_animatedSprite.Play("Skeleton");
			
		}
		if (count > 1000)
		{
			GD.Print("Exiting Death State");
			
			animal.DestroySelf();
		}
	}

	public override void Exit(BaseState nextState)
	{
		
		GD.Print("Exiting Death State");
	}

}

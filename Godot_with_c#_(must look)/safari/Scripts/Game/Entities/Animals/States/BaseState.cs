using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;



public abstract partial class BaseState : Node
{
	public NavigationAgent2D _navAgent;
	public AnimatedSprite2D _animatedSprite;
	public double Hunger;
	public double Thirst;
	public double Age;
	public Animal animal;
	public StateMachine StateMachine { get; set; }
	public override void _Ready()
	{
		
	} 
	public virtual void Enter(BaseState previousState) { }
	public virtual void Exit(BaseState nextState) { }
	public virtual void Update(double delta) {
	   
	}
    public virtual void LoadHunger(double _hunger,double _thirst , double _age, int _count, bool _isHuntGoingOn)
	{

	}
    public virtual void LoadDeath(int _count)
    {

    }
	public virtual void LoadIdle()
	{

	}
	public virtual void LoadThirst(int _count)
    {

    }
    public virtual void LoadGroupUp(int _count)
	{

	}



}

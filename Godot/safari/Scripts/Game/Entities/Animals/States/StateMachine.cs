using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;


	public partial class StateMachine : Node
	{
		private Dictionary<string, BaseState> _states = new Dictionary<string, BaseState>();
		private BaseState _currentState;
		protected static int _stateCounter = 0;
		public BaseState CurrentState
		{
        get { return _currentState; }
        set { _currentState = value; }
		}
		public BaseState GetStateByName(string stateName)
    {
        if (_states.TryGetValue(stateName, out var state))
        {
            return state;
        }
        else
        {
            GD.PrintErr($"State '{stateName}' not found.");
            return null;
        }
    }

    public override void _Ready()
		{
        var agent = GetParent().GetNode<NavigationAgent2D>("NavigationAgent2D");
        var animation = GetParent().GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        var ParentAnimal = GetParent<Animal>();

        foreach (Node child in GetChildren())
        {
            if (child is BaseState state)
            {
                state.StateMachine = this;
                state._navAgent = agent;
                state._animatedSprite = animation;
                state.Hunger = 0;
                state.Thirst = 0;
                state.Age = 1;
                state.animal = ParentAnimal;


                _states[state.Name] = state;
            }
        }
        agent.TargetPosition = ParentAnimal.GlobalPosition;
    }
		public void Start()
		{

			
			ChangeState("IdleState");
		}


		public override void _Process(double delta)
		{
			_currentState?.Update(delta);
		}

		public void ChangeState(string newStateName)
		{
			if (_states.TryGetValue(newStateName, out var newState))
			{
				if (_currentState != null)
				{
					newState.Hunger = _currentState.Hunger;
					newState.Thirst = _currentState.Thirst;
					newState.Age = _currentState.Age;
					//GD.Print($"State changed from {_currentState.Name} to {newState.Name}");
				}

				_currentState?.Exit(newState);
				var previousState = _currentState;
				_currentState = newState;
				_currentState.Enter(previousState);
			}
			else
			{
				//GD.PrintErr($"State '{newStateName}' not found.");
			}
		}
		
}

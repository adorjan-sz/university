

using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Godot;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using Safari.Scripts.Game.Tiles;


public abstract partial class Animal : CharacterBody2D,Buyable
{

    // public PlaceholderStatemachine StateMachine { get; set; }
    [Signal]
    public delegate void AnimalThirstyEventHandler(Animal animal);
    [Signal]
    public delegate void AnimalWillBeDeletedEventHandler(Animal animal);
    [Signal]
    public delegate void AnimalDiedEventHandler(Animal animal);
    [Signal]
    public delegate void AnimalSeeEventHandler(Animal animal);
    [Signal]
    public delegate void AnimalHungryEventHandler(Animal animal);
    public abstract string AnimalsName { get;  }
        public int Price { get; set; }
    public List<UsableTile> Water;
    public UsableTile? CurrentWaterToGo = null;
    public NavigationAgent2D _navAgent;
    public Vector2? GroupUpCoord = null;
    public AnimatedSprite2D _animatedSprite;
    private StateMachine _stateMachine;
    private long _id;
    public long Id
    {
        get => _id;
        set
        {
            _id = value;
            Name = AnimalsName + _id.ToString();
        }
    }
    public CollisionShape2D _myShape;
   
    public abstract float Speed { get; }
    public bool CollisonShapeEnabled
    {
        set
        {
            _myShape.Disabled = value;
        }
    }
    public StateMachine StateMachine => _stateMachine;
    public override void _Ready()
    {
        _navAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _myShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _stateMachine = GetNode<StateMachine>("StateMachine");
        _navAgent.VelocityComputed += OnVelocityComputed;
        Water = new List<UsableTile>();
        
        

    }
    public void NewAnimal()
    {
        _stateMachine.Start();
    }

    public void GoDrink(Vector2? targetPosition, UsableTile water)
    {
        if(water is not null)
        {
            _navAgent.TargetPosition = (Vector2)targetPosition;
        }
        
        CurrentWaterToGo = water;
    }

    public void MoveTo(Vector2 targetPosition)
    {
        GroupUpCoord = targetPosition;
        _navAgent.TargetPosition = targetPosition;
        
    }
    public void DestroySelf()
    {

        EmitSignal(nameof(AnimalWillBeDeleted), this);
        QueueFree();
    }
    public void PlayWalkAnimation(Vector2 direction)
    {
        if (direction == Vector2.Zero)
        {

            return;
        }


        float angle = direction.Angle();

        // Normalize to [0, 2?) range
        if (angle < 0)
            angle += 2 * Mathf.Pi;

        // Now determine the closest diagonal direction
        if (angle >= 7 * Mathf.Pi / 4 || angle < Mathf.Pi / 4)
        {
            // Right (but we skip it!)
        }
        else if (angle >= Mathf.Pi / 4 && angle < 3 * Mathf.Pi / 4)
        {
            if (direction.X < 0)
                _animatedSprite.Play("Walk_bottom_right");
            else
                _animatedSprite.Play("Walk_top_right");
        }
        else if (angle >= 3 * Mathf.Pi / 4 && angle < 5 * Mathf.Pi / 4)
        {
            if (direction.X < 0)
                _animatedSprite.Play("Walk_bottom_left");
            else
                _animatedSprite.Play("Walk_top_left");
        }
        else if (angle >= 5 * Mathf.Pi / 4 && angle < 7 * Mathf.Pi / 4)
        {
            if (direction.X < 0)
                _animatedSprite.Play("Walk_bottom_left");
            else
                _animatedSprite.Play("Walk_bottom_right");
        }
        else
        {
            _animatedSprite.Play("Walk_bottom_right");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_navAgent.IsNavigationFinished())
        {
            Vector2 nextPathPosition = _navAgent.GetNextPathPosition();
            Vector2 newVelocity = GlobalPosition.DirectionTo(nextPathPosition) * Speed;
            _navAgent.Velocity = newVelocity;
        }
        else
        {
            Velocity = Vector2.Zero;
        }
    }
    public void OnVelocityComputed(Vector2 safeVelocity)
    {

        if (_stateMachine.CurrentState is HungerState && !((HungerState)_stateMachine.CurrentState).IsHuntGoingOn)
        {
            ;
        }
        else if (!(_stateMachine.CurrentState is IdleState ||
            _stateMachine.CurrentState is DeathState))
        {
            PlayWalkAnimation(safeVelocity);
        }
        Velocity = safeVelocity;
        MoveAndSlide();
    }

}


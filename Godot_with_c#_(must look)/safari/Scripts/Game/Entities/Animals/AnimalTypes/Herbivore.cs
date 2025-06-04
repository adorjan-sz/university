using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using Godot;

public abstract partial  class Herbivore:Animal
    {
        
    public List<UsableTile> Food = new List<UsableTile>();
    public UsableTile? CurrentVegetationToEat;
    public void GoGrazing(Vector2? targetPosition, UsableTile food)
    {
        if(food is not null)
        {
            _navAgent.TargetPosition = (Vector2)targetPosition;
        }
        CurrentVegetationToEat = food;
    }
    public override void _Ready()
    {
        base._Ready();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using Godot;

public abstract partial class Carnivore: Animal
{
    public List<Animal> Food = new List<Animal>();
    public Animal? CurrentThingToEat = null;
    public void GoHunt(Vector2 targetPosition, Animal animal)
    {
        if (animal is not null)
        {
            _navAgent.TargetPosition = targetPosition;
        }

        CurrentThingToEat = animal;

    }

}

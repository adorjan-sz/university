using Godot;
using Safari.Scripts.Game.Entities.Animals;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using System;
using System.Collections.Generic;

public partial class Boar :Herbivore
{
	public override float Speed { get { return 90; } }
	public override string AnimalsName { get { return "Boar"; } }

}

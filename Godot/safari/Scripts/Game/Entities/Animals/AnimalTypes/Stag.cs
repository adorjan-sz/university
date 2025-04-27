using Godot;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using System;
using System.Collections.Generic;
public partial class Stag: Herbivore
{
	public override float Speed { get { return 110; } }
	public override string AnimalsName { get { return "Stag"; } }
	
}

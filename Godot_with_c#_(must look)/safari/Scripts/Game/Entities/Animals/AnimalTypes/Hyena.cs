using Godot;
using Safari.Scripts.Game.Entities.Animals;
using Safari.Scripts.Game.Entities.Animals.UsableTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Hyena :  Carnivore
{
	public override float Speed { get { return 110; } }
	public override string AnimalsName { get { return "Hyena"; } }
	
}

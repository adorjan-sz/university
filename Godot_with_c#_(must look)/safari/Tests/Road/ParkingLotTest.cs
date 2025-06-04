using Godot;
using GdUnit4;
using System.Collections.Generic;
using Safari.Scripts.Game.Road;
using static GdUnit4.Assertions;

namespace Safari.Tests.Road
{
	[TestSuite]
	public class ParkingLotTest
	{
		[TestCase]
		public void Constructor_CreatesCorrectNumberOfSlots()
		{
			var entrance = new Vector2I(5, 5);
			var lot = new ParkingLot(entrance, 2);

			// Expected slots = (2*width + 1) = 5
			AssertThat(lot.Slots.Count).IsEqual(5);

			// Check that all positions are at X = entrance.X - 3 and Y in [3,4,5,6,7]
			for (int i = -2; i <= 2; i++)
			{
				var expectedPos = new Vector2I(2, 5 + i); // X = 5 - 3 = 2
				AssertThat(lot.Slots.Exists(slot => slot.GridPosition == expectedPos)).IsTrue();
			}
		}

		[TestCase]
		public void GetFreeSlot_ReturnsFirstUnoccupied()
		{
			var lot = new ParkingLot(new Vector2I(5, 5), 1);

			// All slots should initially be unoccupied
			var free = lot.GetFreeSlot();
			AssertThat(free).IsNotNull();
			AssertThat(free.IsOccupied).IsFalse();

			// Mark the first as occupied, test again
			free.IsOccupied = true;
			var next = lot.GetFreeSlot();
			AssertThat(next).IsNotNull();
			AssertThat(next).IsNotEqual(free);
			AssertThat(next.IsOccupied).IsFalse();
		}

		[TestCase]
		public void GetFreeSlot_ReturnsNullIfAllOccupied()
		{
			var lot = new ParkingLot(new Vector2I(0, 0), 0); // Only one slot
			var slot = lot.Slots[0];
			slot.IsOccupied = true;

			var result = lot.GetFreeSlot();
			AssertThat(result).IsNull();
		}

		[TestCase]
		public void ReleaseSlot_MarksSlotAsFree()
		{
			var lot = new ParkingLot(new Vector2I(0, 0), 0);
			var slot = lot.Slots[0];
			slot.IsOccupied = true;

			lot.ReleaseSlot(slot);
			AssertThat(slot.IsOccupied).IsFalse();
		}

		[TestCase]
		public void ReleaseSlot_DoesNothingForInvalidSlot()
		{
			var lot = new ParkingLot(new Vector2I(0, 0), 0);
			var fakeSlot = new JeepParkingSlot(new Vector2I(99, 99)) { IsOccupied = true };

			lot.ReleaseSlot(fakeSlot); // not part of the lot
			AssertThat(fakeSlot.IsOccupied).IsTrue(); // should remain unchanged
		}
	}
}

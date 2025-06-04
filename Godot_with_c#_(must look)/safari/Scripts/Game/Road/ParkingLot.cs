using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safari.Scripts.Game.Road
{
    /// <summary>
    /// Manages parking slots for Jeeps around the park entrance.
    /// </summary>
    public class ParkingLot
    {
        /// <summary>
        /// All parking slots available.
        /// </summary>
        public List<JeepParkingSlot> Slots { get; private set; }

        /// <summary>
        /// Creates a parking lot around the given entrance, with the specified horizontal range.
        /// </summary>
        /// <param name="entrance">Grid coordinate of the park entrance.</param>
        /// <param name="width">Number of cells to each side of the entrance for parking slots.</param>
        public ParkingLot(Vector2I entrance, int width)
        {
            Slots = [];
            GenerateSlots(entrance, width);
        }

        /// <summary>
        /// Generates slots around entrance cell.
        /// </summary>
        private void GenerateSlots(Vector2I entrance, int width)
        {
            int slotRow = entrance.X - 3;

            for (int y = -width; y <= width; ++y)
                Slots.Add(new JeepParkingSlot(new Vector2I(slotRow, entrance.Y + y)));
        }

        /// <summary>
        /// Returns the first free slot, or null if none available.
        /// </summary>
        public JeepParkingSlot GetFreeSlot()
        {
            return Slots.FirstOrDefault(s => !s.IsOccupied);
        }

        /// <summary>
        /// Marks the given slot as free again.
        /// </summary>
        public void ReleaseSlot(JeepParkingSlot slot)
        {
            if (slot != null && Slots.Contains(slot))
                slot.IsOccupied = false;
        }
    }


    /// <summary>
    /// Represents a single parking slot at a grid position.
    /// </summary>
    public class JeepParkingSlot
    {
        /// <summary>
        /// Grid coordinate for this slot.
        /// </summary>
        public Vector2I GridPosition { get; }

        /// <summary>
        /// Whether this slot is currently occupied by a Jeep.
        /// </summary>
        public bool IsOccupied { get; set; }

        public JeepParkingSlot(Vector2I pos)
        {
            GridPosition = pos;
            IsOccupied = false;
        }
    }
}

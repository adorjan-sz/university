using Godot;
using System.Collections.Generic;

namespace Safari.Scripts.Game.Entities
{
    /// <summary>
    /// Represents a tourist who tracks animals seen during the safari tour
    /// and emits a review score between 1 and 5 at the end.
    /// </summary>
    public partial class Tourist : Node
    {
        [Signal] public delegate void ReviewEventHandler(float sum);

        private int _totalAnimalSeen;
        private Dictionary<string, int> _speciesCounts;
        public int TotalAnimalSeen => _totalAnimalSeen;
        public Dictionary<string, int> SpeciesCounts => _speciesCounts;
        public Tourist()
        {
            _totalAnimalSeen = 0;
            _speciesCounts = [];
        }

        /// <summary>
        /// Call whenever the tourist spots an animal of the given species.
        /// </summary>
        public void SeeAnimal(string speciesName)
        {
            _totalAnimalSeen++;
            if (_speciesCounts.ContainsKey(speciesName))
                _speciesCounts[speciesName]++;
            else
                _speciesCounts[speciesName] = 1;
        }

        public void LeaveReview()
        {
            // 1) Diversity component
            int speciesCount = _speciesCounts.Count;
            float diversityRatio = Mathf.Clamp(speciesCount / 4f, 0f, 1f);

            // 2) Quantity component
            float quantityRatio = Mathf.Clamp(_totalAnimalSeen / 20f, 0f, 1f);

            // 3) Price component (penalty)
            //    We define an "ideal" price, above which satisfaction falls.
            float idealPrice = 50f;
            float ticketPrice = GameVariables.Instance.GetTicketPrice();
            //    If ticketPrice ≤ ideal, no penalty; if above, penalty grows up to 100% at 2× ideal
            float priceRatio = Mathf.Clamp((ticketPrice - idealPrice) / idealPrice, 0f, 1f);

            // 4) Weighted sum before scaling
            //    Let’s weight: 50% diversity, 30% quantity, 20% price penalty
            float weighted = 0.5f * diversityRatio +
                             0.3f * quantityRatio -
                             0.2f * priceRatio;

            // 5) Clamp weighted into [0..1]
            weighted = Mathf.Clamp(weighted, 0f, 1f);

            // 6) Map 0→1 to 1→5 star scale
            float score = 1f + 4f * weighted;

            EmitSignal(nameof(Review), score);
        }

        public void BuyTicet()
        {
            GameVariables.Instance.BuyTicket();
        }
        //-------------Save/Load-------------------
        public TouristSave Save()
        {
            return new TouristSave
            {
                TotalAnimalSeen = _totalAnimalSeen,
                SpeciesCounts = new Dictionary<string, int>(_speciesCounts)
            };
        }
        public void Load(TouristSave save)
        {
            _totalAnimalSeen = save.TotalAnimalSeen;
            _speciesCounts = new Dictionary<string, int>(save.SpeciesCounts);
        }

    }


}

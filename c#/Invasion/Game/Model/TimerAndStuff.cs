using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Game.Model
{
    public interface ITimer
    {
        /// <summary>
        /// Aktív-e (fut-e) az időzítő.
        /// </summary>
        /// 
        bool Enabled { get; set; }
        /// <summary>
        /// Időzítő intervalluma.
        /// </summary>
        double Interval { get; set; }

        /// <summary>
        /// Időzítő eseménye.
        /// </summary>
        event EventHandler? Elapsed;

        /// <summary>
        /// Időzítő elindítása.
        /// </summary>
        /// 
        void Start();
        /// <summary>
        /// Időzítő leállítása.
        /// </summary>
        void Stop();
    }
    public class SudokuTimerAggregation : ITimer
    {
        // Aggregálunk egy System.Timers.Timer példányt.
        private readonly System.Timers.Timer _timer;

        public bool Enabled
        {
            get => _timer.Enabled;
            set => _timer.Enabled = value;
        }

        public double Interval
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }

        public event EventHandler? Elapsed;

        public SudokuTimerAggregation()
        {
            _timer = new System.Timers.Timer();
            _timer.Elapsed += (sender, e) =>
            {
                Elapsed?.Invoke(sender, e);
            };
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
    public class TimerInheritance : System.Timers.Timer, ITimer
    {
        private readonly Dictionary<EventHandler, ElapsedEventHandler> _delegateMapper = new();

        // Definiálunk egy Elapsed eseményt és elfedjük vele a System.Timers.Timer-től örökölt azonos nevű eseményt
        public new event EventHandler? Elapsed
        {
            // amikor feliratkoznak az eseményre ...
            add
            {
                if (value != null)
                {
                    var handler = new ElapsedEventHandler(value.Invoke);
                    // egy ElapsedEventHandler-be csomagoljuk az EventHandler-t
                    // (típusbiztos az eseményargumentum típusának kontravarianciája miatt)
                    _delegateMapper.Add(value, handler);
                    // eltároljuk az (EventHandler, ElapsedEventHandler) párost,
                    // erre az eseményről leiratkozás támogatásához van szükség
                    base.Elapsed += handler;
                }
            }
            // amikor leiratkoznak az eseményről ...
            remove
            {
                // előkeressük az EventHandler-hez tartozó ElapsedEventHandler-t
                if (value != null && _delegateMapper.TryGetValue(value, out var handler))
                {
                    _delegateMapper.Remove(value);
                    base.Elapsed -= handler;
                    // leiratkozunk vele
                }
            }
        }
    }
}

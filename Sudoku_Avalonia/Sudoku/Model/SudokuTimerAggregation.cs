﻿using System;
using System.Collections.Generic;
using System.Timers;

namespace ELTE.Sudoku.Model
{
    /// <summary>
    /// Időzítő a <see cref="System.Timers.Timer"/> aggregálásával, az <see cref="ITimer"/> interfészt megvalósítva.
    /// </summary>
    public class LabyrinthTimerAggregation : ITimer
    {
        // Aggregálunk egy System.Timers.Timer példányt.
        private readonly Timer _timer;

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

        public LabyrinthTimerAggregation()
        {
            _timer = new Timer();
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
}
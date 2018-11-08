﻿using ATM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Interfaces
{
    public class SeperationAlertEventArgs : EventArgs
    {
        public Track ConflictingTrack1 { get; set; }

        public Track ConflictingTrack2 { get; set; }

        public SeperationAlertEventArgs(Track track1, Track track2)
        {
            ConflictingTrack1 = track1;
            ConflictingTrack2 = track2;
        }
    }

    public interface ISeperationChecker
    {
        event EventHandler<SeperationAlertEventArgs> SeperationAlert;
    }
}
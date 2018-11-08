﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Interfaces
{
    public interface ICalculator
    {
        double CalVelocity(Track prevTrack, Track newTrack);
        double CalCourse(Track prevTrack, Track newTrack);
    }
}
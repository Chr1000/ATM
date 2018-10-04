﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    public class Track
    {
        public string Tag { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Altitude { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Course { get; set; }
        public double Velocity { get; set; }

        public override string ToString()
        {
            string finalstring = "";
            finalstring = "Tag:" + Tag + " X: " + X.ToString() + " Y: " + Y.ToString() + " Alt: " + Altitude.ToString() + " Time: " + TimeStamp.ToString() + " Vel: " +  Velocity.ToString() ;     
            return finalstring;
        }
    }
}
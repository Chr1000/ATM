using System;
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

<<<<<<< HEAD
=======
        public Track()
        {
            X = 0;
            Y = 0;
            Altitude = 0;
            Velocity = 0;
            Course = 0;
            TimeStamp = DateTime.MinValue;
        }

        public override string ToString()
        {
            string finalstring = "";
            finalstring = "Tag:" + Tag + " X: " + X.ToString() + " Y: " + Y.ToString() + " Alt: " + Altitude.ToString() + " Time: " + TimeStamp.ToString() + " Vel: " +  Velocity.ToString() ;     
            return finalstring;
        }
>>>>>>> 4d4492fb900880c86b0d7020d641d85dace05164
    }
}
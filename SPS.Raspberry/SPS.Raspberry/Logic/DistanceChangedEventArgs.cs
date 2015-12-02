using System;

namespace SPS.Raspberry.Logic
{
    public class DistanceChangedEventArgs : EventArgs
    {
        public DistanceChangedEventArgs(double distance)
        {
            Distance = distance;
        }

        public double Distance { get; private set; }
    }
}

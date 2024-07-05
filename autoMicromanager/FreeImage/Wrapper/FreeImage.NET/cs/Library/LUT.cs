using System;
using System.Collections.Generic;
using System.Text;

namespace FreeImageAPI
{
    /// <summary>
    /// A simple class to implement a contrast control
    /// and providing additional functions.
    /// </summary>
    public class LUTControl
    {
        /// <summary>
        /// The maximum intensity in percent
        /// </summary>
        public double  MaxContrast;
        /// <summary>
        /// The minimum intesity in percent
        /// </summary>
        public double  MinContrast;

        /// <summary>
        /// Simple constructor to get both set
        /// <param name="Max">Max value for white</param>
        /// <param name="Min">Min Value for black</param>
        /// </summary>
        public LUTControl(double  Min, double  Max)
        {
            MinContrast = Min;
            MaxContrast = Max;
        }
    }
}

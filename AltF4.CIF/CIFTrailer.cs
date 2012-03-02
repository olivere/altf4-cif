using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    public class CIFTrailer
    {
        public int ItemCount { get; set; }

        public string Timestamp { get; set; }

        internal static CIFTrailer Parse(string trailerData)
        {
            // TODO parse trailer data here
            return null;
        }
    }
}

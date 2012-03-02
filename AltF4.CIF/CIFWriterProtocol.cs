using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    public abstract class CIFWriterProtocol : ICIFWriterProtocol
    {
        public abstract CIFHeader GetCIFHeader();

        public abstract string[] GetCIFFieldNames();

        public abstract IEnumerable<CIFItem> GetCIFItemEnumerator();
        
        public virtual CIFTrailer GetCIFTrailer()
        {
            return null;
        }
    }
}

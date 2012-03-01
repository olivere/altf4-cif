using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    public interface ICIFWriterProtocol
    {
        CIFHeader GetCIFHeader();

        IEnumerable<CIFItem> GetCIFItemEnumerator();

        CIFTrailer GetCIFTrailer();
    }
}

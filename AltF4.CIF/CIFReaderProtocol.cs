using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    public abstract class CIFReaderProtocol : ICIFReaderProtocol
    {
        public abstract void HandleHeader(CIFHeader header);

        public abstract void HandleItem(CIFItem item);

        public abstract void HandleTrailer(CIFTrailer trailer);
    }
}

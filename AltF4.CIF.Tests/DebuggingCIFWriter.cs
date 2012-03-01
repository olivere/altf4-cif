using System.Collections.Generic;

namespace AltF4.CIF.Tests
{
    public class DebuggingCIFWriter : CIFWriterProtocol
    {
        public DebuggingCIFWriter()
        {
            Items = new List<CIFItem>();
        }

        public int HeaderCalls { get; set; }
        public CIFHeader Header { get; set; }

        public int ItemCalls { get; set; }
        public List<CIFItem> Items { get; set; }

        public int TrailerCalls { get; set; }
        public CIFTrailer Trailer { get; set; }

        public override CIFHeader GetCIFHeader()
        {
            HeaderCalls += 1;
            return Header;
        }

        public override IEnumerable<CIFItem> GetCIFItemEnumerator()
        {
            ItemCalls += 1;
            return Items;
        }

        public override CIFTrailer GetCIFTrailer()
        {
            TrailerCalls += 1;
            return Trailer;
        }
    }
}
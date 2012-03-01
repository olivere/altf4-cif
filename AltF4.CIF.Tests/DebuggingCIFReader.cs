using System.Collections.Generic;

namespace AltF4.CIF.Tests
{
    public class DebuggingCIFReader : CIFReaderProtocol
    {
        public DebuggingCIFReader()
        {
            Items = new List<CIFItem>();
        }

        public int HeaderCalls { get; set; }
        public CIFHeader Header { get; set; }

        public int ItemCalls { get; set; }
        public List<CIFItem> Items { get; set; }

        public int TrailerCalls { get; set; }
        public CIFTrailer Trailer { get; set; }

        public override void HandleHeader(CIFHeader header)
        {
            HeaderCalls += 1;
            Header = header;
        }

        public override void HandleItem(CIFItem item)
        {
            ItemCalls += 1;
            Items.Add(item);
        }

        public override void HandleTrailer(CIFTrailer trailer)
        {
            TrailerCalls += 1;
            Trailer = trailer;
        }
    }
}
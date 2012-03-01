using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    public class CIFReader : IDisposable
    {
        private readonly StreamReader _reader;

        public CIFReader(StreamReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            _reader = reader;
        }

        public void Dispose()
        {
        }

        public void Read(ICIFReaderProtocol protocol)
        {
            if (protocol == null)
                throw new ArgumentNullException("protocol");

            // Read until we find "DATA"
            var headerData = new StringBuilder();
            while (true)
            {
                var line = _reader.ReadLine();

                if (line == null || 0 == string.Compare("DATA", line, StringComparison.InvariantCulture))
                    break;

                headerData.AppendLine(line);
            }

            var header = CIFHeader.Parse(headerData.ToString());
            protocol.HandleHeader(header);

            // Read all items
            int currentItem = 0;
            while(true)
            {
                var line = _reader.ReadLine();
                ++currentItem;

                if (line == null || 0 == string.Compare("ENDOFDATA", line, StringComparison.InvariantCulture))
                    break;

                var item = CIFItem.Parse(header, line);
                protocol.HandleItem(item);
            }

            // Read trailer
            var trailerData = new StringBuilder();
            while(true)
            {
                var line = _reader.ReadLine();

                if (line == null)
                    break;

                trailerData.AppendLine(line);
            }

            //var trailer = CIFTrailer.Parse(trailerData.ToString());
            //protocol.HandleTrailer(trailer);
        }
    }
}

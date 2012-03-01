using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    /// <summary>
    /// Ermöglicht das Schreiben einer CIF-Datei.
    /// </summary>
    public class CIFWriter
    {
        private readonly StreamWriter _writer;

        public CIFWriter(StreamWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            _writer = writer;
        }

        public void Dispose()
        {
        }

        public void Write(ICIFWriterProtocol protocol)
        {
            if (protocol == null)
                throw new ArgumentNullException("protocol");

            var header = protocol.GetCIFHeader();

            _writer.WriteLine(header.Version);

            if (header.Currency != null)
                _writer.WriteLine("CURRENCY:" + header.Currency);

            if (header.Charset != null)
                _writer.WriteLine("CHARSET:" + header.Charset);

            if (header.CodeFormat != null)
                _writer.WriteLine("CODEFORMAT:" + header.CodeFormat);

            if (header.Comments != null)
                _writer.WriteLine("COMMENTS:" + header.Comments.Replace("\r\n", " "));

            if (header.DUNS.HasValue)
                _writer.WriteLine("DUNS:" + (header.DUNS.Value ? "True" : "False"));

            _writer.WriteLine("ITEMCOUNT:" + header.ItemCount);

            _writer.WriteLine("LOADMODE:" + header.LoadMode);

            if (header.SupplierIDDomain != null)
                _writer.WriteLine("SUPPLIERID_DOMAIN:" + header.SupplierIDDomain);

            if (header.Timestamp != null)
                _writer.WriteLine("TIMESTAMP:" + header.Timestamp);

            if (!header.UNUoM.HasValue)
                _writer.WriteLine("UNUOM:" + (header.UNUoM.Value ? "True" : "False"));

            if (!string.IsNullOrEmpty(header.FieldNames))
                _writer.WriteLine("FIELDNAMES:" + header.FieldNames);

            // DATA
            _writer.WriteLine("DATA");

            var enumerator = protocol.GetCIFItemEnumerator();
            foreach(var item in enumerator)
            {
                _writer.WriteLine(item.ToCSV());
            }

            // ENDOFDATA
            _writer.WriteLine("ENDOFDATA");

            // TODO write TRAILER
            var trailer = protocol.GetCIFTrailer();
            if (trailer != null)
            {
                // TODO write TRAILER
            }
        }
    }
}

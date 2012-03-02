using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<string> _gatheredFieldNames = null;

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

            // Write header
            var header = protocol.GetCIFHeader();

            // Writes header fields
            WriteHeader(header, protocol);

            // DATA
            WriteData(header, protocol);

            // TRAILER
            WriteTrailer(header, protocol);
        }

        private void WriteHeader(CIFHeader header, ICIFWriterProtocol protocol)
        {
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

            // Always writes all fields here, regardless of whether they're set
            // There's room for optimization here
            _gatheredFieldNames = GatherFieldNamesInOrder(protocol);
            _writer.WriteLine("FIELDNAMES:" + string.Join(",", _gatheredFieldNames));
        }

        private List<string> GatherFieldNamesInOrder(ICIFWriterProtocol protocol)
        {
            var list = new List<string>();

            // Add required fields first
            list.AddRange(Constants.RequiredFields);

            // Add optional fields next
            list.AddRange(Constants.OptionalFields);

            // Add all other, non-spec'd field names
            var fieldNamesFromProtocol = protocol.GetCIFFieldNames();
            if (fieldNamesFromProtocol != null && fieldNamesFromProtocol.Length > 0)
            {
                foreach(var field in fieldNamesFromProtocol)
                {
                    if (!list.Contains(field))
                        list.Add(field);
                }
            }

            return list;
        }

        private void WriteData(CIFHeader header, ICIFWriterProtocol protocol)
        {
            Debug.Assert(_gatheredFieldNames != null && _gatheredFieldNames.Count > 0,
                         "Field names should have been gathered here");

            // DATA
            _writer.WriteLine("DATA");

            var enumerator = protocol.GetCIFItemEnumerator();
            foreach (var item in enumerator)
            {
                _writer.WriteLine(item.ToCSV(_gatheredFieldNames));
            }

            // ENDOFDATA
            _writer.WriteLine("ENDOFDATA");
        }

        private void WriteTrailer(CIFHeader header, ICIFWriterProtocol protocol)
        {
            var trailer = protocol.GetCIFTrailer();
            if (trailer != null)
            {
                // TODO write TRAILER
            }
        }
    }
}

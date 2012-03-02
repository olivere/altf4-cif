using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    /// <summary>
    /// CIF Header information.
    /// </summary>
    public class CIFHeader
    {
        private IList<string> _fieldNamesList = null;

        /// <summary>
        /// Initializes a new CIF header and sets <see cref="Version"/>
        /// to <see cref="Constants.CIFVersion30"/>.
        /// </summary>
        public CIFHeader()
        {
            Version = Constants.CIFVersion30;
        }

        /// <summary>
        /// Specifies CIF file format and version, e.g. 
        /// <c>CIF_I_V3.0</c> oder <c>CIF_I_V2.1</c>.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// CIF version 2.1?
        /// </summary>
        public bool IsCIF21 { get { return Version == "CIF_I_V2.1"; } }

        /// <summary>
        /// CIF version 3.0?
        /// </summary>
        public bool IsCIF30 { get { return Version == "CIF_I_V3.0"; } }

        /// <summary>
        /// Currency code according to ISO 4217.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Character set.
        /// </summary>
        public string Charset { get; set; }
        
        /// <summary>
        /// Specifies the default commodity code system, e.g. <c>UNSPSC</c>.
        /// </summary>
        public string CodeFormat { get; set; }

        /// <summary>
        /// Arbitrary comment text.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Indicates whether supplier IDs are D-U-N-S numbers.
        /// </summary>
        public bool? DUNS { get; set; }

        /// <summary>
        /// Declares the columns in the body of the CIF file.
        /// </summary>
        public string FieldNames { get; internal set; }

        /// <summary>
        /// Columns in the body of the CIF file as a list.
        /// </summary>
        public IList<string> FieldNamesList
        {
            get
            {
                if (_fieldNamesList == null)
                {
                    _fieldNamesList = CIFItem.ParseCSV(FieldNames, true);
                }
                return _fieldNamesList;
            }
            internal set { _fieldNamesList = value; }
        }

        /// <summary>
        /// Number of items in the file.
        /// </summary>
        public int? ItemCount { get; set; }

        /// <summary>
        /// Declares whether the catalog is complete ('F' or full) 
        /// or an update ('I' for incremental).
        /// </summary>
        public char? LoadMode { get; set; }

        /// <summary>
        /// Full update?
        /// </summary>
        public bool IsFull { get { return LoadMode == 'F'; } }

        /// <summary>
        /// Incremental update?
        /// </summary>
        public bool IsIncremental { get { return LoadMode == 'I'; } }

        /// <summary>
        /// Specifies the domain for Supplier IDs used in the CIF file,
        /// e.g. <c>DUNS</c> or <c>NETWORK_ID</c>.
        /// </summary>
        public string SupplierIDDomain { get; set; }

        /// <summary>
        /// Timestamp in the format YYYY-MM-DD hh:mm:ss.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Tries to parse <see cref="Timestamp"/> and return
        /// a <see cref="DateTime"/>.
        /// </summary>
        public DateTime? TimestampDate
        {
            get
            {
                DateTime ts;
                if (DateTime.TryParse(Timestamp, out ts))
                {
                    return ts;
                }
                return null;
            }
            set
            {
                Timestamp = value != null
                                ? value.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                                : null;
            }
        }

        /// <summary>
        /// Indicates whether this file uses 
        /// United Nations Units of Measure (UNUOM).
        /// </summary>
        public bool? UNUoM { get; set; }

        /// <summary>
        /// Tries to evaluate <see cref="Charset"/> as a valid
        /// .NET encoding (<see cref="Encoding"/>).
        /// </summary>
        /// <param name="encoding">Output encoding or <lang>null</lang></param>
        /// <returns>Returns valid encoding or <lang>null</lang></returns>
        public bool TryGetEncoding(out Encoding encoding)
        {
            try
            {
                // Special case: 8859_1 ist der CIF-Standard, wird aber nicht von .NET erkannt
                if (0 == string.Compare("8859_1", Charset, StringComparison.InvariantCultureIgnoreCase))
                {
                    encoding = Encoding.GetEncoding("iso-8859-1");
                }
                else
                {
                    encoding = Encoding.GetEncoding(Charset);
                }
            }
            catch
            {
                encoding = null;
            }
            return encoding != null;
        }

        /// <summary>
        /// Tries to parse <paramref name="headerData"/> as a
        /// <see cref="CIFHeader"/>.
        /// </summary>
        /// <param name="headerData">Header data as string</param>
        /// <returns>An instance of <see cref="CIF"/>.</returns>
        internal static CIFHeader Parse(string headerData)
        {
            var header = new CIFHeader();

            var lines = headerData.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            for(int i=0; i<lines.Length; ++i)
            {
                var line = lines[i];

                if (i == 0)
                {
                    // First line is version
                    header.Version = line;
                    continue;
                }

                var kvp = line.Split(new[] {':'}, 2, StringSplitOptions.RemoveEmptyEntries);
                var key = kvp[0];
                var value = kvp[1];

                if (0 == string.Compare(key, "CURRENCY", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.Currency = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "CHARSET", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.Charset = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "CODEFORMAT", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.CodeFormat = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "COMMENTS", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.Comments = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "DUNS", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.DUNS = bool.Parse(value.Trim());
                    continue;
                }

                if (0 == string.Compare(key, "FIELDNAMES", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.FieldNames = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "ITEMCOUNT", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.ItemCount = int.Parse(value.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture);
                    continue;
                }

                if (0 == string.Compare(key, "LOADMODE", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.LoadMode = value.Trim().FirstOrDefault();
                    continue;
                }

                if (0 == string.Compare(key, "SUPPLIERID_DOMAIN", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.SupplierIDDomain = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "TIMESTAMP", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.Timestamp = value.Trim();
                    continue;
                }

                if (0 == string.Compare(key, "UNUOM", StringComparison.InvariantCultureIgnoreCase))
                {
                    header.UNUoM = bool.Parse(value.Trim());
                    continue;
                }
            }

            return header;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    /// <summary>
    /// Represents a single item in a CIF file.
    /// </summary>
    public class CIFItem
    {
        private readonly Dictionary<string,string> _dict = new Dictionary<string, string>();
        private readonly List<string> _errors = new List<string>(); 

        public CIFItem()
        {            
        }

        public bool IsEmpty
        {
            get { return !_dict.Any(); }
        }

        #region Set required, optional, and custom fields

        /*
        public string this[int index]
        {
            get { return _dict.Values.ElementAt(index); }
        }
        */

        public string this[string name]
        {
            get { return _dict.ContainsKey(name) ? _dict[name] : null; }
            set { _dict[name] = value; }
        }

        #endregion

        #region Required fields

        public string SupplierID
        {
            get { return this[Constants.SupplierID]; }
            set { this[Constants.SupplierID] = value; }
        }

        public string SupplierPartID
        {
            get { return this[Constants.SupplierPartID]; }
            set { this[Constants.SupplierPartID] = value; }
        }

        public string ManufacturerPartID
        {
            get { return this[Constants.ManufacturerPartID]; }
            set { this[Constants.ManufacturerPartID] = value; }
        }

        public string ItemDescription
        {
            get { return this[Constants.ItemDescription]; }
            set { this[Constants.ItemDescription] = value; }
        }

        public string SPSCCode
        {
            get { return this[Constants.SPSCCode]; }
            set { this[Constants.SPSCCode] = value; }
        }

        public decimal? UnitPrice
        {
            get
            {
                var value = this[Constants.UnitPrice];
                if (string.IsNullOrEmpty(value))
                    return null;
                return decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
            }
            set
            {
                this[Constants.UnitPrice] = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
            }
        }

        /// <summary>
        /// Returns the unit of measure. 
        /// </summary>
        /// <remarks>
        /// Notice that the spec specifies this field as <c>Units of Measure</c>
        /// while the examples in the spec use <c>Unit of Measure</c>. 
        /// We only check for <c>Units of Measure</c>. If you want 
        /// <c>Unit of Measure</c>, you need to use the indexer.
        /// </remarks>
        public string UnitsOfMeasure
        {
            get { return this[Constants.UnitsOfMeasure]; }
            set { this[Constants.UnitsOfMeasure] = value; }
        }

        public int? LeadTime
        {
            get
            {
                var value = this[Constants.LeadTime];
                if (string.IsNullOrEmpty(value))
                    return null;
                return int.Parse(value, NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            set { this[Constants.LeadTime] = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : null; }
        }

        public string ManufacturerName
        {
            get { return this[Constants.ManufacturerName]; }
            set { this[Constants.ManufacturerName] = value; }
        }

        public string SupplierURL
        {
            get { return this[Constants.SupplierURL]; }
            set { this[Constants.SupplierURL] = value; }
        }

        public string ManufacturerURL
        {
            get { return this[Constants.ManufacturerURL]; }
            set { this[Constants.ManufacturerURL] = value; }
        }

        public decimal? MarketPrice
        {
            get
            {
                var value = this[Constants.MarketPrice];
                if (string.IsNullOrEmpty(value))
                    return null;
                return decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
            }
            set { this[Constants.MarketPrice] = value != null ? value.Value.ToString(CultureInfo.InvariantCulture) : null; }
        }

        #endregion

        #region Optional fields

        public string Tier
        {
            get { return this[Constants.Tier]; }
            set { this[Constants.Tier] = value; }
        }

        public string Name
        {
            get { return this[Constants.Name]; }
            set { this[Constants.Name] = value; }
        }

        public string Language
        {
            get { return this[Constants.Language]; }
            set { this[Constants.Language] = value; }
        }

        public string Currency
        {
            get { return this[Constants.Currency]; }
            set { this[Constants.Currency] = value; }
        }

        public DateTime? ExpirationDate
        {
            get
            {
                var value = this[Constants.ExpirationDate];
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                {
                    return dt;
                }
                return null;
            }
            set
            {
                this[Constants.ExpirationDate] = value != null
                                                     ? value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                     : null;
            }
        }

        public DateTime? EffectiveDate
        {
            get
            {
                var value = this[Constants.EffectiveDate];
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                {
                    return dt;
                }
                return null;
            }
            set
            {
                this[Constants.EffectiveDate] = value != null
                                                     ? value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                     : null;
            }
        }

        public IDictionary<string,string> ClassificationCodes
        {
            get
            {
                return ParseHash(this[Constants.ClassificationCodes]);
            }
            set
            {
                this[Constants.ClassificationCodes] = ToHash(value);
            }
        }

        public IDictionary<string,string> ParametricData
        {
            get
            {
                return ParseHash(this[Constants.ParametricData]);
            }
            set
            {
                this[Constants.ParametricData] = ToHash(value);
            }
        }

        public string ParametricName
        {
            get { return this[Constants.ParametricName]; }
            set { this[Constants.ParametricName] = value; }
        }

        /// <summary>
        /// Specifies a PunchOut catalog item.
        /// </summary>
        /// <remarks>
        /// The spec mentions that values <c>True</c>, <c>true</c>,
        /// <c>t</c>, and <c>1</c> (case-insensitive) represent
        /// <lang>true</lang> and <c>False</c>, <c>false</c>,
        /// <c>f</c>, and <c>0</c> (case-insensitive) represent
        /// <lang>false</lang>.
        /// If none of the above values is found, <lang>null</lang>
        /// is returned.
        /// </remarks>
        public bool? PunchOutEnabled
        {
            get
            {
                var value = this[Constants.PunchOutEnabled];

                if (0 == string.Compare(value, "true", true, CultureInfo.InvariantCulture) ||
                    0 == string.Compare(value, "t", true, CultureInfo.InvariantCulture) || 
                    0 == string.Compare(value, "1", true, CultureInfo.InvariantCulture))
                {
                    return true;
                }
                if (0 == string.Compare(value, "false", true, CultureInfo.InvariantCulture) || 
                    0 == string.Compare(value, "f", true, CultureInfo.InvariantCulture) ||
                    0 == string.Compare(value, "0", true, CultureInfo.InvariantCulture))
                {
                    return false;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this[Constants.PunchOutEnabled] = value.Value ? "True" : "False";
                }
                else
                {
                    this[Constants.PunchOutEnabled] = null;
                }
            }
        }

        public string TerritoryAvailable
        {
            get { return this[Constants.TerritoryAvailable]; }
            set { this[Constants.TerritoryAvailable] = value; }
        }

        public string SupplierPartAuxiliaryID
        {
            get { return this[Constants.SupplierPartAuxiliaryID]; }
            set { this[Constants.SupplierPartAuxiliaryID] = value; }
        }

        public bool? Delete
        {
            get
            {
                var value = this[Constants.Delete];
                bool delete;
                if (bool.TryParse(value, out delete))
                {
                    return delete;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this[Constants.Delete] = value.Value ? "True" : "False";
                }
                else
                {
                    this[Constants.Delete] = null;
                }
            }
        }

        #endregion

        public bool IsValid
        {
            get 
            {
                PerformValidation();
                return !_errors.Any();
            }
        }

        public string[] Errors
        {
            get
            {
                PerformValidation();
                return _errors.ToArray();
            }
        }

        private void PerformValidation()
        {
            var errors = new List<string>();

            foreach (var fieldName in Constants.RequiredFields)
            {
                if (null == this[fieldName])
                {
                    errors.Add(string.Format("{0} is missing", fieldName));
                }
            }

            _errors.Clear();
            _errors.AddRange(errors);
        }

        public bool IsHash(string name)
        {
            var value = this[name];
            return !string.IsNullOrEmpty(value) && value[0] == '{';
        }

        public IDictionary<string,string> GetHash(string name)
        {
            var value = this[name];
            return ParseHash(value);
        }

        // TODO IsCollection

        internal static CIFItem Parse(CIFHeader header, string itemData)
        {
            var item = new CIFItem();

            if (string.IsNullOrEmpty(itemData))
                return item;

            var values = ParseCSV(itemData, false);

            // TODO use header encoding for item data here

            var names = header.FieldNamesList;
            if (names != null && names.Any())
            {
                var namesArray = names.ToArray();
                var valuesArray = values.ToArray();

                // Field names are specified in header
                Debug.Assert(valuesArray.Length == namesArray.Length, "Field names count != field values count");

                for (int i = 0; i < namesArray.Length; ++i)
                {
                    var name = namesArray[i];
                    var value = valuesArray[i];

                    // Handle special cases like CIF Hashes in CIFItem instance
                    item[name] = !string.IsNullOrEmpty(value) ? value : null;
                }
            }
            else
            {
                // Field names are not specified, so use the (12) default fields from CIF 2.1
                var namesArray = Constants.RequiredFields;
                var valuesArray = values.ToArray();

                for (int i = 0; i < namesArray.Length; ++i)
                {
                    var name = namesArray[i];
                    var value = valuesArray[i];

                    // Handle special cases like CIF Hashes in CIFItem instance
                    item[name] = !string.IsNullOrEmpty(value) ? value : null;
                }
            }

            return item;
        }

        internal string ToCSV(IEnumerable<string> fieldNames)
        {
            if (fieldNames == null || !fieldNames.Any())
                return string.Empty;

            var parts = new List<string>();

            var fields = fieldNames.ToArray();
            
            // write CIF fields
            for (int i = 0; i < fields.Count(); ++i)
            {
                parts.Add(Csvify(this[fields[i]]));
            }

            return string.Join(Constants.DefaultFieldSep.ToString(CultureInfo.InvariantCulture),
                               parts);
        }

        private static string Csvify(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";

            // Serialize as common field
            var needsQuoting = s.IndexOfAny(new[] { ',' }) >= 0;
            if (needsQuoting)
            {
                return string.Format("\"{0}\"", s.Replace("\"", "\"\""));
            }

            return s;
        }

        internal static string ToHash(IDictionary<string,string> dict)
        {
            if (dict == null)
                return string.Empty;

            // Serialize as hash
            var inner = new List<string>();
            foreach (var kvp in dict)
            {
                inner.Add(string.Format("{0}={1}", kvp.Key, Csvify(kvp.Value)));
            }

            return string.Format("{{{0}}}", string.Join(";", inner));
        }

        #region Parse CSV row data

        /// <summary>
        /// Parses a single CSV row.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="Constants.DefaultFieldSep"/> as field
        /// separator and <see cref="Constants.DefaultQuoteChar"/>
        /// as quote character. Furthermore, field values are not trimmed.
        /// </remarks>
        /// <param name="line">Raw line data</param>
        /// <returns>Column data</returns>
        internal static IList<string> ParseCSV(string line)
        {
            return ParseRow(line, Constants.DefaultFieldSep, Constants.DefaultQuoteChar, false);
        }

        /// <summary>
        /// Parses a single CSV row.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="Constants.DefaultFieldSep"/> as field
        /// separator and <see cref="Constants.DefaultQuoteChar"/>
        /// as quote character.
        /// </remarks>
        /// <param name="line">Raw line data</param>
        /// <param name="trim">Trim field values</param>
        /// <returns>Column data</returns>
        internal static IList<string> ParseCSV(string line, bool trim)
        {
            return ParseRow(line, Constants.DefaultFieldSep, Constants.DefaultQuoteChar, trim);
        }

        /// <summary>
        /// Parses a single CSV row.
        /// </summary>
        /// <remarks>
        /// If <paramref name="line"/> is <lang>null</lang> or empty, an empty
        /// enumeration will be returned. In other words: This method never
        /// returns <lang>null</lang>.
        /// </remarks>
        /// <param name="line">Raw line data</param>
        /// <param name="fieldSep">Field separator</param>
        /// <param name="quoteChar">Quote character</param>
        /// <param name="trim">Trim field values</param>
        /// <returns>Column data</returns>
        internal static IList<string> ParseRow(string line, char fieldSep, char quoteChar, bool trim)
        {
            if (string.IsNullOrEmpty(line))
                return new string[0];

            // It's basically a single FSM

            var fields = new List<string>();
            var field = new StringBuilder();
            var insideQuotedField = false;
            var lastChar = (char)0;

            // We only need one pass.
            int i = 0;
            while (i < line.Length)
            {
                char c = line[i];
                lastChar = c;

                bool startOfQuote = c == '"' && (i > 0 && line[i - 1] == fieldSep);

                // Quote char
                if (c == quoteChar)
                {
                    if (insideQuotedField)
                    {
                        // End quoted field
                        insideQuotedField = false;

                        // Edge case: Look for next field sep, i.e. skip whitespace
                        while (i < line.Length && line[i] != fieldSep)
                        {
                            ++i;
                        }
                    }
                    else if (startOfQuote)
                    {
                        // Start quoted field (can only start after a fieldSep)
                        insideQuotedField = true;
                        ++i;
                        field.Remove(0, field.Length);
                    }
                    else
                    {
                        // Quote inside field
                        field.Append(c);
                        ++i;
                    }
                    continue;
                }

                // Field sep
                if (c == fieldSep)
                {
                    if (!insideQuotedField)
                    {
                        var s = trim ? field.ToString().Trim() : field.ToString();
                        fields.Add(s);
                        field.Remove(0, field.Length);
                        ++i;
                    }
                    else
                    {
                        // Comma in a quoted field
                        field.Append(c);
                        ++i;
                    }
                    continue;
                }

                // Normal char
                field.Append(c);
                ++i;
            }

            // Either field is not empty or last char was field sep
            if (field.Length > 0 || lastChar == fieldSep)
            {
                var s = trim ? field.ToString().Trim() : field.ToString();
                fields.Add(s);
            }

            return fields;
        }

        #endregion

        #region Lesen eines CIF Hash-Werts

        /// <summary>
        /// Internal state of the CIF hash parser.
        /// </summary>
        enum CIFHashState : byte
        {
            /// <summary>Reading key</summary>
            ReadingKey = 0,
            /// <summary>Reading value</summary>
            ReadingValue = 1,
            /// <summary>Inside reading value, but inside quote</summary>
            ReadingValueInQuote = 2,
        }

        /// <summary>
        /// Parses a CIF hash value into a .NET Dictionary.
        /// </summary>
        /// <example>
        /// <code>
        /// // Returns a dictionary with 3 key/value pairs
        /// ParseHash("{TYPE=SEAMLESS;SIZE=20;WEIGHT=94.00}");
        /// 
        /// // Accepts whitespace inside key
        /// ParseHash("{HAZARDOUS MATERIAL CODE=3}");
        /// 
        /// // Whitespace in value is invalid according to spec, but who cares
        /// ParseHash("{TYPE =SEAMLESS ; SIZE= 20; WEIGHT = 94.00 }");
        /// 
        /// // Quoting
        /// ParseHash("{TYPE =\"SEAMLESS \"; SIZE=\" 20\"; WEIGHT =\" 94.00 \"}");
        /// 
        /// // Comma
        /// ParseHash("{TYPE=ROUND,FLAT}");
        /// 
        /// // Double-quotes
        /// ParseHash("{TYPE=\"\"ROUND,FLAT\"\"}");
        /// 
        /// // { and } inside value
        /// ParseHash("{TYPE=\"ROUND,{FLAT}\"}");
        /// </code>
        /// </example>
        /// <param name="hashData">Hash data in CIF format</param>
        /// <returns>Dictionary with key/value pairs.</returns>
        internal static IDictionary<string, string> ParseHash(string hashData)
        {
            var dict = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(hashData))
                return dict;

            hashData = hashData.Trim();

            if (hashData[0] != '{' && hashData[hashData.Length - 1] != '}')
                return dict;

            var sb = new StringBuilder();
            string key = null, value = null;

            var state = CIFHashState.ReadingKey;
            int i = 1;
            while (i < hashData.Length)
            {
                char c = hashData[i];

                if (state == CIFHashState.ReadingKey)
                {
                    // End of input?
                    if (c == '}')
                    {
                        break;
                    }

                    if (c == '=')
                    {
                        // End of key
                        key = sb.ToString();
                        sb.Remove(0, sb.Length);
                        state = CIFHashState.ReadingValue;
                        ++i;
                    }
                    else
                    {
                        sb.Append(c);
                        ++i;
                    }
                    continue;
                }

                if (state == CIFHashState.ReadingValue)
                {
                    // End of input?
                    if (c == '}')
                    {
                        break;
                    }

                    if (c == ';')
                    {
                        // End of value
                        value = sb.ToString();
                        sb.Remove(0, sb.Length);
                        if (!string.IsNullOrEmpty(key))
                            dict[key] = value;
                        key = value = null;
                        state = CIFHashState.ReadingKey;
                        ++i;
                    }
                    else if (c == '"')
                    {
                        // watch out for double-quotes
                        if (i != hashData.Length - 1 && hashData[i + 1] != '"')
                        {
                            // ok, no double-quotes
                            state = CIFHashState.ReadingValueInQuote;
                            ++i;
                        }
                        else
                        {
                            // double-quote, so add a single-quote
                            sb.Append(c);
                            ++i;
                            ++i;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                        ++i;
                    }
                    continue;
                }

                if (state == CIFHashState.ReadingValueInQuote)
                {
                    if (c == '"')
                    {
                        // watch out for double-quotes
                        if (i != hashData.Length - 1 && hashData[i + 1] != '"')
                        {
                            // End of quote
                            state = CIFHashState.ReadingValue;
                            ++i;
                        }
                        else
                        {
                            // double-quote, so add a single-quote and advance
                            sb.Append(c);
                            ++i;
                            ++i;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                        ++i;
                    }
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(key))
                dict[key] = sb.ToString();

            return dict;
        }

        #endregion

    }
}

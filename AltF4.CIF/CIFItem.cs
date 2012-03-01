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

        public CIFItem()
        {            
        }

        public bool IsEmpty
        {
            get { return !_dict.Any(); }
        }

        public string this[int index]
        {
            get { return _dict.Values.ElementAt(index); }
        }

        public string this[string name]
        {
            get { return _dict.ContainsKey(name) ? _dict[name] : null; }
            set { _dict[name] = value; }
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

            var values = ParseCSV(itemData);

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
                    item[name] = value;
                }
            }
            else
            {
                // Field names are not specified, so use the (12) default fields from CIF 2.1
                var namesArray = Constants.DefaultFields;
                var valuesArray = values.ToArray();

                for (int i = 0; i < namesArray.Length; ++i)
                {
                    var name = namesArray[i];
                    var value = valuesArray[i];

                    // Handle special cases like CIF Hashes in CIFItem instance
                    item[name] = value;
                }
            }

            return item;
        }

        internal string ToCSV()
        {
            var parts = new List<string>();
            
            // TODO write first (12) default CIF fields
            // TODO sort fields correctly
            foreach(var kvp in _dict)
            {
                if (IsHash(kvp.Key))
                {
                    // Serialize as hash
                    var inner = new List<string>();
                    foreach (var innerkvp in GetHash(kvp.Value))
                    {
                        inner.Add(string.Format("{0}={1}", innerkvp.Key, Csvify(innerkvp.Value)));
                    }
                    parts.Add(string.Format("{{{0}}}", string.Join(";", inner)));
                }
                /*
                else if (IsCollection(kvp.Key))
                {
                    // Serialize as collection
                }
                */
                else
                {
                    // Serialize as common field
                    parts.Add(Csvify(kvp.Value));
                }
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

        #region Parse CSV row data

        /// <summary>
        /// Parses a single CSV row.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="Constants.DefaultFieldSep"/> as field
        /// separator and <see cref="Constants.DefaultQuoteChar"/>
        /// as quote character.
        /// </remarks>
        /// <param name="line">Raw line data</param>
        /// <returns>Column data</returns>
        internal static IList<string> ParseCSV(string line)
        {
            return ParseRow(line, Constants.DefaultFieldSep, Constants.DefaultQuoteChar);
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
        /// <returns>Column data</returns>
        internal static IList<string> ParseRow(string line, char fieldSep, char quoteChar)
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
                    else
                    {
                        // Start quoted field
                        insideQuotedField = true;
                        ++i;
                        field.Remove(0, field.Length);
                    }
                    continue;
                }

                // Field sep
                if (c == fieldSep)
                {
                    if (!insideQuotedField)
                    {
                        fields.Add(field.ToString());
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
                fields.Add(field.ToString());

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

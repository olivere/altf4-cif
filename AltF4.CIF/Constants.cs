using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    /// <summary>
    /// Assembly-wide constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Version string for CIF 2.1.
        /// </summary>
        public const string CIFVersion21 = "CIF_I_V2.1";

        /// <summary>
        /// Version string for CIF 3.0.
        /// </summary>
        public const string CIFVersion30 = "CIF_I_V3.0";

        // First 12 default CIF 2.1 field names
        public const string SupplierID = "Supplier ID";
        public const string SupplierPartID = "Supplier Part ID";
        public const string ManufacturerPartID = "Manufacturer Part ID";
        public const string ItemDescription = "Item Description";
        public const string SPSCCode = "SPSC Code";
        public const string UnitPrice = "Unit Price";
        public const string UnitsOfMeasure = "Units of Measure";
        public const string LeadTime = "Lead Time";
        public const string ManufacturerName = "Manufacturer Name";
        public const string SupplierURL = "Supplier URL";
        public const string ManufacturerURL = "Manufacturer URL";
        public const string MarketPrice = "Market Price";

        public static readonly string[] DefaultFields = {
                                                            SupplierID,
                                                            SupplierPartID,
                                                            ManufacturerPartID,
                                                            ItemDescription,
                                                            SPSCCode,
                                                            UnitPrice,
                                                            UnitsOfMeasure,
                                                            LeadTime,
                                                            ManufacturerName,
                                                            SupplierURL,
                                                            ManufacturerURL,
                                                            MarketPrice,
                                                        };

        internal const char DefaultQuoteChar = '"';
        internal const char DefaultFieldSep = ',';
    }
}

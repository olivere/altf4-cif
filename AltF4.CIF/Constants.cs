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
        // Non-default fields (optional)
        public const string Tier = "Tier";
        public const string Name = "Name";
        public const string Language = "Language";
        public const string Currency = "Currency";
        public const string ExpirationDate = "Expiration Date";
        public const string EffectiveDate = "Effective Date";
        public const string ClassificationCodes = "Classification Codes";
        public const string ParametricData = "Parametric Data";
        public const string ParametricName = "Parametric Name";
        public const string PunchOutEnabled = "PunchOut Enabled";
        public const string TerritoryAvailable = "Territory Available";
        public const string SupplierPartAuxiliaryID = "Supplier Part Auxiliary ID";
        public const string Delete = "Delete";

        public static readonly string[] RequiredFields = {
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

        public static readonly string[] OptionalFields = {
                                                            Tier,
                                                            Name,
                                                            Language,
                                                            Currency,
                                                            ExpirationDate,
                                                            EffectiveDate,
                                                            ClassificationCodes,
                                                            ParametricData,
                                                            ParametricName,
                                                            PunchOutEnabled,
                                                            TerritoryAvailable,
                                                            SupplierPartAuxiliaryID,
                                                            Delete,
                                                        };

        internal const char DefaultQuoteChar = '"';
        internal const char DefaultFieldSep = ',';
    }
}

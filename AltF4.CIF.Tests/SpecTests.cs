using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class SpecTests : BaseTestFixture
    {
        [Test]
        public void ShouldReadMinimalNumberOfFields()
        {
            using (var stream = OpenStreamReader("MinimalNumberOfFields.cif"))
            {
                var reader = new CIFReader(stream);
                var debug = new DebuggingCIFReader();

                reader.Read(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.Header != null);
                Expect(debug.Header.Charset == null);
                Expect(debug.Header.CodeFormat == null);
                Expect(debug.Header.Comments == null);
                Expect(debug.Header.Currency == null);
                Expect(debug.Header.DUNS == null);
                Expect(debug.Header.FieldNames == null);
                Expect(debug.Header.FieldNamesList != null);
                Expect(!debug.Header.FieldNamesList.Any());
                Expect(!debug.Header.IsCIF21);
                Expect(debug.Header.IsCIF30);
                Expect(!debug.Header.IsFull);
                Expect(!debug.Header.IsIncremental);
                Expect(debug.Header.ItemCount == null);
                Expect(debug.Header.LoadMode == null);
                Expect(debug.Header.SupplierIDDomain == null);
                Expect(debug.Header.Timestamp == null);
                Expect(debug.Header.TimestampDate == null);
                Encoding encoding = null;
                Expect(!debug.Header.TryGetEncoding(out encoding));
                Expect(encoding == null);
                Expect(debug.Header.UNUoM == null);
                Expect(debug.Header.Version == "CIF_I_V3.0");

                // Items
                Expect(debug.ItemCalls == 4);
                Expect(debug.Items[0].SupplierID == "942888711");
                Expect(debug.Items[0].SupplierPartID == "100");
                Expect(debug.Items[0].ManufacturerPartID == null);
                Expect(debug.Items[0].ItemDescription == "Blue Ballpoint Pen");
                Expect(debug.Items[0].SPSCCode == "1213376");
                Expect(debug.Items[0].UnitPrice == 1.95m);
                Expect(debug.Items[0].UnitsOfMeasure == "EA");
                Expect(debug.Items[0].LeadTime == null);
                Expect(debug.Items[0].ManufacturerName == null);
                Expect(debug.Items[0].SupplierURL == null);
                Expect(debug.Items[0].ManufacturerURL == null);
                Expect(debug.Items[0].MarketPrice == null);

                Expect(debug.Items[1].SupplierID == "942888711");
                Expect(debug.Items[1].SupplierPartID == "101");
                Expect(debug.Items[1].ManufacturerPartID == null);
                Expect(debug.Items[1].ItemDescription == "No. 2 Pencil");
                Expect(debug.Items[1].SPSCCode == "1213377");
                Expect(debug.Items[1].UnitPrice == 1.5m);
                Expect(debug.Items[1].UnitsOfMeasure == "DZN");
                Expect(debug.Items[1].LeadTime == null);
                Expect(debug.Items[1].ManufacturerName == null);
                Expect(debug.Items[1].SupplierURL == null);
                Expect(debug.Items[1].ManufacturerURL == null);
                Expect(debug.Items[1].MarketPrice == null);

                Expect(debug.Items[2].SupplierID == "942888711");
                Expect(debug.Items[2].SupplierPartID == "102");
                Expect(debug.Items[2].ManufacturerPartID == null);
                Expect(debug.Items[2].ItemDescription == "Rubber Eraser");
                Expect(debug.Items[2].SPSCCode == "1213472");
                Expect(debug.Items[2].UnitPrice == 0.25m);
                Expect(debug.Items[2].UnitsOfMeasure == "PK");
                Expect(debug.Items[2].LeadTime == null);
                Expect(debug.Items[2].ManufacturerName == null);
                Expect(debug.Items[2].SupplierURL == null);
                Expect(debug.Items[2].ManufacturerURL == null);
                Expect(debug.Items[2].MarketPrice == null);

                Expect(debug.Items[3].SupplierID == "942888711");
                Expect(debug.Items[3].SupplierPartID == "103");
                Expect(debug.Items[3].ManufacturerPartID == null);
                Expect(debug.Items[3].ItemDescription == "Stapler, Standard");
                Expect(debug.Items[3].SPSCCode == "1237461");
                Expect(debug.Items[3].UnitPrice == 2.95m);
                Expect(debug.Items[3].UnitsOfMeasure == "BX");
                Expect(debug.Items[3].LeadTime == null);
                Expect(debug.Items[3].ManufacturerName == null);
                Expect(debug.Items[3].SupplierURL == null);
                Expect(debug.Items[3].ManufacturerURL == null);
                Expect(debug.Items[3].MarketPrice == null);

                // Trailer
                Expect(debug.TrailerCalls == 0);
                Expect(debug.Trailer == null);
            }
        }

        [Test]
        public void ShouldReadMultinationalExample()
        {
            using (var stream = OpenStreamReader("MultinationalExample.cif"))
            {
                var reader = new CIFReader(stream);
                var debug = new DebuggingCIFReader();

                reader.Read(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.Header != null);
                Expect(debug.Header.Charset == "8859_1");
                Expect(debug.Header.CodeFormat == "UNSPSC_V7.0");
                Expect(debug.Header.Comments == null);
                Expect(debug.Header.Currency == null);
                Expect(debug.Header.DUNS == null);
                Expect(debug.Header.FieldNames == "Supplier ID, Supplier Part ID, Manufacturer Part ID, Item Description, SPSC Code, Unit Price, Unit of Measure, Lead Time, Manufacturer Name, Supplier URL, Manufacturer URL, Market Price,  Language, Currency, Supplier Part Auxiliary ID");
                Expect(debug.Header.FieldNamesList != null);
                Expect(debug.Header.FieldNamesList.Count == 15);
                Expect(debug.Header.FieldNamesList[0] == "Supplier ID");
                Expect(debug.Header.FieldNamesList[1] == "Supplier Part ID");
                Expect(debug.Header.FieldNamesList[2] == "Manufacturer Part ID");
                Expect(debug.Header.FieldNamesList[14] == "Supplier Part Auxiliary ID");
                Expect(!debug.Header.IsCIF21);
                Expect(debug.Header.IsCIF30);
                Expect(debug.Header.IsFull);
                Expect(!debug.Header.IsIncremental);
                Expect(debug.Header.ItemCount == 3);
                Expect(debug.Header.LoadMode == 'F');
                Expect(debug.Header.SupplierIDDomain == "DUNS");
                Expect(debug.Header.Timestamp == "2000-05-15 15:25:04");
                Expect(debug.Header.TimestampDate == new DateTime(2000, 05, 15, 15, 25, 04));
                Encoding encoding = null;
                Expect(debug.Header.TryGetEncoding(out encoding));
                Expect(encoding.BodyName == "iso-8859-1");
                Expect(debug.Header.UNUoM == null);
                Expect(debug.Header.Version == "CIF_I_V3.0");

                // Items
                Expect(debug.ItemCalls == 3);
                Expect(debug.Items[0].SupplierID == "6565");
                Expect(debug.Items[0].SupplierPartID == "2B");
                Expect(debug.Items[0].ManufacturerPartID == "2B");
                Expect(debug.Items[0].ItemDescription == "Men's black shoes");
                Expect(debug.Items[0].SPSCCode == "53111601");
                Expect(debug.Items[0].UnitPrice == 54.95m);
                // watch out: "Unit of Measure" in example, "Units of Measure" in spec.
                Expect(debug.Items[0]["Unit of Measure"] == "PR");
                Expect(debug.Items[0].UnitsOfMeasure == null);
                Expect(debug.Items[0].LeadTime == 2);
                Expect(debug.Items[0].ManufacturerName == null);
                Expect(debug.Items[0].SupplierURL == null);
                Expect(debug.Items[0].ManufacturerURL == null);
                Expect(debug.Items[0].MarketPrice == null);
                Expect(debug.Items[0].PunchOutEnabled == null);
                Expect(debug.Items[0]["Language"] == "en_US ");
                Expect(debug.Items[0].Language == "en_US ");
                Expect(debug.Items[0]["Currency"] == "USD ");
                Expect(debug.Items[0].Currency == "USD ");
                Expect(debug.Items[0]["Supplier Part Auxiliary ID"] == "en_US");
                Expect(debug.Items[0].SupplierPartAuxiliaryID == "en_US");

                Expect(debug.Items[1].SupplierID == "6565");
                Expect(debug.Items[1].SupplierPartID == "2B");
                Expect(debug.Items[1].ManufacturerPartID == "2B");
                Expect(debug.Items[1].ItemDescription == "Chaussures noires des hommes");
                Expect(debug.Items[1].SPSCCode == "53111601");
                Expect(debug.Items[1].UnitPrice == 119.95m);
                // watch out: "Unit of Measure" in example, "Units of Measure" in spec.
                Expect(debug.Items[1]["Unit of Measure"] == "PR");
                Expect(debug.Items[1].UnitsOfMeasure == null);
                Expect(debug.Items[1].LeadTime == 2);
                Expect(debug.Items[1].ManufacturerName == null);
                Expect(debug.Items[1].SupplierURL == null);
                Expect(debug.Items[1].ManufacturerURL == null);
                Expect(debug.Items[1].MarketPrice == null);
                Expect(debug.Items[1].PunchOutEnabled == null);
                Expect(debug.Items[1]["Language"] == "fr_FR ");
                Expect(debug.Items[1].Language == "fr_FR ");
                Expect(debug.Items[1]["Currency"] == "FRF ");
                Expect(debug.Items[1].Currency == "FRF ");
                Expect(debug.Items[1]["Supplier Part Auxiliary ID"] == "fr_FR");
                Expect(debug.Items[1].SupplierPartAuxiliaryID == "fr_FR");

                Expect(debug.Items[2].SupplierID == "6565");
                Expect(debug.Items[2].SupplierPartID == "2B");
                Expect(debug.Items[2].ManufacturerPartID == "2B");
                Expect(debug.Items[2].ItemDescription == "Herrenschuhe schwarz");
                Expect(debug.Items[2].SPSCCode == "53111601");
                Expect(debug.Items[2].UnitPrice == 34.95m);
                // watch out: "Unit of Measure" in example, "Units of Measure" in spec.
                Expect(debug.Items[2]["Unit of Measure"] == "PR");
                Expect(debug.Items[2].UnitsOfMeasure == null);
                Expect(debug.Items[2].LeadTime == 2);
                Expect(debug.Items[2].ManufacturerName == null);
                Expect(debug.Items[2].SupplierURL == null);
                Expect(debug.Items[2].ManufacturerURL == null);
                Expect(debug.Items[2].MarketPrice == null);
                Expect(debug.Items[2].PunchOutEnabled == null);
                Expect(debug.Items[2]["Language"] == "de_DE ");
                Expect(debug.Items[2].Language == "de_DE ");
                Expect(debug.Items[2]["Currency"] == "DEM ");
                Expect(debug.Items[2].Currency == "DEM ");
                Expect(debug.Items[2]["Supplier Part Auxiliary ID"] == "de_DE");
                Expect(debug.Items[2].SupplierPartAuxiliaryID == "de_DE");

                // Trailer
                Expect(debug.TrailerCalls == 0);
                Expect(debug.Trailer == null);
            }
        }

        [Test]
        public void ShouldReadParametricDataExample()
        {
            using (var stream = OpenStreamReader("ParametricDataExample.cif"))
            {
                var reader = new CIFReader(stream);
                var debug = new DebuggingCIFReader();

                reader.Read(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.Header != null);
                Expect(debug.Header.Charset == "8859_1");
                Expect(debug.Header.CodeFormat == "UNSPSC");
                Expect(debug.Header.Comments == "Sample CIF 3.0 catalog");
                Expect(debug.Header.Currency == "USD");
                Expect(debug.Header.DUNS == null);
                Expect(debug.Header.FieldNames == "Supplier ID, Supplier Part ID, Manufacturer Part ID, Item Description, SPSC Code, Unit Price, Unit of Measure, Lead Time, Manufacturer Name, Supplier URL, Manufacturer URL, Market Price, Short Name, Currency, Expiration Date, Effective Date, Classification Codes, Parametric Data, Parametric Name");
                Expect(debug.Header.FieldNamesList != null);
                Expect(debug.Header.FieldNamesList.Count == 19);
                Expect(debug.Header.FieldNamesList[0] == "Supplier ID");
                Expect(debug.Header.FieldNamesList[1] == "Supplier Part ID");
                Expect(debug.Header.FieldNamesList[2] == "Manufacturer Part ID");
                Expect(!debug.Header.IsCIF21);
                Expect(debug.Header.IsCIF30);
                Expect(debug.Header.IsFull);
                Expect(!debug.Header.IsIncremental);
                Expect(debug.Header.ItemCount == 2);
                Expect(debug.Header.LoadMode == 'F');
                Expect(debug.Header.SupplierIDDomain == "DUNS");
                Expect(debug.Header.Timestamp == "2000-01-18 00:00:00");
                Expect(debug.Header.TimestampDate == new DateTime(2000, 1, 18, 0, 0, 0));
                Encoding encoding = null;
                Expect(debug.Header.TryGetEncoding(out encoding));
                Expect(encoding.BodyName == "iso-8859-1");
                Expect(debug.Header.UNUoM == null);
                Expect(debug.Header.Version == "CIF_I_V3.0");

                // Items
                Expect(debug.ItemCalls == 2);
                Expect(debug.Items[0].SupplierID == "942888710");
                Expect(debug.Items[0].SupplierPartID == "SUX-33");
                Expect(debug.Items[0].ManufacturerPartID == "286-33");
                Expect(debug.Items[0].ItemDescription == "Super Duper 33-MHz 286, 4MB RAM, 10MB, 14”” VGA color monitor");
                Expect(debug.Items[0].SPSCCode == "43171803");
                Expect(debug.Items[0].UnitPrice == 1259.59m);
                // watch out: it's "Unit of Measure" in the example
                Expect(debug.Items[0]["Unit of Measure"] == "EA");
                Expect(debug.Items[0].UnitsOfMeasure == null);
                Expect(debug.Items[0].LeadTime == 3);
                Expect(debug.Items[0].ManufacturerName == "Super Duper Computer");
                Expect(debug.Items[0].SupplierURL == "http://www.slowcomp.com/products/indepth33.htm");
                Expect(debug.Items[0].ManufacturerURL == null);
                Expect(debug.Items[0].MarketPrice == 1599.49m);
                Expect(debug.Items[0]["Short Name"] == "Desktop Computer");
                Expect(debug.Items[0]["Currency"] == "USD");
                Expect(debug.Items[0].Currency == "USD");
                Expect(debug.Items[0]["Expiration Date"] == "2000-12-01");
                Expect(debug.Items[0].ExpirationDate == new DateTime(2000, 12, 01));
                Expect(debug.Items[0]["Effective Date"] == "2000-03-01");
                Expect(debug.Items[0].EffectiveDate == new DateTime(2000, 03, 01));
                Expect(debug.Items[0]["Classification Codes"] == " {ACME=8BE100}");
                Expect(debug.Items[0].ClassificationCodes != null);
                Expect(debug.Items[0].ClassificationCodes["ACME"] == "8BE100");
                Expect(debug.Items[0]["Parametric Data"] == "{TYPE=\"IBM PC\";SPEED=\"33 MHZ\";}");
                Expect(debug.Items[0].ParametricData != null);
                Expect(debug.Items[0].ParametricData["TYPE"] == "IBM PC");
                Expect(debug.Items[0].ParametricData["SPEED"] == "33 MHZ");
                Expect(debug.Items[0]["Parametric Name"] == "COMPUTERS");
                Expect(debug.Items[0].ParametricName == "COMPUTERS");

                Expect(debug.Items[1].SupplierID == "942888710");
                Expect(debug.Items[1].SupplierPartID == "SUX-66");
                Expect(debug.Items[1].ManufacturerPartID == "286-66");
                Expect(debug.Items[1].ItemDescription == "Super Duper 66-MHz 286, 8MB RAM, 20MB, 17”” VGA color monitor");
                Expect(debug.Items[1].SPSCCode == "5045990402");
                Expect(debug.Items[1].UnitPrice == 1699.99m);
                // watch out: it's "Unit of Measure" in the example
                Expect(debug.Items[1]["Unit of Measure"] == "EA");
                Expect(debug.Items[1].UnitsOfMeasure == null);
                Expect(debug.Items[1].LeadTime == 4);
                Expect(debug.Items[1].ManufacturerName == "Super Duper Computer");
                Expect(debug.Items[1].SupplierURL == "http://www.slowcomp.com/products/indepth66.htm");
                Expect(debug.Items[1].ManufacturerURL == null);
                Expect(debug.Items[1].MarketPrice == 1999.49m);
                Expect(debug.Items[1]["Short Name"] == "Desktop Computer");
                Expect(debug.Items[1]["Currency"] == "USD");
                Expect(debug.Items[1].Currency == "USD");
                Expect(debug.Items[1]["Expiration Date"] == "2000-12-01");
                Expect(debug.Items[1].ExpirationDate == new DateTime(2000, 12, 01));
                Expect(debug.Items[1]["Effective Date"] == "2000-03-01");
                Expect(debug.Items[1].EffectiveDate == new DateTime(2000, 03, 01));
                Expect(debug.Items[1]["Classification Codes"] == "{ACME=8BE101}");
                Expect(debug.Items[1].ClassificationCodes != null);
                Expect(debug.Items[1].ClassificationCodes["ACME"] == "8BE101");
                Expect(debug.Items[1]["Parametric Data"] == "{TYPE=\"IBM PC\";SPEED=\"66 MHZ\";}");
                Expect(debug.Items[1].ParametricData != null);
                Expect(debug.Items[1].ParametricData["TYPE"] == "IBM PC");
                Expect(debug.Items[1].ParametricData["SPEED"] == "66 MHZ");
                Expect(debug.Items[1]["Parametric Name"] == "COMPUTERS");
                Expect(debug.Items[1].ParametricName == "COMPUTERS");

                // Trailer
                Expect(debug.TrailerCalls == 0);
                Expect(debug.Trailer == null);
            }
        }

        [Test]
        public void ShouldReadPunchOutCatalogExample()
        {
            using (var stream = OpenStreamReader("PunchOutCatalogExample.cif"))
            {
                var reader = new CIFReader(stream);
                var debug = new DebuggingCIFReader();

                reader.Read(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.Header != null);
                Expect(debug.Header.Charset == null);
                Expect(debug.Header.CodeFormat == "UNSPSC");
                Expect(debug.Header.Comments == "This is an example of an PunchOut catalog item");
                Expect(debug.Header.Currency == "USD");
                Expect(debug.Header.DUNS == null);
                Expect(debug.Header.FieldNames == "Supplier ID, Supplier Part ID, Manufacturer Part ID, Item Description, SPSC Code, Unit Price, Unit of Measure, Lead Time, Manufacturer Name, Supplier URL, Manufacturer URL, Market Price,  PunchOut Enabled");
                Expect(debug.Header.FieldNamesList != null);
                Expect(debug.Header.FieldNamesList.Count == 13);
                Expect(debug.Header.FieldNamesList[0] == "Supplier ID");
                Expect(debug.Header.FieldNamesList[1] == "Supplier Part ID");
                Expect(debug.Header.FieldNamesList[2] == "Manufacturer Part ID");
                Expect(debug.Header.FieldNamesList[12] == "PunchOut Enabled");
                Expect(!debug.Header.IsCIF21);
                Expect(debug.Header.IsCIF30);
                Expect(debug.Header.IsFull);
                Expect(!debug.Header.IsIncremental);
                Expect(debug.Header.ItemCount == null);
                Expect(debug.Header.LoadMode == 'F');
                Expect(debug.Header.SupplierIDDomain == null);
                Expect(debug.Header.Timestamp == null);
                Expect(debug.Header.TimestampDate == null);
                Encoding encoding = null;
                Expect(!debug.Header.TryGetEncoding(out encoding));
                Expect(encoding == null);
                Expect(debug.Header.UNUoM == null);
                Expect(debug.Header.Version == "CIF_I_V3.0");

                // Items
                Expect(debug.ItemCalls == 1);
                Expect(debug.Items[0].SupplierID == "762311901");
                Expect(debug.Items[0].SupplierPartID == "A2C-311F");
                Expect(debug.Items[0].ManufacturerPartID == "C-311F");
                Expect(debug.Items[0].ItemDescription == "Configurable Chairs from Work Chairs, Inc.");
                Expect(debug.Items[0].SPSCCode == "11116767");
                Expect(debug.Items[0].UnitPrice == null);
                Expect(debug.Items[0].UnitsOfMeasure == null);
                Expect(debug.Items[0].LeadTime == null);
                Expect(debug.Items[0].ManufacturerName == null);
                Expect(debug.Items[0].SupplierURL == "https://www.workchairs.com/configurator.asp");
                Expect(debug.Items[0].ManufacturerURL == null);
                Expect(debug.Items[0].MarketPrice == null);
                Expect(debug.Items[0].PunchOutEnabled == true);

                // Trailer
                Expect(debug.TrailerCalls == 0);
                Expect(debug.Trailer == null);
            }
        }

        [Test]
        public void ShouldRead12RequiredFields()
        {
            using (var stream = OpenStreamReader("Using12RequiredFields.cif"))
            {
                var reader = new CIFReader(stream);
                var debug = new DebuggingCIFReader();

                reader.Read(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.Header != null);
                Expect(debug.Header.Charset == null);
                Expect(debug.Header.CodeFormat == "UNSPSC");
                Expect(debug.Header.Comments == null);
                Expect(debug.Header.Currency == "USD");
                Expect(debug.Header.DUNS == null);
                Expect(debug.Header.FieldNames == null);
                Expect(debug.Header.FieldNamesList != null);
                Expect(!debug.Header.FieldNamesList.Any());
                Expect(!debug.Header.IsCIF21);
                Expect(debug.Header.IsCIF30);
                Expect(debug.Header.IsFull);
                Expect(!debug.Header.IsIncremental);
                Expect(debug.Header.ItemCount == 3);
                Expect(debug.Header.LoadMode == 'F');
                Expect(debug.Header.SupplierIDDomain == "DUNS");
                Expect(debug.Header.Timestamp == "2000-05-15 15:25:04");
                Expect(debug.Header.TimestampDate == new DateTime(2000, 5, 15, 15, 25, 04));
                Encoding encoding = null;
                Expect(!debug.Header.TryGetEncoding(out encoding));
                Expect(encoding == null);
                Expect(debug.Header.UNUoM == null);
                Expect(debug.Header.Version == "CIF_I_V3.0");

                // Items
                Expect(debug.ItemCalls == 3);
                Expect(debug.Items[0].SupplierID == "942888710");
                Expect(debug.Items[0].SupplierPartID == "34A11");
                Expect(debug.Items[0].ManufacturerPartID == "C11");
                Expect(debug.Items[0].ItemDescription == "Eames Chair, Black Leather");
                Expect(debug.Items[0].SPSCCode == "11116767");
                Expect(debug.Items[0].UnitPrice == 400.00m);
                Expect(debug.Items[0].UnitsOfMeasure == "EA");
                Expect(debug.Items[0].LeadTime == 3);
                Expect(debug.Items[0].ManufacturerName == "Fast MFG");
                Expect(debug.Items[0].SupplierURL == "http://www.acme.com/34A11.htm");
                Expect(debug.Items[0].ManufacturerURL == "http://www.mfg.com/C11/indepth.htm");
                Expect(debug.Items[0].MarketPrice == 400.00m);

                Expect(debug.Items[1].SupplierID == "942888710");
                Expect(debug.Items[1].SupplierPartID == "56A12");
                Expect(debug.Items[1].ManufacturerPartID == "C12");
                Expect(debug.Items[1].ItemDescription == "Eames Ottoman, Blk Leather");
                Expect(debug.Items[1].SPSCCode == "11116767");
                Expect(debug.Items[1].UnitPrice == 100m);
                Expect(debug.Items[1].UnitsOfMeasure == "EA");
                Expect(debug.Items[1].LeadTime == 3);
                Expect(debug.Items[1].ManufacturerName == "Fast MFG");
                Expect(debug.Items[1].SupplierURL == "http://www.acme.com/56A12.htm");
                Expect(debug.Items[1].ManufacturerURL == "http://www.mfg.com/C12/indepth.htm");
                Expect(debug.Items[1].MarketPrice == 100m);

                Expect(debug.Items[2].SupplierID == "942888710");
                Expect(debug.Items[2].SupplierPartID == "78A13");
                Expect(debug.Items[2].ManufacturerPartID == "C13");
                Expect(debug.Items[2].ItemDescription == "Folding Chair, Grey Stackable");
                Expect(debug.Items[2].SPSCCode == "11116767");
                Expect(debug.Items[2].UnitPrice == 25.95m);
                Expect(debug.Items[2].UnitsOfMeasure == "EA");
                Expect(debug.Items[2].LeadTime == 3);
                Expect(debug.Items[2].ManufacturerName == "Fast MFG");
                Expect(debug.Items[2].SupplierURL == "http://www.acme.com/78A13.htm");
                Expect(debug.Items[2].ManufacturerURL == "http://www.mfg.com/C13/indepth.htm");
                Expect(debug.Items[2].MarketPrice == 25.95m);

                // Trailer
                Expect(debug.TrailerCalls == 0);
                Expect(debug.Trailer == null);
            }
        }
    }
}

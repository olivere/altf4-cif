using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class ItemTests : BaseTestFixture
    {
        [Test]
        public void SettingFieldsByPropertyShouldFillInternalDict()
        {
            var item = new CIFItem();
            Expect(item["Supplier ID"] == null);
            item.SupplierID = "ID";
            Expect(item["Supplier ID"] == "ID");
        }

        [Test]
        public void ValidationIsEvaluatedWhenCallingIsValid()
        {
            var item = new CIFItem();
            Expect(item.IsValid == false);
            Expect(item.Errors != null);
            Expect(item.Errors.Length > 0);
        }

        [Test]
        public void ValidationIsEvaluatedWhenRetrievingErrors()
        {
            var item = new CIFItem();
            Expect(item.Errors != null);
            Expect(item.Errors.Length > 0);
        }

        [Test]
        public void ValidationErrorsOccurOnNullOnly()
        {
            // we allow items to be empty, but not null
            var item = new CIFItem();
            Expect(item.IsValid == false); // triggers validation and sets errors
            Expect(item.Errors != null);
            Expect(item.Errors.Any(e => Regex.IsMatch(e, "Supplier ID")));
            item.SupplierID = "";
            Expect(item.IsValid == false); // triggers validation and sets errors
            Expect(item.Errors.Any(e => !Regex.IsMatch(e, "Supplier ID")));
        }

        [Test]
        public void ShouldNotValidateUnlessAllRequiredFieldsAreSet()
        {
            var item = new CIFItem();
            Expect(item.IsValid == false);
            Expect(item.Errors.Length > 0);
            Expect(item.Errors.Any(e => Regex.IsMatch(e, "Supplier ID")));
            item["Supplier ID"] = "ALT-F4";
            item["Supplier Part ID"] = "12345";
            Expect(item.IsValid == false);
            Expect(item.Errors.Length > 0);
            Expect(item.Errors.Any(e => !Regex.IsMatch(e, "Supplier ID")));
            item.ManufacturerPartID = "ManuPartID";
            item.ItemDescription = "Item description";
            item.SPSCCode = "SPSC";
            item.UnitPrice = 1.23m;
            item.UnitsOfMeasure = "EA";
            item.LeadTime = 7;
            item.ManufacturerName = "Meplato";
            item.SupplierURL = "http://supplysite.net/";
            item.ManufacturerURL = "http://producer.net/";
            item.MarketPrice = 1.50m;
            Expect(item.IsValid == true);
            Expect(item.Errors.Length == 0);
        }

        [Test]
        public void ShouldCorrectlyHandlePunchOutEnabledValues()
        {
            var item = new CIFItem();
            var cases = new Dictionary<string, bool?>()
                            {
                                {"True", true},
                                {"TRUE", true},
                                {"true", true},
                                {"t", true},
                                {"T", true},
                                {"1", true},

                                {"False", false},
                                {"FALSE", false},
                                {"false", false},
                                {"f", false},
                                {"F", false},
                                {"0", false},

                                {"", null},
                                {" ", null},
                                {"fuzzy", null},
                                {" true", null},
                                {" false", null},
                                {"true ", null},
                                {"false ", null},
                                {"X", null},
                                {"Y", null},
                                {"N", null},
                            };

            foreach(var c in cases)
            {
                item[Constants.PunchOutEnabled] = c.Key;
                Expect(item.PunchOutEnabled == c.Value, "{0} is not {1}", c.Key, c.Value);
            }

            // Check null
            item[Constants.PunchOutEnabled] = null;
            Expect(item.PunchOutEnabled == null);
        }

        // Not sure about this
        /*
        [Test]
        public void UnitOfMeasureIsAnAliasToUnitsOfMeasure()
        {
            var item = new CIFItem();
            item["Unit of Masure"] = "EA";
            Expect(item.UnitOfMeasure == "EA");

            item = new CIFItem();
            item["Units of Masure"] = "EA";
            Expect(item.UnitOfMeasure == "EA");

            item = new CIFItem();
            item.UnitOfMeasure = "EA";
            Expect(item["Unit of Measure"] == "EA");
            Expect(item["Units of Measure"] == null);
        }
        */

        [Test]
        public void ShouldSerializeCIFHash()
        {
            Expect("" == CIFItem.ToHash(null));
            Expect("{}" == CIFItem.ToHash(new Dictionary<string, string>()));

            var dict = new Dictionary<string, string>()
                           {{"TYPE", "SEAMLESS"}, {"SIZE", "20"}, {"WEIGHT PER FOOT", "94.00"}};
            string result = CIFItem.ToHash(dict);
            Expect("{TYPE=SEAMLESS;SIZE=20;WEIGHT PER FOOT=94.00}" == result);
        }

        [Test]
        public void ShouldSerializeGivenFieldsToCSV()
        {
            var item = new CIFItem();
            // required fields
            item.SupplierID = "ALT-F4";
            item.SupplierPartID = "12345";
            item.ManufacturerPartID = "ManuPartID";
            item.ItemDescription = "3.5\" disc";
            item.SPSCCode = "SPSC";
            item.UnitPrice = 1230.00m;
            item.UnitsOfMeasure = "EA";
            item.LeadTime = 7;
            item.ManufacturerName = "Meplato";
            item.SupplierURL = "http://supplysite.net/";
            item.ManufacturerURL = "http://producer.net/";
            item.MarketPrice = 1500.12m;
            // optional fields
            item.Tier = "1";
            item.Name = "Name";
            item.Language = "de_DE";
            item.Currency = "EUR";
            item.ExpirationDate = new DateTime(2010, 1, 13, 23, 01, 02);
            item.EffectiveDate = new DateTime(2011, 2, 14, 22, 02, 04);
            item.ClassificationCodes = new Dictionary<string, string>() {{"ACME", "8BE100" }};
            item.ParametricData = new Dictionary<string, string>()
                                      {{"TYPE", "SEAMLESS"}, {"SIZE", "20"}, {"WEIGHT PER FOOT", "94.00"}};
            item.ParametricName = "PIPES";
            item.PunchOutEnabled = true;
            item.TerritoryAvailable = "US,GB,IT";
            item.SupplierPartAuxiliaryID = "red";
            item.Delete = true;
            // custom fields
            item["Mall Groups"] = "A,B,C";
            
            // validate
            Expect(item.IsValid == true);
            Expect(item.Errors.Length == 0);

            // Generate CSV
            Expect("" == item.ToCSV(null));
            Expect("" == item.ToCSV(new string[0]));

            // required
            string result = item.ToCSV(Constants.RequiredFields);
            Expect("ALT-F4,12345,ManuPartID,3.5\" disc,SPSC,1230.00,EA,7,Meplato,http://supplysite.net/,http://producer.net/,1500.12" == result);
            
            // required + optional
            string[] requiredPlusOptional = Constants.RequiredFields.Concat(Constants.OptionalFields).ToArray();
            result = item.ToCSV(requiredPlusOptional);
            Expect(
                "ALT-F4,12345,ManuPartID,3.5\" disc,SPSC,1230.00,EA,7,Meplato,http://supplysite.net/,http://producer.net/,1500.12,1,Name,de_DE,EUR,2010-01-13,2011-02-14,{ACME=8BE100},{TYPE=SEAMLESS;SIZE=20;WEIGHT PER FOOT=94.00},PIPES,True,\"US,GB,IT\",red,True" ==
                result);

            // required + optional + custom
            string[] requiredPlusOptionalPlusCustom = Constants.RequiredFields.Concat(Constants.OptionalFields).Concat(new[] {"Mall Groups"}).ToArray();
            result = item.ToCSV(requiredPlusOptionalPlusCustom);
            Expect(
                "ALT-F4,12345,ManuPartID,3.5\" disc,SPSC,1230.00,EA,7,Meplato,http://supplysite.net/,http://producer.net/,1500.12,1,Name,de_DE,EUR,2010-01-13,2011-02-14,{ACME=8BE100},{TYPE=SEAMLESS;SIZE=20;WEIGHT PER FOOT=94.00},PIPES,True,\"US,GB,IT\",red,True,\"A,B,C\"" ==
                result);

            // required + optional + custom + some non-specified
            string[] requiredPlusOptionalPlusCustomPlusMissing = Constants.RequiredFields.Concat(Constants.OptionalFields).Concat(new[] { "Mall Groups", "Not specified", "Another non-specified" }).ToArray();
            result = item.ToCSV(requiredPlusOptionalPlusCustomPlusMissing);
            Expect(
                "ALT-F4,12345,ManuPartID,3.5\" disc,SPSC,1230.00,EA,7,Meplato,http://supplysite.net/,http://producer.net/,1500.12,1,Name,de_DE,EUR,2010-01-13,2011-02-14,{ACME=8BE100},{TYPE=SEAMLESS;SIZE=20;WEIGHT PER FOOT=94.00},PIPES,True,\"US,GB,IT\",red,True,\"A,B,C\",," ==
                result);
        }
    }
}

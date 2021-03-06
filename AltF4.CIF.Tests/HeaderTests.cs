﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class HeaderTests : BaseTestFixture
    {
        [Test]
        public void HeaderFieldsAreCaseInsensitive()
        {
            string validHeader = ReadAllText("ValidCaseInsensitiveHeader.txt");
            CIFHeader header = null;
            Expect(() => header = CIFHeader.Parse(validHeader), Throws.Nothing);
            Expect("USD" == header.Currency);
            Expect("8859_1" == header.Charset);
            Expect("UNSPSC" == header.CodeFormat);
        }

        [Test]
        public void HeaderFieldsShouldBeTrimmed()
        {
            string validHeader = ReadAllText("ValidCaseInsensitiveHeader.txt");
            CIFHeader header = null;
            Expect(() => header = CIFHeader.Parse(validHeader), Throws.Nothing);
            Expect('F' == header.LoadMode);
            Expect("DUNS" == header.SupplierIDDomain);
            Expect("Supplier ID" == header.FieldNamesList.First());
        }

        [Test]
        public void ShouldParseHeader()
        {
            string validHeader = ReadAllText("ValidHeader.txt");
            CIFHeader header = null;
            Expect(() => header = CIFHeader.Parse(validHeader), Throws.Nothing);
            Expect("CIF_I_V3.0" == header.Version);
            Expect("USD" == header.Currency);
            Expect("8859_1" == header.Charset);
            Expect("UNSPSC" == header.CodeFormat);
            Expect(4 == header.ItemCount);
            Expect('F' == header.LoadMode);
            Expect("DUNS" == header.SupplierIDDomain);
            Expect("2002-3-12 15:19:55" == header.Timestamp);
            Expect(true == header.UNUoM);
            Expect("This Catalog contains Parametrics" == header.Comments);
            Expect(header.FieldNames, Is.Not.Null.And.Not.Empty);
            Expect(header.FieldNames == "Supplier ID,Supplier Part ID,Manufacturer Part ID,Item Description,SPSC Code,Unit Price,Unit of Measure,Lead Time,Manufacturer Name,Supplier URL,Manufacturer URL,Market Price,Currency,Short Name,Language,Expiration Date,Classification Codes,Parametric Data,Parametric Name,Territory Available,Supplier Part Auxiliary ID,Delete,PunchOut Enabled");
            Expect(23 == header.FieldNamesList.Count());
            Expect(header.FieldNames.Contains("Supplier ID"));
            Expect(header.FieldNames.Contains("Supplier Part ID"));
            Expect(header.FieldNames.Contains("Manufacturer Part ID"));
            Expect(header.FieldNames.Contains("Item Description"));
            Expect(header.FieldNames.Contains("SPSC Code"));
            Expect(header.FieldNames.Contains("Unit Price"));
            Expect(header.FieldNames.Contains("Unit of Measure"));
            Expect(header.FieldNames.Contains("Lead Time"));
            Expect(header.FieldNames.Contains("Manufacturer Name"));
            Expect(header.FieldNames.Contains("Supplier URL"));
            Expect(header.FieldNames.Contains("Manufacturer URL"));
            Expect(header.FieldNames.Contains("Market Price"));
            Expect(header.FieldNames.Contains("Currency"));
            Expect(header.FieldNames.Contains("Short Name"));
            Expect(header.FieldNames.Contains("Language"));
            Expect(header.FieldNames.Contains("Expiration Date"));
            Expect(header.FieldNames.Contains("Classification Codes"));
            Expect(header.FieldNames.Contains("Parametric Data"));
            Expect(header.FieldNames.Contains("Parametric Name"));
            Expect(header.FieldNames.Contains("Territory Available"));
            Expect(header.FieldNames.Contains("Supplier Part Auxiliary ID"));
            Expect(header.FieldNames.Contains("Delete"));
            Expect(header.FieldNames.Contains("PunchOut Enabled"));
        }
    }
}

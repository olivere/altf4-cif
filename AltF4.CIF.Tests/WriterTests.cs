using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class WriterTests : BaseTestFixture
    {
        [Test]
        public void ShouldWriteCIFFile()
        {
            using (var stream = OpenStreamWriter("Output-Test.cif"))
            {
                var writer = new CIFWriter(stream);
                var debug = new DebuggingCIFWriter();

                var items = new List<CIFItem>();
                var item = new CIFItem();
                item["Supplier ID"] = "ALT-F4";
                item["Supplier Part ID"] = "12345";
                // TODO fill all standard fields
                items.Add(item);
                // TODO add more
                debug.Items = items;

                debug.Header = new CIFHeader();
                debug.Header.Charset = "UTF-8";
                debug.Header.CodeFormat = "UNSPSC";
                debug.Header.Comments = "This is a comment";
                debug.Header.Currency = "EUR";
                debug.Header.DUNS = null;
                debug.Header.FieldNames = string.Join(",", Constants.RequiredFields);
                debug.Header.ItemCount = items.Count;
                debug.Header.LoadMode = 'F';
                debug.Header.SupplierIDDomain = "DUNS";
                debug.Header.TimestampDate = DateTime.Now;
                debug.Header.UNUoM = true;

                debug.Trailer = null;

                writer.Write(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.ItemCalls == 1);
                Expect(debug.TrailerCalls == 1);

                // TODO read in and compare back-to-back
            }
        }
    }
}

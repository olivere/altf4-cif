using System;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class ReaderTests : BaseTestFixture
    {
        [Test]
        public void ShouldReadCIFFile()
        {
            using(var stream = OpenStreamReader("Catalog1.cif"))
            {
                var reader = new CIFReader(stream);
                var debug = new DebuggingCIFReader();

                reader.Read(debug);

                Expect(debug.HeaderCalls == 1);
                Expect(debug.Header != null);
                Expect(debug.Header.Version == "CIF_I_V3.0");
                Expect(debug.Header.IsCIF30);
                Expect(debug.Header.Currency == "USD");
                Expect(debug.Header.Charset == "8859_1");
                Encoding encoding = null;
                Expect(debug.Header.TryGetEncoding(out encoding) == true);
                Expect(encoding.BodyName == "iso-8859-1");
                Expect(debug.Header.CodeFormat == "UNSPSC");
                Expect(debug.Header.ItemCount == 4);
                Expect(debug.Header.LoadMode == 'F');
                Expect(debug.Header.IsFull == true);
                Expect(debug.Header.IsIncremental == false);
                Expect(debug.Header.SupplierIDDomain == "DUNS");
                Expect(debug.Header.Timestamp == "2002-3-12 15:19:55");
                Expect(debug.Header.TimestampDate != null);
                Expect(debug.Header.TimestampDate.Value, Is.EqualTo(new DateTime(2002, 3, 12, 15, 19, 55)));
                Expect(debug.Header.UNUoM == true);
                Expect(debug.Header.FieldNames == "Supplier ID,Supplier Part ID,Manufacturer Part ID,Item Description,SPSC Code,Unit Price,Unit of Measure,Lead Time,Manufacturer Name,Supplier URL,Manufacturer URL,Market Price,Currency,Short Name,Language,Expiration Date,Classification Codes,Parametric Data,Parametric Name,Territory Available,Supplier Part Auxiliary ID,Delete,PunchOut Enabled");

                Expect(debug.ItemCalls == 4);
                Expect(debug.Items != null);
                Expect(debug.Items.Count == 4);
                Expect(debug.Items[0]["Supplier ID"] == "195575006");
                Expect(debug.Items[0]["Supplier Part ID"] == "CS2820E");
                // TODO validate other fields
                Expect(debug.Items[1]["Supplier ID"] == "195575006");
                Expect(debug.Items[1]["Supplier Part ID"] == "CS2820EE");
                // TODO validate other fields
                Expect(debug.Items[2]["Supplier ID"] == "195575006");
                Expect(debug.Items[2]["Supplier Part ID"] == "MB2014");
                // TODO validate other fields
                Expect(debug.Items[3]["Supplier ID"] == "195575006");
                Expect(debug.Items[3]["Supplier Part ID"] == "PAD");
            }
        }
    }
}

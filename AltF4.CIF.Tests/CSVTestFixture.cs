using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class CSVTestFixture : BaseTestFixture
    {
        [Test]
        public void ShouldHandleProblematicCSV()
        {
            Expect(0 == CIFItem.ParseCSV(null).Count());
            Expect(0 == CIFItem.ParseCSV("").Count());
        }

        [Test]
        public void ShouldParseSingleFields()
        {
            Expect(CIFItem.ParseCSV(" "), Is.Not.Null.And.Count.EqualTo(1).And.Contains(" "));
            Expect(CIFItem.ParseCSV(" Abc"), Is.Not.Null.And.Count.EqualTo(1).And.Contains(" Abc"));
            Expect(CIFItem.ParseCSV(" Abc "), Is.Not.Null.And.Count.EqualTo(1).And.Contains(" Abc "));
        }

        [Test]
        public void ShouldParseEmptyFields()
        {
            Expect(CIFItem.ParseCSV(","), Is.Not.Null.And.Count.EqualTo(2));
            Expect(CIFItem.ParseCSV(",,,"), Is.Not.Null.And.Count.EqualTo(4));
        }

        [Test]
        public void ShouldParseNormalFields()
        {
            var fields = CIFItem.ParseCSV("First,Second,Third");
            Expect(3 == fields.Count());
            Expect("First" == fields.ElementAt(0));
            Expect("Second" == fields.ElementAt(1));
            Expect("Third" == fields.ElementAt(2));
        }

        [Test]
        public void ShouldNotTrimSpaces()
        {
            var fields = CIFItem.ParseCSV(" First,Second , Third ");
            Expect(3 == fields.Count());
            Expect(" First" == fields.ElementAt(0));
            Expect("Second " == fields.ElementAt(1));
            Expect(" Third " == fields.ElementAt(2));
        }

        [Test]
        public void ShouldParseQuotedFields()
        {
            var fields = CIFItem.ParseCSV("First,\"Second with , comma\",\",Third,,\"");
            Expect(3 == fields.Count());
            Expect("First" == fields.ElementAt(0));
            Expect("Second with , comma" == fields.ElementAt(1));
            Expect(",Third,," == fields.ElementAt(2));
        }

        [Test]
        public void ShouldParseQuoteInField()
        {
            var fields = CIFItem.ParseCSV("First,Disc 3.5\" size,Third");
            Expect(3 == fields.Count());
            Expect("First" == fields.ElementAt(0));
            Expect("Disc 3.5\" size" == fields.ElementAt(1));
            Expect("Third" == fields.ElementAt(2));

            fields = CIFItem.ParseCSV("First,Disc 3.5\",Third");
            Expect(3 == fields.Count());
            Expect("First" == fields.ElementAt(0));
            Expect("Disc 3.5\"" == fields.ElementAt(1));
            Expect("Third" == fields.ElementAt(2));
        }

        [Test]
        public void ShouldParseEdgeCasesWithQuotedFields()
        {
            // No closing quote
            var fields = CIFItem.ParseCSV("First,\"Second");
            Expect(2 == fields.Count());
            Expect("First" == fields.ElementAt(0));
            Expect("Second" == fields.ElementAt(1));

            // Whitespace after closing quote
            fields = CIFItem.ParseCSV("First,\"Second\" ,Third");
            Expect(3 == fields.Count());
            Expect("First" == fields.ElementAt(0));
            Expect("Second" == fields.ElementAt(1));
            Expect("Third" == fields.ElementAt(2));
        }
    }
}

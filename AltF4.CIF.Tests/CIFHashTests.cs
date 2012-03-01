using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    [TestFixture]
    public class CIFHashTestFixture : BaseTestFixture
    {
        [Test]
        public void ShouldHandleProblematicCIFHashes()
        {
            Expect(0 == CIFItem.ParseHash(null).Count);
            Expect(0 == CIFItem.ParseHash("").Count);
            Expect(0 == CIFItem.ParseHash("{}").Count);
        }

        [Test]
        public void ShouldParseSimpleDict()
        {
            var dict = CIFItem.ParseHash("{TYPE=SEAMLESS;SIZE=20;WEIGHT=94.00}");
            Expect(3 == dict.Count);
            Expect(dict.ContainsKey("TYPE"));
            Expect(dict["TYPE"] == "SEAMLESS");
            Expect(dict.ContainsKey("SIZE"));
            Expect(dict["SIZE"] == "20");
            Expect(dict.ContainsKey("WEIGHT"));
            Expect(dict["WEIGHT"] == "94.00");
            Expect(!dict.ContainsKey("type")); // case-sentisive
        }

        [Test]
        public void ShouldParseSimpleDictWithSpacesInKeyName()
        {
            var dict = CIFItem.ParseHash("{HAZARDOUS MATERIAL CODE=3}");
            Expect(1 == dict.Count);
            Expect(dict.ContainsKey("HAZARDOUS MATERIAL CODE"));
            Expect(dict["HAZARDOUS MATERIAL CODE"] == "3");
        }

        [Test]
        public void ShouldNotTrimKeyOrValue()
        {
            // According to the spec, this is invalid
            var dict = CIFItem.ParseHash("{TYPE =SEAMLESS ; SIZE= 20; WEIGHT = 94.00 }");
            Expect(3 == dict.Count);
            Expect(dict.ContainsKey("TYPE "));
            Expect(dict["TYPE "] == "SEAMLESS ");
            Expect(dict.ContainsKey(" SIZE"));
            Expect(dict[" SIZE"] == " 20");
            Expect(dict.ContainsKey(" WEIGHT "));
            Expect(dict[" WEIGHT "] == " 94.00 ");
        }

        [Test]
        public void ShouldParseQuotedValues()
        {
            var dict = CIFItem.ParseHash("{TYPE =\"SEAMLESS \"; SIZE=\" 20\"; WEIGHT =\" 94.00 \"}");
            Expect(3 == dict.Count);
            Expect(dict.ContainsKey("TYPE "));
            Expect(dict["TYPE "] == "SEAMLESS ");
            Expect(dict.ContainsKey(" SIZE"));
            Expect(dict[" SIZE"] == " 20");
            Expect(dict.ContainsKey(" WEIGHT "));
            Expect(dict[" WEIGHT "] == " 94.00 ");
        }

        [Test]
        public void ShouldParseCommaWithoutQuote()
        {
            var dict = CIFItem.ParseHash("{TYPE=ROUND,FLAT}");
            Expect(1 == dict.Count);
            Expect(dict.ContainsKey("TYPE"));
            Expect(dict["TYPE"] == "ROUND,FLAT");
        }

        [Test]
        public void ShouldParseDoubleQuotes()
        {
            var dict = CIFItem.ParseHash("{TYPE=\"\"ROUND,FLAT\"\"}");
            Expect(1 == dict.Count);
            Expect(dict.ContainsKey("TYPE"));
            Expect(dict["TYPE"] == "\"ROUND,FLAT\"");
        }

        [Test]
        public void ShouldParseEscapeChars()
        {
            var dict = CIFItem.ParseHash("{TYPE=\"ROUND,{FLAT}\"}");
            Expect(1 == dict.Count);
            Expect(dict.ContainsKey("TYPE"));
            Expect(dict["TYPE"] == "ROUND,{FLAT}");
        }
    }
}

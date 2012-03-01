using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AltF4.CIF.Tests
{
    public abstract class BaseTestFixture : AssertionHelper
    {
        public string DataDirectory
        {
            get { return Path.Combine(Environment.CurrentDirectory, "Data"); }
        }

        public string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(DataDirectory, file));
        }

        public StreamReader OpenStreamReader(string testFile)
        {
            return new StreamReader(Path.Combine(DataDirectory, testFile));
        }

        public StreamWriter OpenStreamWriter()
        {
            return OpenStreamWriter(Path.GetTempFileName());
        }

        public StreamWriter OpenStreamWriter(string outputFile)
        {
            if (File.Exists(outputFile))
                File.Delete(outputFile);

            return new StreamWriter(Path.Combine(DataDirectory, outputFile));
        }
    }
}

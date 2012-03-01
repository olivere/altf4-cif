using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    /// <summary>
    /// Base class of all exceptions in this assembly.
    /// </summary>
    public class CIFException : Exception
    {
        public CIFException()
        {}

        public CIFException(string message) : base(message)
        {}

        public CIFException(string message, Exception innerException) : base(message, innerException)
        {}
    }
}

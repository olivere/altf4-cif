using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AltF4.CIF
{
    /// <summary>
    /// Defines the protocol CIF writers need to implement in
    /// order to write CIF files.
    /// </summary>
    public interface ICIFWriterProtocol
    {
        /// <summary>
        /// Implementors must returns the complete CIF header,
        /// with the exception of the field names (which will
        /// be determined via <see cref="GetCIFFieldNames"/>.
        /// </summary>
        /// <returns>Reference to <see cref="CIFHeader"/></returns>
        CIFHeader GetCIFHeader();

        /// <summary>
        /// Return the list of all fields to write (in order).
        /// </summary>
        /// <remarks>>
        /// <para>It is the responsibility of the implementor to
        /// return all field names that items might have.</para>
        /// <para>You may include the required CIF field names
        /// according to the CIF spec (<see cref="Constants.RequiredFields"/>). 
        /// If you don't, they'll be added automatically.</para>
        /// </remarks>
        /// <returns>Array of field names</returns>
        /// <seealso cref="Constants.RequiredFields"/>
        /// <seealso cref="Constants.OptionalFields"/>
        string[] GetCIFFieldNames();

        /// <summary>
        /// Implementors need to return an <see cref="IEnumerable"/>
        /// to instances of type <see cref="CIFItem"/>.
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerable<CIFItem> GetCIFItemEnumerator();

        /// <summary>
        /// Implementors need to return a <see cref="CIFTrailer"/>
        /// or <lang>null</lang> here.
        /// </summary>
        /// <remarks>
        /// If implementors return <lang>null</lang>, no trailer
        /// will be written.
        /// </remarks>
        /// <returns>An instance of <see cref="CIFTrailer"/> or
        /// <lang>null</lang></returns>
        CIFTrailer GetCIFTrailer();
    }
}

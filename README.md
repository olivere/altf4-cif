# CIF Tools

Reads and writes Ariba CIF files.

## Prerequisites

* .NET Framework 4.0 (should work with 3.5 with some changes)

## Reading

Reading basically works along these lines:

    using(var streamReader = new StreamReader(...))
	using(var reader = new CIFReader(streamReader))
	{
	   reader.Read(protocol);
	}

where `protocol` is a class that implements the `ICIFReaderProtocol` interface
(you can derive it from the abstract base class `CIFReaderProtocol`). 
It basically defines three methods that will be called during reading:

    public interface ICIFReaderProtocol
	{
	    // Called when header is ready
	    void HandleHeader(CIFHeader header);

		// Called when a new item is ready
		void HandleItem(CIFItem item);

		// Called after trailer is ready
		void HandleTrailer(CIFTrailer trailer);
	}

## Writing

Write CIF files with code like this:

    using(var streamWriter = new StreamWriter(...))
	using(var writer = new CIFWriter(streamWriter))
	{
	  writer.Write(protocol);
	}

where, again, `protocol` implements an interface `ICIFWriterProtocol` (you can,
again, derive from the abstract base class `CIFWriterProtocol`).
Here's `ICIFWriterProtocol`:

    public interface ICIFWriterProtocol
	{
	    // Return header here
		CIFHeader GetCIFHeader();

		// Return enumerator for CIFItem instances here
		IEnumerable<CIFItem> GetCIFItems();

		// Return trailer here
		CIFTrailer GetCIFTrailer();
	}


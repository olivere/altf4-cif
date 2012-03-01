namespace AltF4.CIF
{
    public interface ICIFReaderProtocol
    {
        void HandleHeader(CIFHeader header);

        void HandleItem(CIFItem item);

        void HandleTrailer(CIFTrailer trailer);
    }
}
namespace BufferStream;

public class PcapHeader
{
   /*
    *                      1                   2                   3
       0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    0 |                          Magic Number                         |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    4 |          Major Version        |         Minor Version         |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    8 |                           Reserved1                           |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   12 |                           Reserved2                           |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   16 |                            SnapLen                            |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
   20 | FCS |f|                   LinkType                            |
      +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
    */
   public const UInt16 PcapHeaderLength = 24;
   public UInt32 MagicNumber { get;}
   public UInt16 MajorVersion { get; }
   public UInt16 MinorVersion { get; }
   public UInt32 Reserved1 { get; }
   public UInt32 Reserved2 { get; }
   public UInt32 SnapLen { get; }
   public UInt32 LinkType { get; }

   public PcapHeader(byte[] HeaderData)
   {
      MagicNumber = BitConverter.ToUInt32(HeaderData, 0);
      MajorVersion = BitConverter.ToUInt16(HeaderData, 4);
      MinorVersion = BitConverter.ToUInt16(HeaderData, 6);
      Reserved1 = BitConverter.ToUInt32(HeaderData, 8);
      Reserved2 = BitConverter.ToUInt32(HeaderData, 12);
      SnapLen = BitConverter.ToUInt32(HeaderData, 16);
      LinkType = BitConverter.ToUInt32(HeaderData, 20);
   }

   public bool IsPcapValid()
   {
      //return true;
      return MagicNumber == 0xA1B23C4D || MagicNumber == 0xA1B2C3D4;
   }

   public string GetTimestampFormat()
   {
      switch (MagicNumber)
      {
         case 0xA1B2C3D4:
            return "Microseconds";
         case 0xA1B23C4D:
            return "Nanoseconds";
         default:
            return "UnknownFormat";
      }
   }
}
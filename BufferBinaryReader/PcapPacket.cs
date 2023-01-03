using System.Collections;
using System.Runtime.InteropServices;

namespace BufferStream;

public class PcapPacket  
{
    /*
                           1                   2                   3
        0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      0 |                      Timestamp (Seconds)                      |
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      4 |            Timestamp (Microseconds or nanoseconds)            |
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
      8 |                    Captured Packet Length                     |
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     12 |                    Original Packet Length                     |
        +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
     16 /                                                               /
        /                          Packet Data                          /
        /                        variable length                        /
        /                                                               /
        +---------------------------------------------------------------+
      */
    public const UInt16 PacketInfoLength = 16;
    private UInt32 _timestamp1 { get; }
    private UInt32 _timestamp2 { get; }
    private Boolean IsDataFilled = false;
    public DateTime Timestamp { get; }
    public UInt32 CapturedPacketLength { get; }
    public UInt32 OriginalPacketLength { get; }

    
    private byte[] _packetData; 
    public byte[] PacketData
    {
        get
        {
            return _packetData;
        }
        set
        {
            if (!IsDataFilled)
            {
                _packetData = value;
                IsDataFilled = true;
            }
        }
    }

    public PcapPacket(byte[] RawData)
    {
        if (RawData.Length == PcapPacket.PacketInfoLength)
        {
            _timestamp1 = BitConverter.ToUInt32(RawData, 0);
            _timestamp2 = BitConverter.ToUInt32(RawData, 4);
            Timestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(_timestamp1);
            CapturedPacketLength = BitConverter.ToUInt32(RawData, 8);
            OriginalPacketLength = BitConverter.ToUInt32(RawData, 12);
        }
    }

   
}
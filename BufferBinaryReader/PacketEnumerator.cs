using System.Collections;

namespace BufferStream;

public class PacketEnumerator : IEnumerator<PcapPacket>
{
    private PcapHeader _header;
    private BufferBinaryReader _bufferBinaryReader;
    private PcapPacket _packet;
    public PacketEnumerator(string PcapFileName)
    {
        try
        {
            _bufferBinaryReader = new BufferBinaryReader(PcapFileName);
            _header = new PcapHeader(_bufferBinaryReader.ReadBytes(PcapHeader.PcapHeaderLength));
            if (!_header.IsPcapValid())
            {
                throw new ArgumentException("Not correct file type");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool MoveNext()
    {
       _packet = new PcapPacket(_bufferBinaryReader.ReadBytes(PcapPacket.PacketInfoLength));
       _packet.PacketData = _bufferBinaryReader.ReadBytes(_packet.CapturedPacketLength);
       return _packet.PacketData.Length > 0;
    }

    public void Reset()
    {
        _bufferBinaryReader.Seek(PcapHeader.PcapHeaderLength,SeekOrigin.Begin);
    }

    public PcapPacket Current
    {
        get
        {
            return _packet;
        }
    }

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        _bufferBinaryReader.Dispose();
    }
}
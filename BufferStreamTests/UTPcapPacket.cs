using System;
using BufferStream;
using Xunit;
namespace BufferStreamTests;

public class UTPcapPacket
{
    private PcapPacket _packet;
    private PcapPacket _packet2;
    private PcapHeader _header;
    
    public UTPcapPacket()
    {
        BufferBinaryReader reader = new BufferBinaryReader("Files/TestFile.pcap");
        _header = new PcapHeader(reader.ReadBytes(24));
        _packet = new PcapPacket(reader.ReadBytes(PcapPacket.PacketInfoLength));
        _packet.PacketData = reader.ReadBytes(_packet.CapturedPacketLength);
        _packet2 = new PcapPacket(reader.ReadBytes(PcapPacket.PacketInfoLength));
        _packet2.PacketData = reader.ReadBytes(_packet.CapturedPacketLength); 
    }

    [Fact]
    public void IsCorrectTimestamp()
    {
        Assert.Equal(_packet.Timestamp,new DateTime(2022,08,28,10,12,52));
    }
}
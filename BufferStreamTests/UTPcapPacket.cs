using System;
using BufferStream;
using Xunit;
namespace BufferStreamTests;

public class UTPcapPacket
{
    private PcapPacket _packet;
    private PcapHeader _header;
    
    public UTPcapPacket()
    {
        BufferBinaryReader reader = new BufferBinaryReader("Files/TestFile.pcap");
        CreatePcapHeader(reader);
        CreatePcapPacket(reader);
    }

    public void CreatePcapHeader(BufferBinaryReader reader)
    {
        _header = new PcapHeader(reader.ReadBytes(PcapHeader.PcapHeaderLength));
    }
    public void CreatePcapPacket(BufferBinaryReader reader)
    {
        _packet = new PcapPacket(reader.ReadBytes(PcapPacket.PacketInfoLength));
        _packet.PacketData = reader.ReadBytes(_packet.CapturedPacketLength);
    }

    [Fact]
    public void PacketData_ShouldBeImmutable_AfterCreating()
    {
        var previousData = _packet.PacketData;
        var newData = new byte[_packet.CapturedPacketLength];
        _packet.PacketData = newData;
        Assert.Equal(_packet.PacketData,previousData);
    }
    [Fact]
    public void Timestamp_ShoulbBeCorrect_AfterCreating()
    {
        Assert.Equal(_packet.Timestamp,new DateTime(2022,08,28,10,12,52));
    }
}
using BufferStream;
using Xunit;
namespace BufferStreamTests;

public class UTPcapHeader
{
    private PcapHeader header;

    public UTPcapHeader()
    {
        BufferBinaryReader reader = new BufferBinaryReader("Files/TestFile.pcap");
        header = new PcapHeader(reader.ReadBytes(192));
    }

    [Fact]
    public void IsValidPcapHeader()
    {
        Assert.Equal(header.IsPcapValid(), true);
    }

    [Fact]
    public void GetTimestamp()
    {
        Assert.Equal(header.GetTimestampFormat(),"Microseconds");
    }
}
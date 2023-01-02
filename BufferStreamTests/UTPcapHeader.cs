using BufferStream;
using Xunit;
namespace BufferStreamTests;

public class UTPcapHeader
{
    private PcapHeader header;

    public UTPcapHeader()
    {
        BufferBinaryReader reader = new BufferBinaryReader("Files/TestFile.pcap");
        header = new PcapHeader(reader.ReadBytes(PcapHeader.PcapHeaderLength));
    }

    [Fact]
    public void IsPcapValid_ShouldReturnTrue_IfPcapFile()
    {
        Assert.Equal(header.IsPcapValid(), true);
    }

    [Fact]
    public void GetimestampFormat_ShouldReturnMicroseconds_WhenMagicNumber0xA1B2C3D4()
    {
        Assert.Equal(header.GetTimestampFormat(),"Microseconds");
   }
}
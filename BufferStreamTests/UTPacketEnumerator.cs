using BufferStream;
using Xunit;

namespace BufferStreamTests;

public class UTPacketEnumerator
{ 

    [Theory]
    [InlineData(@"Files/TestFile.pcap", 1602)]
    [InlineData(@"Files/TestFile2.pcap", 13623)]
    public void CountPackets_ShouldReturnCorrectValue_WhenIteratingPackets(string PcapFileName, ulong ActualCount)
    {
        var _packets = new PacketEnumerator(PcapFileName);
        ulong count = 0;
        while (_packets.MoveNext())
        {
            count++;
        }
        Assert.Equal(count,ActualCount);
    }
}
using Xunit;
using BufferStream;
using System;
using System.Linq;
using System.IO;

namespace BufferStreamTests
{
    public class UTBufferBinaryReader
    {
        [Fact]
        public void CreatingClass_WrongFileName_ReturnsExecptions()
        {
            Assert.Throws<ArgumentException>(
                () => _ = new BufferBinaryReader(@"BufferBinaryReader\BufferStreamTests\Files\Wrong.pcap")
                );
        }

        [Fact]
        public void Read10Bytes_FileName_RetursByteArray10()
        {
            var reader = new BufferBinaryReader(@"Files/TestFile.pcap");
            byte[] data = reader.ReadBytes(10);
            Assert.Equal(data.Length, 10);
        }

        /*[Fact]
        public void Read10000Bytes_FileName_RetursBufferSize()
        {
            var reader = new BufferBinaryReader(@"Files/TestFile.pcap");
            byte[] data = reader.ReadBytes(10000);
            Assert.Equal(data.Length, 4096);
        }*/
        [Theory]
        [InlineData(@"Files/TestFile.pcap", 1178124)]
        [InlineData(@"Files/TestFile2.pcap", 6702197)]
        public void EndOfStream_FileName_ReturnFileSize(string FileName, UInt64 FileSize)
        {
            var reader = new BufferBinaryReader(FileName);
            UInt64 fileSize = 0;
            int readSize = reader.ReadBytes(100000).Length;
            while (readSize > 0)
            {
                fileSize += (UInt64)Math.Min(readSize, 100000);
                readSize = reader.ReadBytes(100000).Length;
            }

            Assert.Equal(fileSize, (UInt64)FileSize);
        }
        [Theory]
        [InlineData(@"Files/TestFile.pcap", 1178124)]
        [InlineData(@"Files/TestFile2.pcap", 6702197)]
        public void ProperlyDataRead_FileName_ReturnsDifferenceCount(string FileName,UInt64 FileSize)
        {
            var reader = new BufferBinaryReader(FileName);
            bool IsEqual = true;
            using (var stream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                {
                    using (var readerBinary = new BinaryReader(stream))
                    {
                        for (UInt64 current = 0; current <= FileSize; current += 1000)
                        {
                            var dataFromActualReader = readerBinary.ReadBytes(1000);
                            var dataFromExpectedReader = reader.ReadBytes(1000);
                            IsEqual &= dataFromActualReader.SequenceEqual(dataFromExpectedReader);
                            //IsEqual &= dataFromActualReader.Equals(dataFromExpectedReader);
                        }
                    }
                }
            }
            Assert.Equal(IsEqual, true);
        }
       
        [Theory]
        [InlineData(@"Files/TestFile.pcap", 1178124)]
        [InlineData(@"Files/TestFile2.pcap", 6702197)]
        public void FileSizeUsingSeek_FileName_ReturnsFileSize(string FileName, UInt64 FileSize)
        {
            var reader = new BufferBinaryReader(FileName);
            reader.Seek(0, SeekOrigin.End);
            var fileSize = reader.GerCurrentOffset();
            Assert.Equal((long)FileSize, fileSize);
        }   

        [Fact]
        public void GetOffsets_FileName_ReturnsOffsets()
        {
            var reader = new BufferBinaryReader(@"Files/TestFile.pcap");
            reader.ReadBytes(1000);
            Assert.Equal(reader.GerCurrentOffset(), 1000);
            Assert.Equal(reader.GetPreviousOffset(), 0);

            reader.ReadBytes(1000);
            Assert.Equal(reader.GerCurrentOffset(), 2000);
            Assert.Equal(reader.GetPreviousOffset(), 1000);

            reader.Seek(0, SeekOrigin.Begin);
            Assert.Equal(reader.GerCurrentOffset(), 0);
            Assert.Equal(reader.GetPreviousOffset(), 2000);
        }
        
    }
}

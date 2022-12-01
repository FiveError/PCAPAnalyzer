
namespace BufferStream
{

    public class BufferBinaryReader : IDisposable
    {
        private UInt32 _bufferSize;
        private byte[] _buffer;
        private UInt32 _bufferOffset;
        private Int64 _readDataSize;
        private Int64 _previousOffset;
        private Int64 _offset;
        private string _fileName;
        private FileStream _stream;
        private BinaryReader _reader;
        
        public void Dispose()
        {
            _reader.Close();
            _stream.Close();
        }
        public BufferBinaryReader(string FileName, UInt32 BufferSize = 4096)
        {
            _fileName = FileName;
            if (!IsFileExists())
            {
                throw new ArgumentException();
            }
            _bufferOffset = 0;
            _readDataSize = 0;
            _bufferSize = BufferSize;
            _buffer = new byte[_bufferSize];
            _stream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
            _reader = new BinaryReader(_stream);
        }
        public byte[] ReadBytes(UInt32 Size)
        {
            if (_bufferOffset >= _readDataSize)
            {
                UpdateBuffer();
            }
            if (_bufferOffset + Size > _bufferSize)
            {
                UpdateBuffer();
            }
            UInt32 readDataSize = (UInt32)Math.Min(_readDataSize - _bufferOffset, Size);
            byte[] readBytes = new byte[readDataSize];
            Buffer.BlockCopy(_buffer, (Int32)_bufferOffset, readBytes, 0, (Int32)readDataSize);
            _bufferOffset += readDataSize;
            _previousOffset = _offset;
            _offset += readDataSize;
            return readBytes;

        }
        public void Seek(Int64 Offset,SeekOrigin Origin)
        {
            _previousOffset = _offset;
            _offset = _stream.Seek(Offset, Origin);

        }
        public Int64 GerCurrentOffset()
        {
            return _offset;
        }

        public Int64 GetPreviousOffset()
        {
            return _previousOffset;
        }
        private void UpdateBuffer()
        {
            var remainDataSize = _readDataSize - _bufferOffset;
            Buffer.BlockCopy(_buffer, (Int32)_bufferOffset, _buffer, 0, (Int32)remainDataSize);
            _bufferOffset = 0;
            _readDataSize = _reader.Read(_buffer, (Int32)remainDataSize, (Int32)(_bufferSize - remainDataSize));
            _readDataSize += remainDataSize;

        }
        private bool IsFileExists()
        {
            return File.Exists(_fileName);
        }

    }
}
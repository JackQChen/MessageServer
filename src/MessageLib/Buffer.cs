using System;
using System.Text;

namespace MessageLib
{
    /// <summary>
    /// Dynamic byte buffer
    /// </summary>
    public class Buffer
    {
        private byte[] _data;
        private int _size;
        private int _offset;

        /// <summary>
        /// Is the buffer empty?
        /// </summary>
        public bool IsEmpty { get { return (_data == null) || (_size == 0); } }
        /// <summary>
        /// Bytes memory buffer
        /// </summary>
        public byte[] Data { get { return _data; } }
        /// <summary>
        /// Bytes memory buffer capacity
        /// </summary>
        public int Capacity { get { return _data.Length; } }
        /// <summary>
        /// Bytes memory buffer size
        /// </summary>
        public int Size { get { return _size; } }
        /// <summary>
        /// Bytes memory buffer offset
        /// </summary>
        public int Offset { get { return _offset; } }

        /// <summary>
        /// Buffer indexer operator
        /// </summary>
        public byte this[int index] { get { return _data[index]; } }

        /// <summary>
        /// Initialize a new expandable buffer with zero capacity
        /// </summary>
        public Buffer() { _data = new byte[0]; _size = 0; _offset = 0; }
        /// <summary>
        /// Initialize a new expandable buffer with the given capacity
        /// </summary>
        public Buffer(int capacity) { _data = new byte[capacity]; _size = 0; _offset = 0; }
        /// <summary>
        /// Initialize a new expandable buffer with the given data
        /// </summary>
        public Buffer(byte[] data) { _data = data; _size = data.Length; _offset = 0; }

        #region Memory buffer methods

        /// <summary>
        /// Get string from the current buffer
        /// </summary>
        public override string ToString()
        {
            return ExtractString(0, _size);
        }

        // Clear the current buffer and its offset
        public void Clear()
        {
            _size = 0;
            _offset = 0;
        }

        /// <summary>
        /// Extract the string from buffer of the given offset and size
        /// </summary>
        public string ExtractString(int offset, int size)
        {
            if ((offset + size) > Size)
                throw new ArgumentException("Invalid offset & size!", "offset");

            return Encoding.UTF8.GetString(_data, offset, size);
        }

        /// <summary>
        /// Remove the buffer of the given offset and size
        /// </summary>
        public void Remove(int offset, int size)
        {
            if ((offset + size) > Size)
                throw new ArgumentException("Invalid offset & size!", "offset");

            Array.Copy(_data, offset + size, _data, offset, _size - size - offset);
            _size -= size;
            if (_offset >= (offset + size))
                _offset -= size;
            else if (_offset >= offset)
            {
                _offset -= _offset - offset;
                if (_offset > Size)
                    _offset = Size;
            }
        }

        /// <summary>
        /// Reserve the buffer of the given capacity
        /// </summary>
        public void Reserve(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Invalid reserve capacity!", "capacity");

            if (capacity > Capacity)
            {
                byte[] data = new byte[Math.Max(capacity, 2 * Capacity)];
                Array.Copy(_data, 0, data, 0, _size);
                _data = data;
            }
        }

        // Resize the current buffer
        public void Resize(int size)
        {
            Reserve(size);
            _size = size;
            if (_offset > _size)
                _offset = _size;
        }

        // Shift the current buffer offset
        public void Shift(int offset) { _offset += offset; }
        // Unshift the current buffer offset
        public void Unshift(int offset) { _offset -= offset; }

        #endregion

        #region Buffer I/O methods

        /// <summary>
        /// Append the given buffer
        /// </summary>
        /// <param name="buffer">Buffer to append</param>
        /// <returns>Count of append bytes</returns>
        public int Append(byte[] buffer)
        {
            Reserve(_size + buffer.Length);
            Array.Copy(buffer, 0, _data, _size, buffer.Length);
            _size += buffer.Length;
            return buffer.Length;
        }

        /// <summary>
        /// Append the given buffer fragment
        /// </summary>
        /// <param name="buffer">Buffer to append</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>Count of append bytes</returns>
        public int Append(byte[] buffer, int offset, int size)
        {
            Reserve(_size + size);
            Array.Copy(buffer, offset, _data, _size, size);
            _size += size;
            return size;
        }

        /// <summary>
        /// Append the given text in UTF-8 encoding
        /// </summary>
        /// <param name="text">Text to append</param>
        /// <returns>Count of append bytes</returns>
        public int Append(string text)
        {
            Reserve(_size + Encoding.UTF8.GetMaxByteCount(text.Length));
            int result = Encoding.UTF8.GetBytes(text, 0, text.Length, _data, _size);
            _size += result;
            return result;
        }

        #endregion
    }
}

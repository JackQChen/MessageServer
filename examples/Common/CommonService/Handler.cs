using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonService
{
    public class Handler
    {
        public Handler()
        {
        }

        public event Action<int, Message> MessageReceived;
        public event Action<int, byte[]> RawDataReceived;

        void OnMessageReceived(int id, Message msg)
        {
            if (this.MessageReceived != null)
                this.MessageReceived(id, msg);
        }

        void OnRawDataReceived(int id, byte[] data)
        {
            if (this.RawDataReceived != null)
                this.RawDataReceived(id, data);
        }

        int ArrayCopy(Array sourceArray, int offset, int size, int sourceIndex, Array destinationArray, int destinationIndex)
        {
            int rLength = destinationArray.Length - destinationIndex, aLength = size - sourceIndex;
            int length = rLength < aLength ? rLength : aLength;
            Buffer.BlockCopy(sourceArray, offset + sourceIndex, destinationArray, destinationIndex, length);
            return length;
        }

        public void ProcessReceive(int id, SessionData data, byte[] buffer, int offset, int size)
        {
            int position = 0, length = 0;
            while (position < size)
            {
                if (data.Data == null)
                {
                    if (data.UseRawData)
                    {
                        int aLength = size - position;
                        long rLength = data.RawDataLength - data.Position;
                        var rawData = new byte[rLength < aLength ? rLength : aLength];
                        length = ArrayCopy(buffer, offset, size, position, rawData, 0);
                        data.Position += length;
                        this.OnRawDataReceived(id, rawData);
                        if (data.Position == data.RawDataLength)
                        {
                            data.UseRawData = false;
                            data.Position = 0;
                        }
                    }
                    else
                    {
                        length = ArrayCopy(buffer, offset, size, position, data.Head, Convert.ToInt32(data.Position));
                        data.Position += length;
                        if (data.Position == data.Head.Length)
                        {
                            data.Data = new byte[GetHead(data.Head)];
                            data.Position = 0;
                        }
                    }
                }
                else
                {
                    length = ArrayCopy(buffer, offset, size, position, data.Data, Convert.ToInt32(data.Position));
                    data.Position += length;
                    if (data.Position == data.Data.Length)
                    {
                        var message = FormatterByteObject(data.Data) as Message;
                        if (message.Type == MessageType.RawData)
                        {
                            data.UseRawData = true;
                            data.RawDataLength = (message as FileInfoMessage).FileSize;
                        }
                        this.OnMessageReceived(id, message);
                        data.Data = null;
                        data.Position = 0;
                    }
                }
                position += length;
            }
        }

        int GetHead(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        byte[] SetHead(byte[] bytes)
        {
            byte[] bHead = BitConverter.GetBytes(bytes.Length);
            byte[] bRst = new byte[bytes.Length + 4];
            Buffer.BlockCopy(bHead, 0, bRst, 0, bHead.Length);
            Buffer.BlockCopy(bytes, 0, bRst, bHead.Length, bytes.Length);
            return bRst;
        }

        public byte[] FormatterMessageBytes(Message message)
        {
            return SetHead(FormatterObjectBytes(message));
        }

        byte[] FormatterObjectBytes(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj is null");
            byte[] buff;
            using (var ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                buff = ms.GetBuffer();
            }
            return buff;
        }

        object FormatterByteObject(byte[] buff)
        {
            if (buff == null)
                throw new ArgumentNullException("buff is null");
            object obj;
            using (var ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = iFormatter.Deserialize(ms);
            }
            return obj;
        }
    }
}

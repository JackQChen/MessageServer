using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonService
{
    public static class FormatterExtension
    {
        public static T ToObject<T>(this byte[] buffer, int index, int count) where T : class
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer is null");
            object obj;
            using (var ms = new MemoryStream(buffer, index, count))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = iFormatter.Deserialize(ms);
            }
            return obj as T;
        }

        public static byte[] ToBytes(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj is null");
            byte[] result, header, body;
            using (var ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                body = ms.GetBuffer();
            }
            header = BitConverter.GetBytes(body.Length);
            result = new byte[header.Length + body.Length];
            Array.Copy(header, result, header.Length);
            Array.Copy(body, 0, result, header.Length, body.Length);
            return result;
        }
    }
}

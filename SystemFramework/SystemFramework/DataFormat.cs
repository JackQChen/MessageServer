using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemFramework
{
    /// <summary>
    ///DataFormat 的摘要说明
    ///描述:包括数据的压缩，对象与byte[]之间的转换
    /// </summary>
    public class DataFormat
    {

        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
            stream.Write(data, 0, data.Length);
            stream.Close();
            stream.Dispose();
            //必须把stream流关闭才能返回ms流数据,不然数据会不完整
            //并且解压缩方法stream.Read(buffer, 0, buffer.Length)时会返回0
            bData = ms.ToArray();
            ms.Close();
            ms.Dispose();
            return bData;
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            byte[] bData;
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            GZipStream stream = new GZipStream(ms, CompressionMode.Decompress, true);
            byte[] buffer = new byte[1024];
            MemoryStream temp = new MemoryStream();
            int read = stream.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                temp.Write(buffer, 0, read);
                read = stream.Read(buffer, 0, buffer.Length);
            }
            //必须把stream流关闭才能返回ms流数据,不然数据会不完整
            stream.Close();
            stream.Dispose();
            ms.Close();
            ms.Dispose();
            bData = temp.ToArray();
            temp.Close();
            temp.Dispose();
            return bData;
        }

        /// <summary>
        /// 将object格式化成字节数组byte[]
        /// </summary>
        /// <param name="dsOriginal">object对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatData(object objOriginal)
        {
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            brFormatter.Serialize(memStream, objOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return binaryDataResult;
        }

        /// <summary>
        /// 将字节数组反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static object RetrieveObject(byte[] binaryData)
        {
            MemoryStream memStream = new MemoryStream(binaryData);
            IFormatter brFormatter = new BinaryFormatter();
            Object obj = brFormatter.Deserialize(memStream);
            return obj;
        }

        /// <summary>
        /// 将字节数组反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static T RetrieveObject<T>(byte[] binaryData)
        {
            if (binaryData == null)
                return default(T);
            MemoryStream memStream = new MemoryStream(binaryData);
            IFormatter brFormatter = new BinaryFormatter();
            return (T)brFormatter.Deserialize(memStream);
        }

        /// <summary>
        /// 将objec格式化成字节数组byte[]，并压缩
        /// </summary>
        /// <param name="dsOriginal">泛型对象</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBinaryFormatDataCompress(object objOriginal)
        {
            if (objOriginal == null)
                return null;
            byte[] binaryDataResult = null;
            MemoryStream memStream = new MemoryStream();
            IFormatter brFormatter = new BinaryFormatter();
            brFormatter.Serialize(memStream, objOriginal);
            binaryDataResult = memStream.ToArray();
            memStream.Close();
            memStream.Dispose();
            return Compress(binaryDataResult);
        }

        /// <summary>
        /// 将字节数组解压后反序列化成object对象
        /// </summary>
        /// <param name="binaryData">字节数组</param>
        /// <returns>object对象</returns>
        public static T RetrieveObjectDecompress<T>(byte[] binaryData)
        {
            if (binaryData == null)
                return default(T);
            MemoryStream memStream = new MemoryStream(Decompress(binaryData));
            IFormatter brFormatter = new BinaryFormatter();
            return (T)brFormatter.Deserialize(memStream);
        }
    }
}
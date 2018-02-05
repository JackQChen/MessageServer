
using System;
namespace SystemFramework
{
    public class Common
    {
        public static string GetGuidString()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0).ToString();
        }
    }
}
